namespace VsCodeDebugGen.Core.Models;

/// <summary>
/// 命令行选项模型
/// </summary>
public class CommandLineOptions
{
    /// <summary>
    /// 搜索路径
    /// </summary>
    public string? SearchPath { get; set; }

    /// <summary>
    /// 输出路径
    /// </summary>
    public string? OutputPath { get; set; }

    /// <summary>
    /// 包含的项目（逗号分隔）
    /// </summary>
    public string? Include { get; set; }

    /// <summary>
    /// 排除的项目（逗号分隔）
    /// </summary>
    public string? Exclude { get; set; }

    /// <summary>
    /// 是否交互模式
    /// </summary>
    public bool Interactive { get; set; } = false;

    /// <summary>
    /// 是否显示帮助
    /// </summary>
    public bool ShowHelp { get; set; } = false;

    /// <summary>
    /// 是否显示版本
    /// </summary>
    public bool ShowVersion { get; set; } = false;

    /// <summary>
    /// 是否启用详细输出
    /// </summary>
    public bool Verbose { get; set; } = false;
}
