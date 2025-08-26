using System.Xml.Linq;
using System.IO;

/// <summary>
/// 解析 .csproj 文件工具类，用于提取项目的关键信息
/// </summary>
static class CsprojParser
{
    /// <summary>
    /// 解析指定 .csproj 文件，提取项目信息
    /// </summary>
    /// <param name="csprojFile">.csproj 文件路径</param>
    /// <param name="basePath">基础目录路径，用于查找 Directory.Build.props 文件</param>
    /// <returns>解析后的项目信息对象，若解析失败则返回 null</returns>
    public static ProjectInfo? Parse(string csprojFile, string basePath)
    {
        try
        {
            // 加载 .csproj 文件的 XML 文档
            var doc = LoadXmlDocument(csprojFile);
            if (doc == null)
                return null;

            // 解析 .csproj 文件中的属性信息，传递 csprojFile 参数
            var (outputType, targetFramework, assemblyName, outputPath) = ParseCsprojProperties(doc, csprojFile);

            // 获取目标框架版本，若 .csproj 文件中未找到，则尝试从 Directory.Build.props 文件中查找
            string finalFramework = GetTargetFramework(targetFramework, basePath);
            if (string.IsNullOrEmpty(finalFramework))
            {
                Console.WriteLine("未找到目标框架版本，且无法从 Directory.Build.props 文件获取。");
                return null;
            }

            // 设置输出路径，若 .csproj 文件中未指定，则使用默认路径
            string finalOutputPath = string.IsNullOrEmpty(outputPath) 
                ? Path.Combine("bin", "Debug", finalFramework)
                : outputPath;

            // 返回解析后的项目信息对象
            return new ProjectInfo
            {
                CsprojFile = csprojFile,
                OutputType = outputType,
                TargetFramework = finalFramework,
                AssemblyName = assemblyName,
                OutputPath = finalOutputPath
            };
        }
        catch
        {
            // 发生异常时返回 null
            return null;
        }
    }

    /// <summary>
    /// 加载指定路径的 XML 文件并返回 XDocument 对象
    /// </summary>
    /// <param name="filePath">XML 文件路径</param>
    /// <returns>加载成功返回 XDocument 对象，失败返回 null</returns>
    private static XDocument? LoadXmlDocument(string filePath)
    {
        try
        {
            return XDocument.Load(filePath);
        }
        catch
        {
            // 加载文件失败时返回 null
            return null;
        }
    }

    /// <summary>
    /// 解析 .csproj 文件中的属性信息
    /// </summary>
    /// <param name="doc">.csproj 文件的 XDocument 对象</param>
    /// <param name="csprojFile">.csproj 文件路径</param>
    /// <returns>包含输出类型、目标框架、程序集名称和输出路径的元组</returns>
    private static (string outputType, string targetFramework, string assemblyName, string outputPath) ParseCsprojProperties(XDocument doc, string csprojFile)
    {
        // 初始化默认值
        string outputType = "Exe";
        string targetFramework = "";
        // 使用 csprojFile 的文件名作为默认程序集名称
        string assemblyName = Path.GetFileNameWithoutExtension(csprojFile);
        string outputPath = "";

        // 获取所有 PropertyGroup 元素
        var propertyGroups = doc.Descendants("PropertyGroup");
        foreach (var propertyGroup in propertyGroups)
        {
            // 解析输出类型，若未找到则使用默认值
            outputType = propertyGroup.Element("OutputType")?.Value ?? outputType;
            // 解析目标框架版本
            targetFramework = GetFirstValidFramework(propertyGroup, targetFramework);
            // 解析程序集名称，若未找到则使用默认值
            assemblyName = propertyGroup.Element("AssemblyName")?.Value ?? assemblyName;
            // 解析输出路径，若未找到则使用默认值
            outputPath = propertyGroup.Element("OutputPath")?.Value ?? outputPath;
        }

        return (outputType, targetFramework, assemblyName, outputPath);
    }

    /// <summary>
    /// 从 PropertyGroup 元素中获取第一个有效的目标框架版本
    /// </summary>
    /// <param name="propertyGroup">PropertyGroup 元素</param>
    /// <param name="currentFramework">当前的目标框架版本</param>
    /// <returns>第一个有效的目标框架版本，若未找到则返回当前版本</returns>
    private static string GetFirstValidFramework(XElement propertyGroup, string currentFramework)
    {
        // 尝试获取 TargetFramework 元素的值
        string framework = propertyGroup.Element("TargetFramework")?.Value ?? string.Empty;
        if (!string.IsNullOrEmpty(framework))
            return framework;

        // 若 TargetFramework 未找到，尝试获取 TargetFrameworks 元素的值
        string frameworks = propertyGroup.Element("TargetFrameworks")?.Value ?? string.Empty;
        return !string.IsNullOrEmpty(frameworks) ? frameworks.Split(';')[0] : currentFramework;
    }

    /// <summary>
    /// 获取目标框架版本，若初始版本为空，则尝试从 Directory.Build.props 文件中查找
    /// </summary>
    /// <param name="initialFramework">初始的目标框架版本</param>
    /// <param name="basePath">基础目录路径，用于查找 Directory.Build.props 文件</param>
    /// <returns>有效的目标框架版本，若未找到则返回空字符串</returns>
    private static string GetTargetFramework(string initialFramework, string basePath)
    {
        // 若初始版本不为空，直接返回
        if (!string.IsNullOrEmpty(initialFramework))
            return initialFramework;

        // 构建 Directory.Build.props 文件路径
        string propsPath = Path.Combine(basePath, "Directory.Build.props");
        if (File.Exists(propsPath))
        {
            // 加载 Directory.Build.props 文件的 XML 文档
            var propsDoc = LoadXmlDocument(propsPath);
            if (propsDoc != null)
            {
                // 获取所有 PropertyGroup 元素
                var propertyGroups = propsDoc.Descendants("PropertyGroup");
                foreach (var group in propertyGroups)
                {
                    // 尝试获取目标框架版本
                    string framework = GetFirstValidFramework(group, string.Empty);
                    if (!string.IsNullOrEmpty(framework))
                        return framework;
                }
            }
            else
            {
                // 加载 Directory.Build.props 文件失败时输出错误信息
                Console.WriteLine("读取 Directory.Build.props 文件时出错。");
            }
        }

        return string.Empty;
    }
}