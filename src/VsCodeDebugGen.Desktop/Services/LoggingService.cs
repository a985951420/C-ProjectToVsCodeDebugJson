using System.Collections.ObjectModel;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 日志服务实现
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    /// <summary>
    /// 日志集合
    /// </summary>
    public ObservableCollection<LogEntry> Logs { get; } = new();

    /// <summary>
    /// 初始化日志服务
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="level">日志级别</param>
    public void Log(string message, Models.LogLevel level = Models.LogLevel.Info)
    {
        var logEntry = new LogEntry
        {
            Message = message,
            Level = level,
            Timestamp = DateTime.Now
        };

        // 在 UI 线程上添加日志
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            Logs.Add(logEntry);
        });

        // 同时记录到文件日志
        switch (level)
        {
            case Models.LogLevel.Debug:
                _logger.LogDebug(message);
                break;
            case Models.LogLevel.Info:
                _logger.LogInformation(message);
                break;
            case Models.LogLevel.Success:
                _logger.LogInformation("[SUCCESS] {Message}", message);
                break;
            case Models.LogLevel.Warning:
                _logger.LogWarning(message);
                break;
            case Models.LogLevel.Error:
                _logger.LogError(message);
                break;
        }
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    public void Clear()
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            Logs.Clear();
        });
        _logger.LogInformation("日志已清空");
    }

    /// <summary>
    /// 导出日志到文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    public async Task ExportToFileAsync(string filePath)
    {
        try
        {
            var lines = Logs.Select(log =>
                $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] [{log.Level}] {log.Message}");

            await File.WriteAllLinesAsync(filePath, lines);
            _logger.LogInformation("日志已导出到: {FilePath}", filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "导出日志失败: {FilePath}", filePath);
            throw;
        }
    }
}
