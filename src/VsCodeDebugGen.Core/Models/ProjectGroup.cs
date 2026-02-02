namespace VsCodeDebugGen.Core.Models;

/// <summary>
/// 项目分组
/// </summary>
public class ProjectGroup
{
    public string GroupName { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
    public List<string> Projects { get; set; } = new();
    public int Count => Projects.Count;
}
