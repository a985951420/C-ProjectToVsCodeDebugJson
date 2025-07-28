using System.Text.Json;
using System.Text;

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
        var configurations = projects.Select(p =>
        {
            string projectDir = Path.GetDirectoryName(p.CsprojFile)!;
            string relativeDir = projectDir.Replace(basePath, string.Empty).Replace("\\", "/");
            string programPath = $"{ "${workspaceFolder}" }{relativeDir}/{p.OutputPath}/{p.AssemblyName}.dll".Replace("\\", "/");

            return new LaunchConfiguration
            {
                name = p.LaunchName,
                program = programPath,
                preLaunchTask = p.LaunchName,
                cwd = $"{ "${workspaceFolder}" }{relativeDir}".Replace("\\", "/"),
                sourceFileMap = new Dictionary<string, string>
                {
                    { "/Views", Path.Combine("${workspaceFolder}", relativeDir, "Views").Replace("\\", "/") }
                }
            };
        }).ToArray();

        var launchConfig = new LaunchConfig { configurations = configurations };
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        string json = JsonSerializer.Serialize(launchConfig, options);
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
                    "/consoleloggerparameters:NoSummary"
                }
            };
        }).ToArray();

        var tasksConfig = new TasksConfig { tasks = tasks };
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        string json = JsonSerializer.Serialize(tasksConfig, options);
        File.WriteAllText(Path.Combine(vscodeDir, "tasks.json"), json, Encoding.UTF8);
    }
}