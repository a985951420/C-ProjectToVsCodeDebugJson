using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 模板模型
/// </summary>
public class TemplateModel
{
    /// <summary>
    /// 模板唯一标识符
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模板描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 创建日期
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    /// <summary>
    /// 应用配置
    /// </summary>
    public AppConfiguration? Configuration { get; set; }

    /// <summary>
    /// 端口配置列表
    /// </summary>
    public List<PortConfiguration> PortConfigurations { get; set; } = new();

    /// <summary>
    /// 多站点配置列表
    /// </summary>
    public List<MultiSiteConfiguration> MultiSiteConfigurations { get; set; } = new();
}
