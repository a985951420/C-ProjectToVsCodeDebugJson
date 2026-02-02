using System.Xml.Linq;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Services;

/// <summary>
/// 项目解析服务
/// </summary>
public class ProjectParserService : IProjectParser
{
    /// <summary>
    /// 解析单个项目文件
    /// </summary>
    public ProjectInfo? Parse(string projectPath, string basePath)
    {
        if (string.IsNullOrWhiteSpace(projectPath))
        {
            return null;
        }

        if (!File.Exists(projectPath))
        {
            Console.WriteLine($"警告: 项目文件不存在 - {projectPath}");
            return null;
        }

        try
        {
            var doc = XDocument.Load(projectPath);
            var (outputType, targetFramework, assemblyName, outputPath) =
                ParseProjectProperties(doc, projectPath);

            // 获取目标框架
            string finalFramework = GetTargetFramework(targetFramework, basePath);
            if (string.IsNullOrEmpty(finalFramework))
            {
                Console.WriteLine($"警告: 无法确定项目 {Path.GetFileName(projectPath)} 的目标框架");
                return null;
            }

            // 设置输出路径
            string finalOutputPath = string.IsNullOrEmpty(outputPath)
                ? Path.Combine("bin", "Debug", finalFramework)
                : outputPath;

            return new ProjectInfo
            {
                CsprojFile = projectPath,
                OutputType = outputType,
                TargetFramework = finalFramework,
                AssemblyName = assemblyName,
                OutputPath = finalOutputPath
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: 解析项目文件失败 - {projectPath}");
            Console.WriteLine($"详细信息: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 批量解析项目文件
    /// </summary>
    public IEnumerable<ProjectInfo> ParseMany(IEnumerable<string> projectPaths, string basePath)
    {
        var results = new List<ProjectInfo>();

        foreach (var path in projectPaths)
        {
            var info = Parse(path, basePath);
            if (info != null)
            {
                results.Add(info);
            }
        }

        return results;
    }

    /// <summary>
    /// 解析项目属性
    /// </summary>
    private (string outputType, string targetFramework, string assemblyName, string outputPath)
        ParseProjectProperties(XDocument doc, string projectPath)
    {
        string outputType = "Exe";
        string targetFramework = string.Empty;
        string assemblyName = Path.GetFileNameWithoutExtension(projectPath);
        string outputPath = string.Empty;

        var propertyGroups = doc.Descendants("PropertyGroup");
        foreach (var group in propertyGroups)
        {
            outputType = group.Element("OutputType")?.Value ?? outputType;
            targetFramework = GetFrameworkFromGroup(group, targetFramework);
            assemblyName = group.Element("AssemblyName")?.Value ?? assemblyName;
            outputPath = group.Element("OutputPath")?.Value ?? outputPath;
        }

        return (outputType, targetFramework, assemblyName, outputPath);
    }

    /// <summary>
    /// 从 PropertyGroup 获取目标框架
    /// </summary>
    private string GetFrameworkFromGroup(XElement group, string current)
    {
        // 单个目标框架
        string framework = group.Element("TargetFramework")?.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(framework))
        {
            return framework;
        }

        // 多目标框架，取第一个
        string frameworks = group.Element("TargetFrameworks")?.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(frameworks))
        {
            return frameworks.Split(';', StringSplitOptions.RemoveEmptyEntries)[0].Trim();
        }

        return current;
    }

    /// <summary>
    /// 获取目标框架，如果项目文件中没有，尝试从 Directory.Build.props 获取
    /// </summary>
    private string GetTargetFramework(string initialFramework, string basePath)
    {
        if (!string.IsNullOrEmpty(initialFramework))
        {
            return initialFramework;
        }

        // 尝试从 Directory.Build.props 读取
        string propsPath = Path.Combine(basePath, "Directory.Build.props");
        if (!File.Exists(propsPath))
        {
            return string.Empty;
        }

        try
        {
            var doc = XDocument.Load(propsPath);
            var propertyGroups = doc.Descendants("PropertyGroup");

            foreach (var group in propertyGroups)
            {
                string framework = GetFrameworkFromGroup(group, string.Empty);
                if (!string.IsNullOrEmpty(framework))
                {
                    return framework;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"警告: 读取 Directory.Build.props 失败 - {ex.Message}");
        }

        return string.Empty;
    }
}
