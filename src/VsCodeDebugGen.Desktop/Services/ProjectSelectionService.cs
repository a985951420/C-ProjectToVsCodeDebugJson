using System.Collections.Generic;
using System.Linq;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 项目选择服务实现
/// </summary>
public class ProjectSelectionService : IProjectSelectionService
{
    private IEnumerable<ProjectItemViewModel> _selectedProjects = Enumerable.Empty<ProjectItemViewModel>();
    private string _searchPath = string.Empty;

    /// <summary>
    /// 获取或设置选中的项目列表
    /// </summary>
    public IEnumerable<ProjectItemViewModel> SelectedProjects
    {
        get => _selectedProjects;
        set => _selectedProjects = value ?? Enumerable.Empty<ProjectItemViewModel>();
    }

    /// <summary>
    /// 获取或设置搜索路径
    /// </summary>
    public string SearchPath
    {
        get => _searchPath;
        set => _searchPath = value ?? string.Empty;
    }
}
