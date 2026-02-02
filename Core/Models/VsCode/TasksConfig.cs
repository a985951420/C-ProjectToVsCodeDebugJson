using System.Text.Json.Serialization;

namespace VsCodeDebugGen.Core.Models.VsCode;

/// <summary>
/// tasks.json 配置根对象
/// </summary>
public sealed class TasksConfig
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = "2.0.0";

    [JsonPropertyName("tasks")]
    public TaskItem[] Tasks { get; set; } = Array.Empty<TaskItem>();
}

/// <summary>
/// 单个任务配置
/// </summary>
public sealed class TaskItem
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = "build";

    [JsonPropertyName("command")]
    public string Command { get; set; } = "dotnet";

    [JsonPropertyName("type")]
    public string Type { get; set; } = "process";

    [JsonPropertyName("args")]
    public string[] Args { get; set; } = Array.Empty<string>();

    [JsonPropertyName("problemMatcher")]
    public string ProblemMatcher { get; set; } = "$msCompile";

    [JsonPropertyName("group")]
    public TaskGroup? Group { get; set; }
}

/// <summary>
/// 任务分组
/// </summary>
public sealed class TaskGroup
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "build";

    [JsonPropertyName("isDefault")]
    public bool IsDefault { get; set; } = true;
}
