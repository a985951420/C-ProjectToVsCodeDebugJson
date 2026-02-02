using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 模板服务接口
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// 保存模板
    /// </summary>
    /// <param name="template">模板对象</param>
    Task SaveTemplateAsync(TemplateModel template);

    /// <summary>
    /// 加载模板
    /// </summary>
    /// <param name="name">模板名称</param>
    /// <returns>模板对象</returns>
    Task<TemplateModel?> LoadTemplateAsync(string name);

    /// <summary>
    /// 获取所有模板列表
    /// </summary>
    /// <returns>模板名称列表</returns>
    Task<List<TemplateModel>> GetTemplateListAsync();

    /// <summary>
    /// 删除模板
    /// </summary>
    /// <param name="name">模板名称</param>
    Task DeleteTemplateAsync(string name);

    /// <summary>
    /// 导出模板到文件
    /// </summary>
    /// <param name="template">模板对象</param>
    /// <param name="filePath">文件路径</param>
    Task ExportTemplateAsync(TemplateModel template, string filePath);

    /// <summary>
    /// 从文件导入模板
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns>导入的模板</returns>
    Task<TemplateModel?> ImportTemplateAsync(string filePath);
}
