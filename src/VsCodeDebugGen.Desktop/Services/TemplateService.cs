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
/// 模板服务实现
/// </summary>
public class TemplateService : ITemplateService
{
    private readonly ILogger<TemplateService> _logger;
    private readonly string _templateDirectory;
    private readonly List<TemplateModel> _templates;

    /// <summary>
    /// 初始化模板服务
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public TemplateService(ILogger<TemplateService> logger)
    {
        _logger = logger;
        _templateDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "VsCodeDebugGen",
            "Templates");

        _templates = new List<TemplateModel>();

        // 确保模板目录存在
        Directory.CreateDirectory(_templateDirectory);
    }

    /// <summary>
    /// 获取所有模板列表
    /// </summary>
    /// <returns>模板名称列表</returns>
    public async Task<List<TemplateModel>> GetTemplateListAsync()
    {
        try
        {
            _templates.Clear();

            if (!Directory.Exists(_templateDirectory))
            {
                return _templates;
            }

            var templateFiles = Directory.GetFiles(_templateDirectory, "*.json");

            foreach (var file in templateFiles)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file);
                    var template = JsonSerializer.Deserialize<TemplateModel>(json);

                    if (template != null)
                    {
                        _templates.Add(template);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "无法加载模板文件: {File}", file);
                }
            }

            _logger.LogInformation("已加载 {Count} 个模板", _templates.Count);
            return _templates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取模板列表失败");
            throw;
        }
    }

    /// <summary>
    /// 加载模板
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <returns>模板对象</returns>
    public async Task<TemplateModel?> LoadTemplateAsync(string name)
    {
        try
        {
            if (_templates.Count == 0)
            {
                await GetTemplateListAsync();
            }

            return _templates.FirstOrDefault(t => t.Name == name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加载模板失败: {Name}", name);
            throw;
        }
    }

    /// <summary>
    /// 保存模板
    /// </summary>
    /// <param name="template">模板</param>
    public async Task SaveTemplateAsync(TemplateModel template)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(template.Id))
            {
                template.Id = Guid.NewGuid().ToString();
            }

            template.CreatedDate = DateTime.Now;

            var fileName = $"{SanitizeFileName(template.Name)}_{template.Id}.json";
            var filePath = Path.Combine(_templateDirectory, fileName);

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(template, options);
            await File.WriteAllTextAsync(filePath, json);

            // 更新内存中的模板列表
            var existingTemplate = _templates.FirstOrDefault(t => t.Id == template.Id);
            if (existingTemplate != null)
            {
                _templates.Remove(existingTemplate);
            }
            _templates.Add(template);

            _logger.LogInformation("模板已保存: {Name}", template.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "保存模板失败: {Name}", template.Name);
            throw;
        }
    }

    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="name">模板名称</param>
    public async Task DeleteTemplateAsync(string name)
    {
        try
        {
            var template = await LoadTemplateAsync(name);
            if (template == null)
            {
                _logger.LogWarning("模板不存在: {Name}", name);
                return;
            }

            // 查找并删除文件
            var templateFiles = Directory.GetFiles(_templateDirectory, $"*_{template.Id}.json");
            foreach (var file in templateFiles)
            {
                File.Delete(file);
            }

            // 从内存中移除
            _templates.RemoveAll(t => t.Name == name);

            _logger.LogInformation("模板已删除: {Name}", name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除模板失败: {Name}", name);
            throw;
        }
    }

    /// <summary>
    /// 导出模板到文件
    /// </summary>
    /// <param name="template">模板对象</param>
    /// <param name="filePath">文件路径</param>
    public async Task ExportTemplateAsync(TemplateModel template, string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(template, options);
            await File.WriteAllTextAsync(filePath, json);

            _logger.LogInformation("模板已导出: {Path}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "导出模板失败");
            throw;
        }
    }

    /// <summary>
    /// 从文件导入模板
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>导入的模板</returns>
    public async Task<TemplateModel?> ImportTemplateAsync(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("文件不存在", filePath);
            }

            var json = await File.ReadAllTextAsync(filePath);
            var template = JsonSerializer.Deserialize<TemplateModel>(json);

            if (template == null)
            {
                throw new InvalidOperationException("无效的模板文件");
            }

            // 生成新的 ID 避免冲突
            template.Id = Guid.NewGuid().ToString();
            await SaveTemplateAsync(template);

            _logger.LogInformation("模板已导入: {Name}", template.Name);
            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "导入模板失败: {Path}", filePath);
            throw;
        }
    }

    /// <summary>
    /// 清理文件名中的非法字符
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>清理后的文件名</returns>
    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(fileName.Select(c =>
            invalidChars.Contains(c) ? '_' : c).ToArray());

        return string.IsNullOrWhiteSpace(sanitized) ? "template" : sanitized;
    }
}
