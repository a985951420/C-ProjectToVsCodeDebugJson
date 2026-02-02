using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using VsCodeDebugGen.Core.Models;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 端口配置服务实现
/// </summary>
public class PortConfigurationService : IPortConfigurationService
{
    private readonly ILogger<PortConfigurationService> _logger;

    /// <summary>
    /// 初始化端口配置服务
    /// </summary>
    /// <param name="logger">日志记录器</param>
    public PortConfigurationService(ILogger<PortConfigurationService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 自动检测项目的端口配置
    /// </summary>
    /// <param name="projectInfo">项目信息</param>
    /// <returns>端口配置</returns>
    public PortConfiguration AutoDetectPorts(ProjectInfo projectInfo)
    {
        try
        {
            // 尝试从 launchSettings.json 读取
            var launchSettingsPath = Path.Combine(
                Path.GetDirectoryName(projectInfo.CsprojFile) ?? "",
                "Properties",
                "launchSettings.json");

            if (File.Exists(launchSettingsPath))
            {
                var config = ReadFromLaunchSettingsAsync(launchSettingsPath).Result;
                if (config != null)
                {
                    _logger.LogInformation("从 launchSettings.json 检测到端口配置: {Port}", config.HttpPort);
                    return config;
                }
            }

            // 默认端口配置
            var defaultConfig = new PortConfiguration
            {
                HttpPort = 5000,
                HttpsPort = 5001,
                AutoIncrement = true,
                UsedPorts = new List<int>()
            };

            _logger.LogDebug("使用默认端口配置");
            return defaultConfig;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "自动检测端口失败");
            return new PortConfiguration
            {
                HttpPort = 5000,
                HttpsPort = 5001,
                AutoIncrement = true,
                UsedPorts = new List<int>()
            };
        }
    }

    /// <summary>
    /// 检查端口是否可用
    /// </summary>
    /// <param name="port">端口号</param>
    /// <returns>true 表示可用，false 表示已占用</returns>
    public bool IsPortAvailable(int port)
    {
        try
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            return !tcpConnInfoArray.Any(endpoint => endpoint.Port == port);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查端口可用性失败: {Port}", port);
            return true; // 出错时假设可用
        }
    }

    /// <summary>
    /// 获取下一个可用端口
    /// </summary>
    /// <param name="startPort">起始端口号</param>
    /// <returns>可用的端口号</returns>
    public int GetNextAvailablePort(int startPort)
    {
        try
        {
            var port = startPort;
            while (!IsPortAvailable(port) && port < 65535)
            {
                port++;
            }

            if (port >= 65535)
            {
                _logger.LogWarning("未找到可用端口，返回起始端口: {Port}", startPort);
                return startPort;
            }

            return port;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取可用端口失败");
            return startPort;
        }
    }

    /// <summary>
    /// 从 launchSettings.json 读取端口配置
    /// </summary>
    /// <param name="projectPath">项目路径</param>
    /// <returns>端口配置，如果文件不存在则返回 null</returns>
    public async Task<PortConfiguration?> ReadFromLaunchSettingsAsync(string projectPath)
    {
        try
        {
            var launchSettingsPath = projectPath;

            // 如果传入的是项目文件路径，构建完整的 launchSettings.json 路径
            if (projectPath.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                launchSettingsPath = Path.Combine(
                    Path.GetDirectoryName(projectPath) ?? "",
                    "Properties",
                    "launchSettings.json");
            }

            if (!File.Exists(launchSettingsPath))
            {
                _logger.LogDebug("launchSettings.json 不存在: {Path}", launchSettingsPath);
                return null;
            }

            var json = await File.ReadAllTextAsync(launchSettingsPath);
            var doc = JsonDocument.Parse(json);

            var config = new PortConfiguration
            {
                HttpPort = 5000,
                HttpsPort = 5001,
                AutoIncrement = true,
                UsedPorts = new List<int>()
            };

            // 尝试从 profiles 中读取端口信息
            if (doc.RootElement.TryGetProperty("profiles", out var profiles))
            {
                foreach (var profile in profiles.EnumerateObject())
                {
                    if (profile.Value.TryGetProperty("applicationUrl", out var applicationUrl))
                    {
                        var urls = applicationUrl.GetString();
                        if (!string.IsNullOrEmpty(urls))
                        {
                            // 解析 applicationUrl (e.g., "https://localhost:5001;http://localhost:5000")
                            var urlParts = urls.Split(';');
                            foreach (var urlPart in urlParts)
                            {
                                var uri = new Uri(urlPart.Trim());
                                if (uri.Scheme == "http")
                                {
                                    config.HttpPort = uri.Port;
                                }
                                else if (uri.Scheme == "https")
                                {
                                    config.HttpsPort = uri.Port;
                                }
                            }
                            break; // 使用第一个有效的配置
                        }
                    }
                }
            }

            _logger.LogInformation("从 launchSettings.json 读取端口配置: HTTP={HttpPort}, HTTPS={HttpsPort}",
                config.HttpPort, config.HttpsPort);

            return config;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "读取 launchSettings.json 失败: {Path}", projectPath);
            return null;
        }
    }
}
