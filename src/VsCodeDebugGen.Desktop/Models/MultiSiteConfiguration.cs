namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 多站点配置模型
/// </summary>
public class MultiSiteConfiguration
{
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// 站点配置列表
    /// </summary>
    public List<SiteConfig> Sites { get; set; } = new();
}

/// <summary>
/// 站点配置
/// </summary>
public class SiteConfig
{
    /// <summary>
    /// 配置名称（如 Development, Staging, Production）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// HTTP 端口
    /// </summary>
    public int HttpPort { get; set; }

    /// <summary>
    /// HTTPS 端口
    /// </summary>
    public int HttpsPort { get; set; }

    /// <summary>
    /// 环境变量
    /// </summary>
    public Dictionary<string, string> EnvironmentVariables { get; set; } = new();
}
