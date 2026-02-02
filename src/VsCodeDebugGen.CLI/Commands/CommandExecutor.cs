using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.CLI.Commands;

/// <summary>
/// 命令执行器
/// </summary>
public class CommandExecutor
{
    private readonly IProjectFinder _projectFinder;
    private readonly IProjectParser _projectParser;
    private readonly IConfigGenerator _configGenerator;
    private readonly IConfigurationService _configService;

    public CommandExecutor(
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
    /// 执行非交互模式生成
    /// </summary>
    public void Execute(CommandLineOptions options)
    {
        try
        {
            // 获取路径
            string searchPath = options.SearchPath ?? Directory.GetCurrentDirectory();
            string outputPath = options.OutputPath ?? Directory.GetCurrentDirectory();

            Console.WriteLine($"搜索路径: {searchPath}");
            Console.WriteLine($"输出路径: {outputPath}");
            Console.WriteLine();

            // 查找项目
            Console.WriteLine("正在查找项目文件...");
            var projectFiles = _projectFinder.FindProjects(searchPath).ToList();
            Console.WriteLine($"找到 {projectFiles.Count} 个项目文件");

            if (!projectFiles.Any())
            {
                Console.WriteLine("错误: 未找到任何 .csproj 文件");
                return;
            }

            // 过滤项目
            projectFiles = FilterProjects(projectFiles, options);
            Console.WriteLine($"过滤后剩余 {projectFiles.Count} 个项目");

            if (!projectFiles.Any())
            {
                Console.WriteLine("错误: 所有项目都被过滤，没有需要生成的配置");
                return;
            }

            // 解析项目
            Console.WriteLine();
            Console.WriteLine("正在解析项目...");
            var projects = _projectParser.ParseMany(projectFiles, searchPath).ToList();

            if (!projects.Any())
            {
                Console.WriteLine("错误: 没有成功解析的项目");
                return;
            }

            Console.WriteLine($"成功解析 {projects.Count} 个项目:");
            foreach (var project in projects)
            {
                Console.WriteLine($"  - {project.AssemblyName} ({project.TargetFramework})");
            }

            // 生成配置
            Console.WriteLine();
            Console.WriteLine("正在生成 VSCode 配置...");
            _configGenerator.Generate(projects, outputPath, searchPath);

            Console.WriteLine();
            Console.WriteLine("✓ 完成！配置文件已生成");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"错误: {ex.Message}");
            if (options.Verbose)
            {
                Console.WriteLine($"详细信息: {ex}");
            }
        }
    }

    /// <summary>
    /// 过滤项目列表
    /// </summary>
    private List<string> FilterProjects(List<string> projects, CommandLineOptions options)
    {
        var filtered = projects.AsEnumerable();

        // 应用包含过滤
        if (!string.IsNullOrWhiteSpace(options.Include))
        {
            var includeList = options.Include
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

            filtered = filtered.Where(p =>
                includeList.Any(include =>
                    Path.GetFileName(p).Contains(include, StringComparison.OrdinalIgnoreCase)));
        }

        // 应用排除过滤
        if (!string.IsNullOrWhiteSpace(options.Exclude))
        {
            var excludeList = options.Exclude
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

            filtered = filtered.Where(p =>
                !excludeList.Any(exclude =>
                    Path.GetFileName(p).Contains(exclude, StringComparison.OrdinalIgnoreCase)));
        }

        return filtered.ToList();
    }
}
