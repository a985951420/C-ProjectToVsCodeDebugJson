namespace VsCodeDebugGen.Core.Interfaces;

/// <summary>
/// 项目查找器接口
/// </summary>
public interface IProjectFinder
{
    /// <summary>
    /// 在指定路径查找所有 .csproj 文件
    /// </summary>
    /// <param name="searchPath">搜索路径</param>
    /// <returns>找到的项目文件路径集合</returns>
    IEnumerable<string> FindProjects(string searchPath);
}
