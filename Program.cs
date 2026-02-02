using VsCodeDebugGen.CLI.Commands;
using VsCodeDebugGen.CLI.UI;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Services;
using Infrastructure.Configuration;

namespace VsCodeDebugGen;

/// <summary>
/// 应用程序主入口
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        // 初始化服务
        IProjectFinder projectFinder = new ProjectFinderService();
        IProjectParser projectParser = new ProjectParserService();
        IConfigGenerator configGenerator = new ConfigGeneratorService();
        IConfigurationService configService = new ConfigurationService();

        // 解析命令行参数
        var parser = new CommandLineParser();
        var options = parser.Parse(args);

        // 处理特殊命令
        if (options.ShowHelp)
        {
            parser.ShowHelp();
            return;
        }

        if (options.ShowVersion)
        {
            parser.ShowVersion();
            return;
        }

        try
        {
            // 交互模式
            if (options.Interactive)
            {
                var ui = new InteractiveUI(projectFinder, projectParser, configGenerator, configService);
                ui.Run();
            }
            // 非交互模式
            else
            {
                var executor = new CommandExecutor(projectFinder, projectParser, configGenerator, configService);
                executor.Execute(options);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ 发生未处理的错误: {ex.Message}");
            if (options.Verbose)
            {
                Console.WriteLine($"\n堆栈跟踪:\n{ex.StackTrace}");
            }
            Environment.Exit(1);
        }
    }
}
