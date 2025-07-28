/// <summary>
/// 项目信息结构体，保存 .csproj 解析后的关键信息
/// </summary>
class ProjectInfo
{
    /// <summary>
    /// .csproj 文件路径
    /// </summary>
    public string CsprojFile { get; set; }

    /// <summary>
    /// 输出类型（Exe/Library）
    /// </summary>
    public string OutputType { get; set; }

    /// <summary>
    /// 目标框架
    /// </summary>
    public string TargetFramework { get; set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// 输出路径
    /// </summary>
    public string OutputPath { get; set; }

    /// <summary>
    /// VSCode 调试配置名称
    /// </summary>
    public string LaunchName => $"Launch {AssemblyName}";
}