namespace VsCodeDebugGen.Core.Models;

/// <summary>
/// 应用配置模型
/// </summary>
public class AppConfiguration
{
    /// <summary>
    /// 默认搜索路径
    /// </summary>
    public string DefaultSearchPath { get; set; } = string.Empty;

    /// <summary>
    /// 默认输出路径
    /// </summary>
    public string DefaultOutputPath { get; set; } = string.Empty;

    /// <summary>
    /// 包含的项目列表
    /// </summary>
    public List<string> IncludeProjects { get; set; } = new();

    /// <summary>
    /// 排除的项目列表
    /// </summary>
    public List<string> ExcludeProjects { get; set; } = new();

    /// <summary>
    /// 是否启用详细日志
    /// </summary>
    public bool VerboseLogging { get; set; } = false;
}
