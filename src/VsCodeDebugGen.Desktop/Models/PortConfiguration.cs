namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 端口配置模型
/// </summary>
public class PortConfiguration
{
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// HTTP 端口
    /// </summary>
    public int HttpPort { get; set; } = 5000;

    /// <summary>
    /// HTTPS 端口
    /// </summary>
    public int HttpsPort { get; set; } = 5001;

    /// <summary>
    /// 起始端口
    /// </summary>
    public int StartPort { get; set; } = 5000;

    /// <summary>
    /// 结束端口
    /// </summary>
    public int EndPort { get; set; } = 5999;

    /// <summary>
    /// 自动递增
    /// </summary>
    public bool AutoIncrement { get; set; } = true;

    /// <summary>
    /// 已使用的端口列表
    /// </summary>
    public List<int> UsedPorts { get; set; } = new();

    /// <summary>
    /// 环境名称
    /// </summary>
    public string Environment { get; set; } = "Development";
}
