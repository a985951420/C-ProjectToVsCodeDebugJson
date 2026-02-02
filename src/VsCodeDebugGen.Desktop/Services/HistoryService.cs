using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 历史记录服务实现
/// </summary>
public class HistoryService : IHistoryService
{
    private readonly ILogger<HistoryService> _logger;
    private readonly string _historyFilePath;
    private List<HistoryEntry> _historyEntries;
    private const int MaxHistoryCount = 100;

    /// <summary>
    /// 初始化历史记录服务
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public HistoryService(ILogger<HistoryService> logger)
    {
        _logger = logger;

        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VsCodeDebugGen");

        Directory.CreateDirectory(appDataPath);

        _historyFilePath = Path.Combine(appDataPath, "history.json");
        _historyEntries = new List<HistoryEntry>();
    }

    /// <summary>
    /// 加载所有历史记录
    /// </summary>
    /// <returns>历史记录列表</returns>
    public async Task<List<HistoryEntry>> LoadHistoryAsync()
    {
        try
        {
            if (File.Exists(_historyFilePath))
            {
                var json = await File.ReadAllTextAsync(_historyFilePath);
                _historyEntries = JsonSerializer.Deserialize<List<HistoryEntry>>(json)
                                  ?? new List<HistoryEntry>();
            }

            // 按时间倒序排序
            _historyEntries = _historyEntries.OrderByDescending(h => h.Timestamp).ToList();

            _logger.LogDebug("已加载 {Count} 条历史记录", _historyEntries.Count);
            return _historyEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加载历史记录失败");
            _historyEntries = new List<HistoryEntry>();
            return _historyEntries;
        }
    }

    /// <summary>
    /// 添加历史记录
    /// </summary>
    /// <param name="entry">历史条目</param>
    public async Task AddHistoryEntryAsync(HistoryEntry entry)
    {
        try
        {
            entry.Timestamp = DateTime.Now;

            _historyEntries.Insert(0, entry);

            // 限制历史记录数量
            if (_historyEntries.Count > MaxHistoryCount)
            {
                _historyEntries = _historyEntries.Take(MaxHistoryCount).ToList();
            }

            await SaveHistoryAsync();

            _logger.LogInformation("已添加历史记录: {ProjectPath}", entry.ProjectPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "添加历史记录失败");
            throw;
        }
    }

    /// <summary>
    /// 删除历史记录
    /// </summary>
    /// <param name="id">历史记录 ID</param>
    public async Task DeleteHistoryEntryAsync(Guid id)
    {
        try
        {
            var entry = _historyEntries.FirstOrDefault(h => h.Id == id.ToString());
            if (entry != null)
            {
                _historyEntries.Remove(entry);
                await SaveHistoryAsync();

                _logger.LogInformation("已删除历史记录: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除历史记录失败: {Id}", id);
            throw;
        }
    }

    /// <summary>
    /// 清空所有历史记录
    /// </summary>
    public async Task ClearHistoryAsync()
    {
        try
        {
            _historyEntries.Clear();
            await SaveHistoryAsync();

            _logger.LogInformation("历史记录已清空");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "清空历史记录失败");
            throw;
        }
    }

    /// <summary>
    /// 搜索历史记录
    /// </summary>
    /// <param name="keyword">搜索关键字</param>
    /// <returns>匹配的历史记录列表</returns>
    public async Task<List<HistoryEntry>> SearchHistoryAsync(string keyword)
    {
        try
        {
            if (_historyEntries.Count == 0)
            {
                await LoadHistoryAsync();
            }

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return _historyEntries;
            }

            return _historyEntries.Where(h =>
                h.ProjectPath.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "搜索历史记录失败: {Keyword}", keyword);
            return new List<HistoryEntry>();
        }
    }

    /// <summary>
    /// 保存历史记录到文件
    /// </summary>
    private async Task SaveHistoryAsync()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(_historyEntries, options);
            await File.WriteAllTextAsync(_historyFilePath, json);

            _logger.LogDebug("历史记录已保存到: {Path}", _historyFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存历史记录失败");
            throw;
        }
    }
}
