using System.Xml.Linq;
using System.IO;

/// <summary>
/// 解析 .csproj 文件工具类
/// </summary>
static class CsprojParser
{
    /// <summary>
    /// 解析指定 .csproj 文件，提取项目信息
    /// </summary>
    /// <param name="csprojFile">.csproj 文件路径</param>
    /// <param name="basePath">基础目录路径</param>
    /// <returns>解析后的项目信息对象，失败返回 null</returns>
    public static ProjectInfo? Parse(string csprojFile, string basePath)
    {
        try
        {
            XDocument doc = XDocument.Load(csprojFile);
            var propertyGroups = doc.Descendants("PropertyGroup");

            string outputType = "Exe";
            string targetFramework = "";
            string assemblyName = Path.GetFileNameWithoutExtension(csprojFile);
            string outputPath = "";

            foreach (var propertyGroup in propertyGroups)
            {
                outputType = propertyGroup.Element("OutputType")?.Value ?? outputType;
                targetFramework = propertyGroup.Element("TargetFramework")?.Value ??
                                  propertyGroup.Element("TargetFrameworks")?.Value?.Split(';')[0] ?? targetFramework;
                assemblyName = propertyGroup.Element("AssemblyName")?.Value ?? assemblyName;
                outputPath = propertyGroup.Element("OutputPath")?.Value ?? outputPath;
            }

            if (string.IsNullOrEmpty(targetFramework))
            {
                string propsPath = Path.Combine(basePath, "Directory.Build.props");
                if (File.Exists(propsPath))
                {
                    try
                    {
                        XDocument propsDoc = XDocument.Load(propsPath);
                        var propsPropertyGroups = propsDoc.Descendants("PropertyGroup");
                        foreach (var propsPropertyGroup in propsPropertyGroups)
                        {
                            targetFramework = propsPropertyGroup.Element("TargetFramework")?.Value ??
                                              propsPropertyGroup.Element("TargetFrameworks")?.Value?.Split(';')[0] ?? targetFramework;
                            if (!string.IsNullOrEmpty(targetFramework))
                            {
                                break;
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("读取 Directory.Build.props 文件时出错。");
                    }
                }
            }

            if (string.IsNullOrEmpty(targetFramework))
            {
                Console.WriteLine("未找到目标框架版本，且无法从 Directory.Build.props 文件获取。");
                return null;
            }

            if (string.IsNullOrEmpty(outputPath))
                outputPath = Path.Combine("bin", "Debug", targetFramework);

            return new ProjectInfo
            {
                CsprojFile = csprojFile,
                OutputType = outputType,
                TargetFramework = targetFramework,
                AssemblyName = assemblyName,
                OutputPath = outputPath
            };
        }
        catch
        {
            return null;
        }
    }
}