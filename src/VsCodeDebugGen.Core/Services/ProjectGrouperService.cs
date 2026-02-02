using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Services;

/// <summary>
/// 项目分组服务 - 按相似度和命名模式对项目进行分组
/// </summary>
public class ProjectGrouperService : IProjectGrouper
{
    // 常见的项目类型模式
    private static readonly string[] CommonPatterns = new[]
    {
        "*.Tests", "*.Test", "*Test", "*Tests",     // 测试项目
        "*.Web", "*.WebApi", "*Web", "*Api",        // Web/API 项目
        "*.UI", "*.Desktop", "*.WPF", "*.WinForms", // UI 项目
        "*.Core", "*.Domain",                        // 核心/领域项目
        "*.Infrastructure", "*.Data", "*.Dal",       // 基础设施/数据访问项目
        "*.Service", "*.Services", "*Service",       // 服务项目
        "*.Client", "*.Sdk",                         // 客户端/SDK 项目
        "*.Shared", "*.Common",                      // 共享/通用项目
    };

    /// <summary>
    /// 将项目列表按相似度自动分组
    /// </summary>
    public List<ProjectGroup> GroupProjects(IEnumerable<string> projectPaths)
    {
        var projects = projectPaths.ToList();
        var groups = new List<ProjectGroup>();

        // 1. 按常见模式分组
        foreach (var pattern in CommonPatterns)
        {
            var matchedProjects = projects
                .Where(p => MatchesPattern(Path.GetFileNameWithoutExtension(p), pattern))
                .ToList();

            if (matchedProjects.Any())
            {
                var groupName = GetGroupNameFromPattern(pattern);
                groups.Add(new ProjectGroup
                {
                    GroupName = groupName,
                    Pattern = pattern,
                    Projects = matchedProjects
                });

                // 从列表中移除已分组的项目
                projects = projects.Except(matchedProjects).ToList();
            }
        }

        // 2. 按目录分组剩余项目
        var projectsByDirectory = projects
            .GroupBy(p => Path.GetDirectoryName(p) ?? "")
            .Where(g => g.Count() > 1) // 只分组包含多个项目的目录
            .ToList();

        foreach (var dirGroup in projectsByDirectory)
        {
            var dirName = new DirectoryInfo(dirGroup.Key).Name;
            groups.Add(new ProjectGroup
            {
                GroupName = $"Directory: {dirName}",
                Pattern = dirGroup.Key,
                Projects = dirGroup.ToList()
            });

            projects = projects.Except(dirGroup).ToList();
        }

        // 3. 剩余项目归入"其他"组
        if (projects.Any())
        {
            groups.Add(new ProjectGroup
            {
                GroupName = "Other Projects",
                Pattern = "*",
                Projects = projects
            });
        }

        return groups.OrderByDescending(g => g.Count).ToList();
    }

    /// <summary>
    /// 根据自定义模式分组
    /// </summary>
    public List<ProjectGroup> GroupByPattern(IEnumerable<string> projectPaths, string[] patterns)
    {
        var projects = projectPaths.ToList();
        var groups = new List<ProjectGroup>();

        foreach (var pattern in patterns)
        {
            var matchedProjects = projects
                .Where(p => MatchesPattern(Path.GetFileNameWithoutExtension(p), pattern))
                .ToList();

            if (matchedProjects.Any())
            {
                groups.Add(new ProjectGroup
                {
                    GroupName = GetGroupNameFromPattern(pattern),
                    Pattern = pattern,
                    Projects = matchedProjects
                });

                projects = projects.Except(matchedProjects).ToList();
            }
        }

        // 未匹配的项目
        if (projects.Any())
        {
            groups.Add(new ProjectGroup
            {
                GroupName = "Unmatched",
                Pattern = "*",
                Projects = projects
            });
        }

        return groups;
    }

    /// <summary>
    /// 检查项目名称是否匹配模式
    /// </summary>
    private bool MatchesPattern(string projectName, string pattern)
    {
        // 移除开头的 * 和结尾的扩展名
        var cleanPattern = pattern.TrimStart('*').Replace(".*", "").Replace(".csproj", "");

        if (pattern.StartsWith("*") && pattern.EndsWith("*"))
        {
            // *Pattern* - 包含
            return projectName.Contains(cleanPattern, StringComparison.OrdinalIgnoreCase);
        }
        else if (pattern.StartsWith("*"))
        {
            // *Pattern - 结尾
            return projectName.EndsWith(cleanPattern, StringComparison.OrdinalIgnoreCase);
        }
        else if (pattern.EndsWith("*"))
        {
            // Pattern* - 开头
            return projectName.StartsWith(cleanPattern, StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            // 完全匹配
            return projectName.Equals(cleanPattern, StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 从模式生成友好的分组名称
    /// </summary>
    private string GetGroupNameFromPattern(string pattern)
    {
        var clean = pattern.TrimStart('*').Replace(".*", "").Replace(".csproj", "");

        return clean switch
        {
            "Tests" or "Test" => "Test Projects",
            "Web" or "WebApi" or "Api" => "Web/API Projects",
            "UI" or "Desktop" or "WPF" or "WinForms" => "UI/Desktop Projects",
            "Core" or "Domain" => "Core/Domain Projects",
            "Infrastructure" or "Data" or "Dal" => "Infrastructure/Data Projects",
            "Service" or "Services" => "Service Projects",
            "Client" or "Sdk" => "Client/SDK Projects",
            "Shared" or "Common" => "Shared/Common Projects",
            _ => $"{clean} Projects"
        };
    }
}
