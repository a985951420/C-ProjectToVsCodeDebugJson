using System.Text;

/// <summary>
/// 主程序入口，负责用户交互和流程控制
/// </summary>
class Program
{
    /// <summary>
    /// 应用程序主入口
    /// </summary>
    static void Main()
    {
        // 设置控制台输入输出编码为 UTF-8
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        string basePath = GetUserInput("请输入要查找 .csproj 文件的目录路径（留空则使用当前目录）：", Directory.GetCurrentDirectory());
        string vscodeSavePath = GetUserInput("请输入 .vscode 目录的保存路径（留空则使用当前目录）：", Directory.GetCurrentDirectory());
        var excludeList = GetUserInputList("请输入要排除的 .csproj 文件名，多个文件名用逗号分隔（留空则不排除）：");
        var includeList = GetUserInputList("请输入要生成的 .csproj 文件名，多个文件名用逗号分隔（留空则生成所有文件）：");

        Console.WriteLine("开始生成 VSCode 调试配置...");

        // 查找所有 .csproj 文件
        string[] csprojFiles = ProjectFinder.FindAllCsprojFiles(basePath);
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
        return csprojFiles
            .Select(csprojFile => CsprojParser.Parse(csprojFile))
            .Where(info => info != null)
            .ToList();
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
        string input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }

    private static List<string> GetUserInputList(string prompt)
    {
        Console.WriteLine(prompt);
        string input = Console.ReadLine();
        return string.IsNullOrEmpty(input) ? new List<string>() : input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(name => name.Trim()).ToList();
    }
}