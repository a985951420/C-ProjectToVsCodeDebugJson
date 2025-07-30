using System.Text;
using System.IO;
using System.Linq;

/// <summary>
/// 主程序入口，负责用户交互和流程控制
/// </summary>
class Program
{
    private const string ConfigFilePath = "csproj_include_config.txt";

    /// <summary>
    /// 应用程序主入口
    /// </summary>
    static void Main()
    {
        // 设置控制台输入输出编码为 UTF-8
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        string basePath = GetUserInput("请输入要查找 .csproj 文件的目录路径（留空则使用当前目录）：", Directory.GetCurrentDirectory());
        string vscodeSavePath = GetUserInput("请输入 .vscode 目录的保存路径（留空则使用当前目录）：", Directory.GetCurrentDirectory());

        // 查找所有 .csproj 文件
        string[] csprojFiles = ProjectFinder.FindAllCsprojFiles(basePath);
        var availableProjects = csprojFiles.Select(Path.GetFileName).Where(name => name != null).ToList()!;

        var excludeList = GetUserInputList("请输入要排除的 .csproj 文件名，多个文件名用逗号分隔（留空则不排除）：", new List<string>());

        // 读取默认配置
        var defaultIncludeList = ReadDefaultIncludeList();

        // 以勾选方式获取用户选择
        var includeList = GetSelectedProjects(defaultIncludeList);

        // 保存用户输入到配置文件
        SaveIncludeListToConfig(includeList);

        Console.WriteLine("开始生成 VSCode 调试配置...");

        csprojFiles = csprojFiles
            .Where(file => !excludeList.Contains(Path.GetFileName(file)))
            .Where(file => includeList.Count == 0 || includeList.Any(include => Path.GetFileName(file).Contains(include)))
            .ToArray();

        if (csprojFiles.Length == 0)
        {
            Console.WriteLine("错误: 未找到 .csproj 文件");
            return;
        }

        // 解析所有项目文件
        var projectInfos = ParseProjectFiles(csprojFiles);

        // 生成 VSCode 配置文件
        GenerateVscodeConfigs(vscodeSavePath, projectInfos, basePath);

        Console.WriteLine("VSCode 调试配置已成功生成。");
    }

    private static List<ProjectInfo> ParseProjectFiles(string[] csprojFiles)
    {
        return [.. csprojFiles
            .Select(CsprojParser.Parse)
            .Where(info => info != null)
            .Cast<ProjectInfo>()];
    }

    private static void GenerateVscodeConfigs(string vscodeSavePath, List<ProjectInfo> projectInfos, string basePath)
    {
        var vscodeDir = VscodeConfigGenerator.CreateVscodeDirectory(vscodeSavePath);
        VscodeConfigGenerator.GenerateLaunchJson(vscodeDir, projectInfos, basePath);
        VscodeConfigGenerator.GenerateTasksJson(vscodeDir, projectInfos, basePath);
    }

    private static string GetUserInput(string prompt, string defaultValue)
    {
        Console.WriteLine(prompt);
        string input = Console.ReadLine() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }

    private static List<string> GetUserInputList(string prompt, List<string> defaultList)
    {
        Console.WriteLine(prompt);
        string input = Console.ReadLine() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultList : input.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(name => name.Trim()).ToList();
    }

    private static List<string> ReadDefaultIncludeList()
    {
        if (File.Exists(ConfigFilePath))
        {
            string content = File.ReadAllText(ConfigFilePath);
            return content.Split([',', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Select(name => name.Trim()).ToList();
        }
        return new List<string>();
    }

    /// <summary>
    /// 让用户以勾选方式选择项目，并提供手动输入入口
    /// </summary>
    /// <param name="projects">所有可选项目</param>
    /// <returns>用户选择的项目列表</returns>
    private static List<string> GetSelectedProjects(List<string> projects)
    {
        var selectedProjects = new List<string>();

        if (projects.Count > 0)
        {
            Console.WriteLine("配置文件中的可用 .csproj 项目:");
            for (int i = 0; i < projects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {projects[i]}");
            }
            Console.WriteLine("请输入要生成的项目编号，多个编号用逗号分隔（留空则不选择任何项目）：");
            string input = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrEmpty(input))
            {
                var selectedIndices = input.Split([','], StringSplitOptions.RemoveEmptyEntries)
                                        .Select(s =>
                                        {
                                            if (int.TryParse(s.Trim(), out int index) && index > 0 && index <= projects.Count)
                                            {
                                                return index - 1;
                                            }
                                            return -1;
                                        })
                                        .Where(index => index >= 0)
                                        .ToList();

                selectedProjects.AddRange(selectedIndices.Select(index => projects[index]));
            }
        }

        // 添加手动输入新 .csproj 项目的入口
        Console.WriteLine("若要手动添加新的 .csproj 项目，多个项目用逗号分隔（留空则跳过）：");
        string manualInput = Console.ReadLine() ?? string.Empty;
        if (!string.IsNullOrEmpty(manualInput))
        {
            var manualProjects = manualInput.Split([','], StringSplitOptions.RemoveEmptyEntries)
                                        .Select(name => name.Trim())
                                        .ToList();
            selectedProjects.AddRange(manualProjects);
        }

        return selectedProjects.Distinct().ToList();
    }

    /// <summary>
    /// 将包含的项目列表保存到配置文件
    /// </summary>
    /// <param name="includeList">要保存的项目列表</param>
    private static void SaveIncludeListToConfig(List<string> includeList)
    {
        // 读取现有配置
        var existingList = ReadDefaultIncludeList();

        // 合并现有配置和新的包含列表，去除重复项
        var combinedList = existingList.Concat(includeList).Distinct().ToList();

        // 将合并后的列表写入配置文件
        string content = string.Join(",", combinedList);
        File.WriteAllText(ConfigFilePath, content, Encoding.UTF8);
    }
}