/// <summary>
/// 项目信息结构体，保存 .csproj 解析后的关键信息
/// </summary>
class ProjectInfo
{
    /// <summary>
    /// .csproj 文件路径
    /// </summary>
    public required string CsprojFile { get; init; }

    /// <summary>
    /// 输出类型（Exe/Library）
    /// </summary>
    public required string OutputType { get; init; }

    /// <summary>
    /// 目标框架
    /// </summary>
    public required string TargetFramework { get; init; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    public required string AssemblyName { get; init; }

    /// <summary>
    /// 输出路径
    /// </summary>
    public required string OutputPath { get; init; }

    /// <summary>
    /// VSCode 调试配置名称
    /// </summary>
    public string LaunchName => $"Launch {AssemblyName}";
}