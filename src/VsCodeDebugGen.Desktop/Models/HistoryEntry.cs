namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 历史记录条目
/// </summary>
public class HistoryEntry
{
    /// <summary>
    /// 唯一标识符（字符串形式）
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 时间戳
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// 搜索路径
    /// </summary>
    public string SearchPath { get; set; } = string.Empty;

    /// <summary>
    /// 项目路径
    /// </summary>
    public string ProjectPath { get; set; } = string.Empty;

    /// <summary>
    /// 输出路径
    /// </summary>
    public string OutputPath { get; set; } = string.Empty;

    /// <summary>
    /// 项目数量
    /// </summary>
    public int ProjectCount { get; set; }

    /// <summary>
    /// 项目名称列表
    /// </summary>
    public List<string> ProjectNames { get; set; } = new();

    /// <summary>
    /// 生成状态
    /// </summary>
    public GenerationStatus Status { get; set; }

    /// <summary>
    /// 错误消息（如果有）
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// 生成状态枚举
/// </summary>
public enum GenerationStatus
{
    /// <summary>
    /// 成功
    /// </summary>
    Success,

    /// <summary>
    /// 部分成功
    /// </summary>
    PartialSuccess,

    /// <summary>
    /// 失败
    /// </summary>
    Failed
}
