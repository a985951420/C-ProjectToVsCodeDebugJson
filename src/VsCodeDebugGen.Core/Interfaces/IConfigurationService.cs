using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Interfaces;

/// <summary>
/// 配置服务接口
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// 加载应用配置
    /// </summary>
    /// <returns>应用配置</returns>
    AppConfiguration Load();

    /// <summary>
    /// 保存应用配置
    /// </summary>
    /// <param name="config">应用配置</param>
    void Save(AppConfiguration config);

    /// <summary>
    /// 获取包含项目列表
    /// </summary>
    /// <returns>项目名称列表</returns>
    List<string> GetIncludeList();

    /// <summary>
    /// 保存包含项目列表
    /// </summary>
    /// <param name="includeList">项目名称列表</param>
    void SaveIncludeList(List<string> includeList);
}
