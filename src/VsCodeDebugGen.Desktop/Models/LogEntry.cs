namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 日志条目模型
/// </summary>
public class LogEntry
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 日志级别
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// 日志消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 图标（根据日志级别自动生成）
    /// </summary>
    public string Icon => Level switch
    {
        LogLevel.Debug => "🔍",
        LogLevel.Info => "ℹ",
        LogLevel.Success => "✓",
        LogLevel.Warning => "⚠",
        LogLevel.Error => "✗",
        _ => "•"
    };

    /// <summary>
    /// 颜色（根据日志级别自动生成）
    /// </summary>
    public string Color => Level switch
    {
        LogLevel.Debug => "#808080",
        LogLevel.Info => "#0078D4",
        LogLevel.Success => "#107C10",
        LogLevel.Warning => "#FF8C00",
        LogLevel.Error => "#D13438",
        _ => "#000000"
    };
}
