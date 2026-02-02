using VsCodeDebugGen.Core.Models;
using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 端口配置服务接口
/// </summary>
public interface IPortConfigurationService
{
    /// <summary>
    /// 自动检测项目的端口配置
    /// </summary>
    /// <param name="projectInfo">项目信息</param>
    /// <returns>端口配置</returns>
    PortConfiguration AutoDetectPorts(ProjectInfo projectInfo);

    /// <summary>
    /// 检查端口是否可用
    /// </summary>
    /// <param name="port">端口号</param>
    /// <returns>true 表示可用，false 表示已占用</returns>
    bool IsPortAvailable(int port);

    /// <summary>
    /// 获取下一个可用端口
    /// </summary>
    /// <param name="startPort">起始端口号</param>
    /// <returns>可用的端口号</returns>
    int GetNextAvailablePort(int startPort);

    /// <summary>
    /// 从 launchSettings.json 读取端口配置
    /// </summary>
    /// <param name="projectPath">项目路径</param>
    /// <returns>端口配置，如果文件不存在则返回 null</returns>
    Task<PortConfiguration?> ReadFromLaunchSettingsAsync(string projectPath);
}
