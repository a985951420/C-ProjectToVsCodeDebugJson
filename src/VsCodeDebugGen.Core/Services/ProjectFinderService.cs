using VsCodeDebugGen.Core.Interfaces;

namespace VsCodeDebugGen.Core.Services;

/// <summary>
/// 项目查找服务
/// </summary>
public class ProjectFinderService : IProjectFinder
{
    /// <summary>
    /// 在指定路径查找所有 .csproj 文件
    /// </summary>
    public IEnumerable<string> FindProjects(string searchPath)
    {
        if (string.IsNullOrWhiteSpace(searchPath))
        {
            throw new ArgumentException("搜索路径不能为空", nameof(searchPath));
        }

        if (!Directory.Exists(searchPath))
        {
            throw new DirectoryNotFoundException($"目录不存在: {searchPath}");
        }

        try
        {
            return Directory.GetFiles(searchPath, "*.csproj", SearchOption.AllDirectories)
                           .OrderBy(f => f);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"没有权限访问目录: {searchPath}", ex);
        }
    }
}
