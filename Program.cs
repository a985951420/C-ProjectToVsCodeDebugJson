using System;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

// 定义 JSON 序列化上下文
[JsonSerializable(typeof(LaunchConfig))]
[JsonSerializable(typeof(TasksConfig))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}

// 定义 launch.json 对应的类
internal class LaunchConfig
{
    public string version { get; set; } = "0.2.0";
    public LaunchConfiguration[] configurations { get; set; } = Array.Empty<LaunchConfiguration>();
}

internal class LaunchConfiguration
{
    public string name { get; set; } = ".NET Core Launch (web)";
    public string type { get; set; } = "coreclr";
    public string request { get; set; } = "launch";
    public string preLaunchTask { get; set; } = "build";
    public string program { get; set; } = "";
    public string[] args { get; set; } = Array.Empty<string>();
    public string cwd { get; set; } = "${workspaceFolder}";
    public LaunchBrowser launchBrowser { get; set; } = new LaunchBrowser();
    public Dictionary<string, string> env { get; set; } = new Dictionary<string, string>
    {
        { "ASPNETCORE_ENVIRONMENT", "Development" }
    };
    public Dictionary<string, string> sourceFileMap { get; set; } = new Dictionary<string, string>
    {
        { "/Views", "${workspaceFolder}/Views" }
    };
}

internal class LaunchBrowser
{
    public bool enabled { get; set; } = true;
    public string args { get; set; } = "${auto-detect-url}";
    public WindowsCommand windows { get; set; } = new WindowsCommand();
    public OsxCommand osx { get; set; } = new OsxCommand();
    public LinuxCommand linux { get; set; } = new LinuxCommand();
}

internal class WindowsCommand
{
    public string command { get; set; } = "cmd.exe";
    public string args { get; set; } = "/C start ${auto-detect-url}";
}

internal class OsxCommand
{
    public string command { get; set; } = "open";
}

internal class LinuxCommand
{
    public string command { get; set; } = "xdg-open";
}

// 定义 tasks.json 对应的类
internal class TasksConfig
{
    public string version { get; set; } = "2.0.0";
    public TaskItem[] tasks { get; set; } = Array.Empty<TaskItem>();
}

internal class TaskItem
{
    public string label { get; set; } = "build";
    public string command { get; set; } = "dotnet";
    public string type { get; set; } = "process";
    public string[] args { get; set; } = Array.Empty<string>();
    public string problemMatcher { get; set; } = "${msCompile}";
}

