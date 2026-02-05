using System.Text;
using System.Text.Json;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Models;
using VsCodeDebugGen.Core.Models.VsCode;

namespace VsCodeDebugGen.Core.Services;

/// <summary>
/// VSCode 配置生成服务
/// </summary>
public class ConfigGeneratorService : IConfigGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// 生成 VSCode 配置文件
    /// </summary>
    public void Generate(IEnumerable<ProjectInfo> projects, string outputPath, string basePath)
    {
        var projectList = projects.ToList();
        if (!projectList.Any())
        {
            throw new InvalidOperationException("没有可用的项目信息生成配置");
        }

        // 创建 .vscode 目录
        string vscodeDir = Path.Combine(outputPath, ".vscode");
        Directory.CreateDirectory(vscodeDir);

        // 生成配置文件
        GenerateLaunchJson(vscodeDir, projectList, basePath);
        GenerateTasksJson(vscodeDir, projectList, basePath);

        Console.WriteLine($"✓ 配置文件已生成到: {vscodeDir}");
    }

    /// <summary>
    /// 生成 launch.json
    /// </summary>
    private void GenerateLaunchJson(string vscodeDir, List<ProjectInfo> projects, string basePath)
    {
        var configurations = projects.Select(project => CreateLaunchConfiguration(project, basePath)).ToArray();

        var launchConfig = new LaunchConfig
        {
            Configurations = configurations
        };

        string json = JsonSerializer.Serialize(launchConfig, JsonOptions);
        string filePath = Path.Combine(vscodeDir, "launch.json");
        File.WriteAllText(filePath, json, Encoding.UTF8);

        Console.WriteLine($"  - launch.json ({configurations.Length} 个配置)");
    }

    /// <summary>
    /// 创建启动配置
    /// </summary>
    private LaunchConfiguration CreateLaunchConfiguration(ProjectInfo project, string basePath)
    {
        string projectDir = project.ProjectDirectory;
        string relativeDir = GetRelativePath(basePath, projectDir);
        string programPath = $"${{workspaceFolder}}{relativeDir}/{project.OutputPath}/{project.AssemblyName}.dll"
            .Replace("\\", "/");

        var env = new Dictionary<string, string>
        {
            { "ASPNETCORE_ENVIRONMENT", "Development" }
        };

        // 如果配置了端口，添加 ASPNETCORE_URLS 环境变量
        if (project.Port.HasValue)
        {
            env["ASPNETCORE_URLS"] = $"http://localhost:{project.Port.Value}";
        }

        return new LaunchConfiguration
        {
            Name = project.LaunchName,
            Program = programPath,
            PreLaunchTask = project.LaunchName,
            Cwd = $"${{workspaceFolder}}{relativeDir}".Replace("\\", "/"),
            Env = env,
            SourceFileMap = new Dictionary<string, string>
            {
                { "/Views", $"${{workspaceFolder}}{relativeDir}/Views".Replace("\\", "/") }
            }
        };
    }

    /// <summary>
    /// 生成 tasks.json
    /// </summary>
    private void GenerateTasksJson(string vscodeDir, List<ProjectInfo> projects, string basePath)
    {
        var tasks = projects.Select(project => CreateTaskItem(project, basePath)).ToArray();

        var tasksConfig = new TasksConfig
        {
            Tasks = tasks
        };

        string json = JsonSerializer.Serialize(tasksConfig, JsonOptions);
        string filePath = Path.Combine(vscodeDir, "tasks.json");
        File.WriteAllText(filePath, json, Encoding.UTF8);

        Console.WriteLine($"  - tasks.json ({tasks.Length} 个任务)");
    }

    /// <summary>
    /// 创建任务项
    /// </summary>
    private TaskItem CreateTaskItem(ProjectInfo project, string basePath)
    {
        string relativeCsproj = GetRelativePath(basePath, project.CsprojFile);

        return new TaskItem
        {
            Label = project.LaunchName,
            Args = new[]
            {
                "build",
                $"${{workspaceFolder}}{relativeCsproj}".Replace("\\", "/"),
                "/property:GenerateFullPaths=true",
                "/consoleLoggerParameters:NoSummary"
            },
            Group = new TaskGroup
            {
                Kind = "build",
                IsDefault = false
            }
        };
    }

    /// <summary>
    /// 获取相对路径
    /// </summary>
    private string GetRelativePath(string basePath, string fullPath)
    {
        if (fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
        {
            return fullPath.Substring(basePath.Length);
        }
        return fullPath;
    }
}
