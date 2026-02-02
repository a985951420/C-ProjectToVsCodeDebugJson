using VsCodeDebugGen.Core.Models;

namespace VsCodeDebugGen.Core.Interfaces;

/// <summary>
/// VSCode 配置生成器接口
/// </summary>
public interface IConfigGenerator
{
    /// <summary>
    /// 生成 VSCode 调试配置文件
    /// </summary>
    /// <param name="projects">项目信息列表</param>
    /// <param name="outputPath">输出路径</param>
    /// <param name="basePath">基础路径</param>
    void Generate(IEnumerable<ProjectInfo> projects, string outputPath, string basePath);
}
