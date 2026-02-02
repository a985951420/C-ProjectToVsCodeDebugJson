using System.Text.Json.Serialization;

namespace VsCodeDebugGen.Core.Models.VsCode;

/// <summary>
/// launch.json 配置根对象
/// </summary>
public sealed class LaunchConfig
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = "0.2.0";

    [JsonPropertyName("configurations")]
    public LaunchConfiguration[] Configurations { get; set; } = Array.Empty<LaunchConfiguration>();
}

/// <summary>
/// 单个启动配置
/// </summary>
public sealed class LaunchConfiguration
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "coreclr";

    [JsonPropertyName("request")]
    public string Request { get; set; } = "launch";

    [JsonPropertyName("preLaunchTask")]
    public string PreLaunchTask { get; set; } = "build";

    [JsonPropertyName("program")]
    public required string Program { get; set; }

    [JsonPropertyName("args")]
    public string[] Args { get; set; } = Array.Empty<string>();

    [JsonPropertyName("cwd")]
    public string Cwd { get; set; } = "${workspaceFolder}";

    [JsonPropertyName("console")]
    public string Console { get; set; } = "internalConsole";

    [JsonPropertyName("stopAtEntry")]
    public bool StopAtEntry { get; set; } = false;

    [JsonPropertyName("env")]
    public Dictionary<string, string> Env { get; set; } = new()
    {
        { "ASPNETCORE_ENVIRONMENT", "Development" }
    };

    [JsonPropertyName("sourceFileMap")]
    public Dictionary<string, string> SourceFileMap { get; set; } = new();
}
