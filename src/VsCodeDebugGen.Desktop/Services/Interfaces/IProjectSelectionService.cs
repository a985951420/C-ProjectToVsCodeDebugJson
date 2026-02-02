using System.Collections.Generic;
using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 项目选择服务接口 - 用于在 ViewModel 之间共享选中的项目
/// </summary>
public interface IProjectSelectionService
{
    /// <summary>
    /// 获取或设置选中的项目列表
    /// </summary>
    IEnumerable<ProjectItemViewModel> SelectedProjects { get; set; }

    /// <summary>
    /// 获取或设置搜索路径
    /// </summary>
    string SearchPath { get; set; }
}
