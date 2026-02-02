using ReactiveUI;
using System.Collections.ObjectModel;

namespace VsCodeDebugGen.Desktop.Models;

/// <summary>
/// 项目项视图模型
/// </summary>
public class ProjectItemViewModel : ReactiveObject
{
    private bool _isSelected = true;
    private bool _isExpanded;
    private string _projectPath = string.Empty;
    private string _projectName = string.Empty;
    private string _projectType = string.Empty;
    private string _targetFramework = string.Empty;

    /// <summary>
    /// 是否选中
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    /// <summary>
    /// 是否展开
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
    }

    /// <summary>
    /// 项目文件路径
    /// </summary>
    public string ProjectPath
    {
        get => _projectPath;
        set => this.RaiseAndSetIfChanged(ref _projectPath, value);
    }

    /// <summary>
    /// 项目名称
    /// </summary>
    public string ProjectName
    {
        get => _projectName;
        set => this.RaiseAndSetIfChanged(ref _projectName, value);
    }

    /// <summary>
    /// 项目类型（Web应用、控制台、类库等）
    /// </summary>
    public string ProjectType
    {
        get => _projectType;
        set => this.RaiseAndSetIfChanged(ref _projectType, value);
    }

    /// <summary>
    /// 目标框架
    /// </summary>
    public string TargetFramework
    {
        get => _targetFramework;
        set => this.RaiseAndSetIfChanged(ref _targetFramework, value);
    }

    /// <summary>
    /// 子项目
    /// </summary>
    public ObservableCollection<ProjectItemViewModel> Children { get; set; } = new();

    /// <summary>
    /// 项目类型图标
    /// </summary>
    public string Icon => ProjectType switch
    {
        "Web" => "🌐",
        "Console" => "⌨",
        "Library" => "📚",
        _ => "📄"
    };
}
