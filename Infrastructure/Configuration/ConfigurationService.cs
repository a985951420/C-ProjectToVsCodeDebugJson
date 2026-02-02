using System.Text;
using System.Text.Json;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Core.Models;

namespace Infrastructure.Configuration;

/// <summary>
/// 配置服务实现
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private const string ConfigFileName = "vscodegen.config.json";
    private const string LegacyConfigFileName = "csproj_include_config.txt";
    private readonly string _configFilePath;
    private readonly string _legacyConfigPath;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public ConfigurationService()
    {
        _configFilePath = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
        _legacyConfigPath = Path.Combine(Directory.GetCurrentDirectory(), LegacyConfigFileName);
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    public AppConfiguration Load()
    {
        // 尝试加载新格式配置
        if (File.Exists(_configFilePath))
        {
            try
            {
                string json = File.ReadAllText(_configFilePath, Encoding.UTF8);
                var config = JsonSerializer.Deserialize<AppConfiguration>(json, JsonOptions);
                return config ?? new AppConfiguration();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"警告: 读取配置文件失败 - {ex.Message}");
            }
        }

        // 尝试从旧版本配置迁移
        var legacyConfig = LoadLegacyConfig();
        if (legacyConfig.IncludeProjects.Any())
        {
            Save(legacyConfig);
            return legacyConfig;
        }

        return new AppConfiguration();
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    public void Save(AppConfiguration config)
    {
        try
        {
            string json = JsonSerializer.Serialize(config, JsonOptions);
            File.WriteAllText(_configFilePath, json, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"警告: 保存配置文件失败 - {ex.Message}");
        }
    }

    /// <summary>
    /// 获取包含项目列表
    /// </summary>
    public List<string> GetIncludeList()
    {
        var config = Load();
        return config.IncludeProjects;
    }

    /// <summary>
    /// 保存包含项目列表
    /// </summary>
    public void SaveIncludeList(List<string> includeList)
    {
        var config = Load();

        // 合并去重
        config.IncludeProjects = config.IncludeProjects
            .Concat(includeList)
            .Distinct()
            .ToList();

        Save(config);
    }

    /// <summary>
    /// 加载旧版本配置
    /// </summary>
    private AppConfiguration LoadLegacyConfig()
    {
        var config = new AppConfiguration();

        if (!File.Exists(_legacyConfigPath))
        {
            return config;
        }

        try
        {
            string content = File.ReadAllText(_legacyConfigPath, Encoding.UTF8);
            var projects = content
                .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            config.IncludeProjects = projects;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"警告: 读取旧版配置文件失败 - {ex.Message}");
        }

        return config;
    }
}