class Program
{
    static void Main()
    {
        // 设置控制台输入输出编码为 UTF-8
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("请输入要查找 .csproj 文件的目录路径（留空则使用当前目录）：");
        string basePath = Console.ReadLine();
        if (string.IsNullOrEmpty(basePath))
        {
            basePath = Directory.GetCurrentDirectory();
        }

        Console.WriteLine("请输入 .vscode 目录的保存路径（留空则使用当前目录）：");
        string vscodeSavePath = Console.ReadLine();
        if (string.IsNullOrEmpty(vscodeSavePath))
        {
            vscodeSavePath = Directory.GetCurrentDirectory();
        }

        Console.WriteLine("开始生成 VSCode 调试配置...");

        // 查找所有 .csproj 文件
        string[] csprojFiles = FindAllCsprojFiles(basePath);
        if (csprojFiles.Length == 0)
        {
            Console.WriteLine("错误: 未找到 .csproj 文件");
            return;
        }

        // 创建 .vscode 目录
        string vscodeDir = CreateVscodeDirectory(vscodeSavePath);

        // 收集所有 launch 配置
        var projectInfoList = new System.Collections.Generic.List<(string csprojFile, string launchName)>();
        var allLaunchConfigurations = new System.Collections.Generic.List<LaunchConfiguration>();

        foreach (string csprojFile in csprojFiles)
        {
            // 解析 .csproj 文件，新增 outputPath 参数
            if (TryParseCsprojFile(csprojFile, out string outputType, out string targetFramework, out string assemblyName, out string outputPath))
            {
                string launchName = $"Launch {assemblyName}";

                // 获取项目目录
                string projectDir = Path.GetDirectoryName(csprojFile)!;

                var relativeProjectDir = projectDir.Replace(basePath, string.Empty);

                // 拼接正确的 DLL 路径，统一使用正斜杠
                string programPath = string.Concat("${workspaceFolder}", relativeProjectDir, "\\", outputPath, "\\", $"{assemblyName}.dll").Replace("\\", "/");

                var launchConfig = new LaunchConfiguration
                {
                    name = launchName,
                    program = programPath,
                    preLaunchTask = launchName,
                    cwd = string.Concat("${workspaceFolder}", relativeProjectDir).Replace("\\", "/"),
                    sourceFileMap = new Dictionary<string, string>
                    {
                        { "/Views", Path.Combine("${workspaceFolder}", relativeProjectDir, "Views").Replace("\\", "/") }
                    }
                };
                allLaunchConfigurations.Add(launchConfig);

                projectInfoList.Add((csprojFile, launchName));
            }
        }

        // 生成 launch.json
        GenerateLaunchJson(vscodeDir, allLaunchConfigurations);
        // 生成 tasks.json，传递项目信息列表
        GenerateTasksJson(vscodeDir, projectInfoList, basePath);

        // 打印生成目录
        Console.WriteLine($"VSCode 调试配置已成功生成到目录: {vscodeDir}");
    }

