using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

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
    // 计算前缀相似度的方法
    public static double PrefixSimilarity(string s1, string s2)
    {
        int minLength = Math.Min(s1.Length, s2.Length);
        int prefixLength = 0;
        for (int i = 0; i < minLength; i++)
        {
            if (s1[i] == s2[i])
            {
                prefixLength++;
            }
            else
            {
                break;
            }
        }
        return (double)prefixLength / Math.Max(s1.Length, s2.Length);
    }

    // 计算后缀相似度的方法
    public static double SuffixSimilarity(string s1, string s2)
    {
        int minLength = Math.Min(s1.Length, s2.Length);
        int suffixLength = 0;
        for (int i = 0; i < minLength; i++)
        {
            if (s1[s1.Length - 1 - i] == s2[s2.Length - 1 - i])
            {
                suffixLength++;
            }
            else
            {
                break;
            }
        }
        return (double)suffixLength / Math.Max(s1.Length, s2.Length);
    }

    // 计算整体相似度的方法，这里使用 Levenshtein 距离计算
    public static double OverallSimilarity(string s1, string s2)
    {
        int distance = LevenshteinDistance(s1, s2);
        int maxLength = Math.Max(s1.Length, s2.Length);
        return 1 - (double)distance / maxLength;
    }

    // 计算 Levenshtein 距离的方法
    private static int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (int i = 0; i <= n; d[i, 0] = i++) { }
        for (int j = 0; j <= m; d[0, j] = j++) { }

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    // 综合相似度计算方法
    public static double CombinedSimilarity(string s1, string s2)
    {
        double prefixSim = PrefixSimilarity(s1, s2);
        double suffixSim = SuffixSimilarity(s1, s2);
        double overallSim = OverallSimilarity(s1, s2);

        // 可以根据实际情况调整权重
        const double prefixWeight = 0.2;
        const double suffixWeight = 0.2;
        const double overallWeight = 0.6;

        return prefixSim * prefixWeight + suffixSim * suffixWeight + overallSim * overallWeight;
    }

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

