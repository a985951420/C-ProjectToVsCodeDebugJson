using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

// 定义 JSON 序列化上下文，标记需要序列化的类型
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(LaunchConfiguration))]
[JsonSerializable(typeof(LaunchConfig))]
[JsonSerializable(typeof(TaskItem))]
[JsonSerializable(typeof(TasksConfig))]
[JsonSerializable(typeof(ProjectInfo))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
    // 源代码生成器会自动填充内容
}

/// <summary>
/// VSCode 配置生成工具类
/// </summary>
static class VscodeConfigGenerator
{
    /// <summary>
    /// 创建 .vscode 目录（如不存在则新建）
    /// </summary>
    /// <param name="savePath">保存路径</param>
    /// <returns>.vscode 目录完整路径</returns>
    public static string CreateVscodeDirectory(string savePath)
    {
        string vscodeDir = Path.Combine(savePath, ".vscode");
        Directory.CreateDirectory(vscodeDir);
        return vscodeDir;
    }

    /// <summary>
    /// 生成 launch.json 调试配置文件
    /// </summary>
    /// <param name="vscodeDir">.vscode 目录路径</param>
    /// <param name="projects">所有项目的信息</param>
    /// <param name="basePath">工作区根目录</param>
    public static void GenerateLaunchJson(string vscodeDir, List<ProjectInfo> projects, string basePath)
    {
        var configurations = new List<LaunchConfiguration>();

        foreach (var project in projects)
        {
            string projectDir = Path.GetDirectoryName(project.CsprojFile)!;
            string relativeDir = projectDir.Replace(basePath, string.Empty).Replace("\\", "/");
            string programPath = $"{"${workspaceFolder}"}{relativeDir}/{project.OutputPath}/{project.AssemblyName}.dll".Replace("\\", "/");

            var launchConfiguration = new LaunchConfiguration
            {
                name = project.LaunchName,
                program = programPath,
                preLaunchTask = project.LaunchName,
                cwd = $"{"${workspaceFolder}"}{relativeDir}".Replace("\\", "/"),
                sourceFileMap = new Dictionary<string, string>
                {
                    { "/Views", Path.Combine("${workspaceFolder}", relativeDir, "Views").Replace("\\", "/") }
                }
            };

            configurations.Add(launchConfiguration);
        }

        var launchConfig = new LaunchConfig { configurations = configurations.ToArray() };
        // 直接传入 JsonSerializerContext 实例
        string json = JsonSerializer.Serialize(launchConfig, typeof(LaunchConfig), AppJsonSerializerContext.Default);
        File.WriteAllText(Path.Combine(vscodeDir, "launch.json"), json, Encoding.UTF8);
    }

    /// <summary>
    /// 生成 tasks.json 构建任务配置文件
    /// </summary>
    /// <param name="vscodeDir">.vscode 目录路径</param>
    /// <param name="projects">所有项目的信息</param>
    /// <param name="basePath">工作区根目录</param>
    public static void GenerateTasksJson(string vscodeDir, List<ProjectInfo> projects, string basePath)
    {
        var tasks = projects.Select(p =>
        {
            var relativeCsproj = p.CsprojFile.Replace(basePath, string.Empty).Replace("\\", "/");
            return new TaskItem
            {
                label = p.LaunchName,
                args = new[]
                {
                    "build",
                    $"{ "${workspaceFolder}" }{relativeCsproj}",
                    "/property:GenerateFullPaths=true",
                    // 修正拼写错误
                    "/consoleLoggerParameters:NoSummary"
                }
            };
        }).ToArray();

        var tasksConfig = new TasksConfig { tasks = tasks };
        // 直接传入 JsonSerializerContext 实例
        string json = JsonSerializer.Serialize(tasksConfig, typeof(TasksConfig), AppJsonSerializerContext.Default);
        File.WriteAllText(Path.Combine(vscodeDir, "tasks.json"), json, Encoding.UTF8);
    }
}