    /// <summary>
    /// 获取目标路径相对于基准路径的相对路径
    /// </summary>
    /// <param name="basePath">基准路径</param>
    /// <param name="targetPath">目标路径</param>
    /// <returns>相对路径</returns>
    private static string GetRelativePath(string basePath, string targetPath)
    {
        // 获取基准路径的绝对路径，并确保路径以目录分隔符结尾
        string fullBasePath = Path.GetFullPath(basePath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        // 获取目标路径的绝对路径

        var fullTargetPath = string.Concat(fullBasePath, targetPath);
        // string fullTargetPath = Path.GetFullPath(targetPath);

        // 使用 UriKind.Absolute 明确指定为绝对路径
        Uri baseUri = new Uri(fullBasePath, UriKind.Absolute);
        Uri targetUri = new Uri(fullTargetPath, UriKind.Absolute);

        Uri relativeUri = baseUri.MakeRelativeUri(targetUri);
        string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// 递归查找指定目录及其子目录下的所有 .csproj 文件
    /// </summary>
    /// <param name="searchPath">要查找的目录路径</param>
    /// <returns>.csproj 文件路径数组</returns>
    private static string[] FindAllCsprojFiles(string searchPath)
    {
        // 使用 SearchOption.AllDirectories 递归搜索所有子目录
        return Directory.GetFiles(searchPath, "*.csproj", SearchOption.AllDirectories);
    }

    /// <summary>
    /// 尝试解析 .csproj 文件，获取项目信息
    /// </summary>
    /// <param name="csprojFile">.csproj 文件路径</param>
    /// <param name="outputType">输出类型</param>
    /// <param name="targetFramework">目标框架</param>
    /// <param name="assemblyName">程序集名称</param>
    /// <param name="outputPath">输出路径</param>
    /// <returns>解析成功返回 true，否则返回 false</returns>
    private static bool TryParseCsprojFile(string csprojFile, out string outputType, out string targetFramework, out string assemblyName, out string outputPath)
    {
        outputType = "Exe";
        targetFramework = "";
        assemblyName = Path.GetFileNameWithoutExtension(csprojFile);
        outputPath = "";
    
        try
        {
            XDocument doc = XDocument.Load(csprojFile);
            var propertyGroups = doc.Descendants("PropertyGroup");
    
            foreach (var propertyGroup in propertyGroups)
            {
                if (string.IsNullOrEmpty(outputType))
                {
                    outputType = propertyGroup.Element("OutputType")?.Value ?? outputType;
                }
                if (string.IsNullOrEmpty(targetFramework))
                {
                    targetFramework = propertyGroup.Element("TargetFramework")?.Value ??
                                     propertyGroup.Element("TargetFrameworks")?.Value?.Split(';')[0] ??
                                     targetFramework;
                }
                if (string.IsNullOrEmpty(assemblyName))
                {
                    assemblyName = propertyGroup.Element("AssemblyName")?.Value ?? assemblyName;
                }
                if (string.IsNullOrEmpty(outputPath))
                {
                    outputPath = propertyGroup.Element("OutputPath")?.Value ?? outputPath;
                }
            }
    
            if (string.IsNullOrEmpty(targetFramework))
            {
                Console.WriteLine($"错误: 无法从 {csprojFile} 中获取目标框架信息");
                return false;
            }
    
            if (string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.Combine("bin", "Debug", targetFramework);
            }
            else if (!Path.IsPathRooted(outputPath))
            {
                // 以 .csproj 文件所在目录为基准获取完整路径
                outputPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(csprojFile)!, outputPath));
            }
    
            // 计算相对于项目目录的相对路径
            string projectDir = Path.GetDirectoryName(csprojFile)!;
            outputPath = GetRelativePath(projectDir, outputPath);
    
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: 解析项目文件 {csprojFile} 时出错 - {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 在指定目录创建 .vscode 目录
    /// </summary>
    /// <param name="savePath">保存路径</param>
    /// <returns>.vscode 目录的路径</returns>
    private static string CreateVscodeDirectory(string savePath)
    {
        string vscodeDir = Path.Combine(savePath, ".vscode");
        Directory.CreateDirectory(vscodeDir);
        return vscodeDir;
    }

    static void GenerateLaunchJson(string vscodeDir, System.Collections.Generic.List<LaunchConfiguration> configurations)
    {
        var launchConfig = new LaunchConfig
        {
            configurations = configurations.ToArray()
        };

        // 设置 JSON 序列化选项，启用缩进
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 避免特殊字符转义问题
        };

        // 使用只需要 JsonSerializerOptions 的重载方法
        string json = JsonSerializer.Serialize(launchConfig, options);
        string launchFilePath = Path.Combine(vscodeDir, "launch.json");
        File.WriteAllText(launchFilePath, json, Encoding.UTF8);
    }

    // 移除重复的 GenerateLaunchJson 方法
    // static void GenerateLaunchJson(string vscodeDir, string assemblyName, string outputPath)
    // {
    //     ...
    // }

    static void GenerateTasksJson(string vscodeDir, System.Collections.Generic.List<(string csprojFile, string launchName)> projectInfo,string basePath = "")
    {
        var tasks = new System.Collections.Generic.List<TaskItem>();

        foreach (var info in projectInfo)
        {
            // 获取相对于工作区的 .csproj 文件路径
            // string relativeCsprojPath = GetRelativePath(basePath, info.csprojFile);

            var asdf = info.csprojFile.Replace(basePath, string.Empty);

            tasks.Add(new TaskItem
            {
                label = info.launchName,
                args = new[]
                {
                    "build",
                    string.Concat("${workspaceFolder}", asdf).Replace("\\", "/"),
                    "/property:GenerateFullPaths=true",
                    "/consoleloggerparameters:NoSummary"
                }
            });
        }

        var tasksConfig = new TasksConfig
        {
            tasks = tasks.ToArray()
        };

        // 设置 JSON 序列化选项，启用缩进
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // 避免特殊字符转义问题
        };

        string json = JsonSerializer.Serialize(tasksConfig, options);
        string tasksFilePath = Path.Combine(vscodeDir, "tasks.json");
        File.WriteAllText(tasksFilePath, json, Encoding.UTF8);
    }
}