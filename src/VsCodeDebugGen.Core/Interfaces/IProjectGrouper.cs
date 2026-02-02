using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Interfaces;

/// <summary>
/// 项目分组服务接口
/// </summary>
public interface IProjectGrouper
{
    /// <summary>
    /// 将项目列表按相似度分组
    /// </summary>
    List<ProjectGroup> GroupProjects(IEnumerable<string> projectPaths);

    /// <summary>
    /// 根据命名模式分组
    /// </summary>
    List<ProjectGroup> GroupByPattern(IEnumerable<string> projectPaths, string[] patterns);
}
