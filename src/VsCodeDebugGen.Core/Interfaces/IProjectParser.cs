using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Interfaces;

/// <summary>
/// 项目解析器接口
/// </summary>
public interface IProjectParser
{
    /// <summary>
    /// 解析项目文件
    /// </summary>
    /// <param name="projectPath">项目文件路径</param>
    /// <param name="basePath">基础路径</param>
    /// <returns>解析后的项目信息，失败返回 null</returns>
    ProjectInfo? Parse(string projectPath, string basePath);

    /// <summary>
    /// 批量解析项目文件
    /// </summary>
    /// <param name="projectPaths">项目文件路径集合</param>
    /// <param name="basePath">基础路径</param>
    /// <returns>解析成功的项目信息列表</returns>
    IEnumerable<ProjectInfo> ParseMany(IEnumerable<string> projectPaths, string basePath);
}
