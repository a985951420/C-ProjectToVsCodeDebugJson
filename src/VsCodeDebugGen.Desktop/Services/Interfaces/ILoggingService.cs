using System.Collections.ObjectModel;
using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 日志服务接口
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// 日志集合
    /// </summary>
    ObservableCollection<LogEntry> Logs { get; }

    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="message">日志消息</param>
    /// <param name="level">日志级别</param>
    void Log(string message, LogLevel level = LogLevel.Info);

    /// <summary>
    /// 清空日志
    /// </summary>
    void Clear();

    /// <summary>
    /// 导出日志到文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    Task ExportToFileAsync(string filePath);
}
