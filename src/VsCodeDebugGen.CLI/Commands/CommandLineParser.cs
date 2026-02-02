using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.CLI.Commands;

/// <summary>
/// 命令行参数解析器
/// </summary>
public class CommandLineParser
{
    private const string HelpFlag = "--help";
    private const string HelpFlagShort = "-h";
    private const string VersionFlag = "--version";
    private const string VersionFlagShort = "-v";
    private const string InteractiveFlag = "--interactive";
    private const string InteractiveFlagShort = "-i";
    private const string PathFlag = "--path";
    private const string PathFlagShort = "-p";
    private const string OutputFlag = "--output";
    private const string OutputFlagShort = "-o";
    private const string IncludeFlag = "--include";
    private const string ExcludeFlag = "--exclude";
    private const string VerboseFlag = "--verbose";

    /// <summary>
    /// 解析命令行参数
    /// </summary>
    public CommandLineOptions Parse(string[] args)
    {
        var options = new CommandLineOptions();

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            switch (arg.ToLowerInvariant())
            {
                case HelpFlag:
                case HelpFlagShort:
                    options.ShowHelp = true;
                    break;

                case VersionFlag:
                case VersionFlagShort:
                    options.ShowVersion = true;
                    break;

                case InteractiveFlag:
                case InteractiveFlagShort:
                    options.Interactive = true;
                    break;

                case VerboseFlag:
                    options.Verbose = true;
                    break;

                case PathFlag:
                case PathFlagShort:
                    if (i + 1 < args.Length)
                    {
                        options.SearchPath = args[++i];
                    }
                    break;

                case OutputFlag:
                case OutputFlagShort:
                    if (i + 1 < args.Length)
                    {
                        options.OutputPath = args[++i];
                    }
                    break;

                case IncludeFlag:
                    if (i + 1 < args.Length)
                    {
                        options.Include = args[++i];
                    }
                    break;

                case ExcludeFlag:
                    if (i + 1 < args.Length)
                    {
                        options.Exclude = args[++i];
                    }
                    break;

                default:
                    // 未知参数，忽略或提示
                    if (arg.StartsWith("-"))
                    {
                        Console.WriteLine($"警告: 未知参数 '{arg}'，使用 --help 查看帮助");
                    }
                    break;
            }
        }

        // 如果没有任何参数，默认进入交互模式
        if (args.Length == 0)
        {
            options.Interactive = true;
        }

        return options;
    }

    /// <summary>
    /// 显示帮助信息
    /// </summary>
    public void ShowHelp()
    {
        Console.WriteLine(@"
VSCode Debug Generator - C# 项目 VSCode 调试配置生成工具
版本: 2.0.0

用法:
  vscodegen [选项]

选项:
  -h, --help              显示帮助信息
  -v, --version           显示版本信息
  -i, --interactive       交互模式（默认）
  -p, --path <路径>       指定搜索 .csproj 文件的目录路径
  -o, --output <路径>     指定 .vscode 目录的保存路径
  --include <项目>        包含的项目名称（逗号分隔）
  --exclude <项目>        排除的项目名称（逗号分隔）
  --verbose               显示详细输出

示例:
  # 交互模式（默认）
  vscodegen

  # 非交互模式，指定路径
  vscodegen --path ./src --output ./

  # 包含特定项目
  vscodegen --path ./src --include ""MyProject,AnotherProject""

  # 排除特定项目
  vscodegen --exclude ""TestProject,*.Tests""

更多信息: https://github.com/your-repo/vscodegen
");
    }

    /// <summary>
    /// 显示版本信息
    /// </summary>
    public void ShowVersion()
    {
        Console.WriteLine("VSCode Debug Generator v2.0.0");
        Console.WriteLine("Copyright (c) 2025");
    }
}
