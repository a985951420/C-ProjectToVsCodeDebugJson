using System.Text;
using VsCodeDebugGen.Core.Interfaces;

namespace VsCodeDebugGen.CLI.UI;

/// <summary>
/// 交互式用户界面
/// </summary>
public class InteractiveUI
{
    private readonly IProjectFinder _projectFinder;
    private readonly IProjectParser _projectParser;
    private readonly IConfigGenerator _configGenerator;
    private readonly IConfigurationService _configService;

    public InteractiveUI(
        IProjectFinder projectFinder,
        IProjectParser projectParser,
        IConfigGenerator configGenerator,
        IConfigurationService configService)
    {
        _projectFinder = projectFinder;
        _projectParser = projectParser;
        _configGenerator = configGenerator;
        _configService = configService;
    }

    /// <summary>
    /// 启动交互式流程
    /// </summary>
    public void Run()
    {
        // 设置控制台编码
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        ShowWelcome();

        try
        {
            // 1. 获取搜索路径
            string searchPath = GetInput(
                "请输入要查找 .csproj 文件的目录路径",
                Directory.GetCurrentDirectory(),
                "留空则使用当前目录");

            // 2. 获取输出路径
            string outputPath = GetInput(
                "请输入 .vscode 目录的保存路径",
                Directory.GetCurrentDirectory(),
                "留空则使用当前目录");

            Console.WriteLine();
            Console.WriteLine("正在查找项目文件...");

            // 3. 查找项目
            var projectFiles = _projectFinder.FindProjects(searchPath).ToList();
            Console.WriteLine($"✓ 找到 {projectFiles.Count} 个项目文件");

            if (!projectFiles.Any())
            {
                Console.WriteLine("错误: 未找到任何 .csproj 文件");
                return;
            }

            // 4. 选择项目
            var selectedProjects = SelectProjects(projectFiles);

            if (!selectedProjects.Any())
            {
                Console.WriteLine("未选择任何项目，退出程序");
                return;
            }

            // 5. 解析项目
            Console.WriteLine();
            Console.WriteLine("正在解析项目...");
            var projects = _projectParser.ParseMany(selectedProjects, searchPath).ToList();

            if (!projects.Any())
            {
                Console.WriteLine("错误: 没有成功解析的项目");
                return;
            }

            Console.WriteLine($"✓ 成功解析 {projects.Count} 个项目");

            // 6. 生成配置
            Console.WriteLine();
            Console.WriteLine("正在生成 VSCode 配置...");
            _configGenerator.Generate(projects, outputPath, searchPath);

            Console.WriteLine();
            Console.WriteLine("🎉 完成！VSCode 调试配置已成功生成");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine($"❌ 错误: {ex.Message}");
        }
    }

    /// <summary>
    /// 显示欢迎信息
    /// </summary>
    private void ShowWelcome()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║   VSCode Debug Generator v2.0                         ║");
        Console.WriteLine("║   C# 项目 VSCode 调试配置生成工具                      ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }

    /// <summary>
    /// 获取用户输入
    /// </summary>
    private string GetInput(string prompt, string defaultValue, string hint = "")
    {
        Console.WriteLine($"{prompt}");
        if (!string.IsNullOrEmpty(hint))
        {
            Console.WriteLine($"  ({hint})");
        }
        Console.Write("> ");

        string input = Console.ReadLine()?.Trim() ?? string.Empty;
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }

    /// <summary>
    /// 选择项目
    /// </summary>
    private List<string> SelectProjects(List<string> allProjects)
    {
        Console.WriteLine();
        Console.WriteLine("找到的项目:");
        Console.WriteLine();

        // 显示项目列表
        for (int i = 0; i < allProjects.Count; i++)
        {
            string projectName = Path.GetFileNameWithoutExtension(allProjects[i]);
            string projectDir = Path.GetDirectoryName(allProjects[i]) ?? "";
            Console.WriteLine($"  [{i + 1}] {projectName}");
            Console.WriteLine($"      {projectDir}");
        }

        Console.WriteLine();
        Console.WriteLine("请选择要生成配置的项目:");
        Console.WriteLine("  - 输入项目编号（多个用逗号分隔，如: 1,3,5）");
        Console.WriteLine("  - 输入 'all' 选择所有项目");
        Console.WriteLine("  - 直接回车使用配置文件中的记录");
        Console.Write("> ");

        string input = Console.ReadLine()?.Trim() ?? string.Empty;

        // 使用配置文件
        if (string.IsNullOrEmpty(input))
        {
            var savedProjects = _configService.GetIncludeList();
            if (savedProjects.Any())
            {
                var matched = allProjects
                    .Where(p => savedProjects.Any(saved =>
                        Path.GetFileName(p).Contains(saved, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                if (matched.Any())
                {
                    Console.WriteLine($"✓ 使用配置文件，匹配到 {matched.Count} 个项目");
                    return matched;
                }
            }

            Console.WriteLine("配置文件为空或没有匹配项，将选择所有项目");
            return allProjects;
        }

        // 选择所有
        if (input.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("✓ 已选择所有项目");
            return allProjects;
        }

        // 按编号选择
        var selected = new List<string>();
        var indices = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var indexStr in indices)
        {
            if (int.TryParse(indexStr.Trim(), out int index) && index > 0 && index <= allProjects.Count)
            {
                selected.Add(allProjects[index - 1]);
            }
        }

        if (selected.Any())
        {
            Console.WriteLine($"✓ 已选择 {selected.Count} 个项目");

            // 保存到配置
            var projectNames = selected
                .Select(Path.GetFileName)
                .Where(n => !string.IsNullOrEmpty(n))
                .Cast<string>()
                .ToList();
            _configService.SaveIncludeList(projectNames);
        }

        return selected;
    }
}
