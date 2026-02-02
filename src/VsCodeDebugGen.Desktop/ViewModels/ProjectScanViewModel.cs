using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 项目扫描视图模型
/// </summary>
public class ProjectScanViewModel : ViewModelBase
{
    private readonly IProjectFinder _projectFinder;
    private readonly ILoggingService _loggingService;
    private readonly IDialogService _dialogService;
    private readonly IHistoryService _historyService;
    private readonly IProjectSelectionService _projectSelectionService;

    private string _searchPath = string.Empty;
    private bool _isScanning;
    private int _foundProjectCount;

    /// <summary>
    /// 搜索路径
    /// </summary>
    public string SearchPath
    {
        get => _searchPath;
        set => this.RaiseAndSetIfChanged(ref _searchPath, value);
    }

    /// <summary>
    /// 是否正在扫描
    /// </summary>
    public bool IsScanning
    {
        get => _isScanning;
        set => this.RaiseAndSetIfChanged(ref _isScanning, value);
    }

    /// <summary>
    /// 找到的项目数量
    /// </summary>
    public int FoundProjectCount
    {
        get => _foundProjectCount;
        set => this.RaiseAndSetIfChanged(ref _foundProjectCount, value);
    }

    /// <summary>
    /// 项目列表
    /// </summary>
    public ObservableCollection<ProjectItemViewModel> Projects { get; } = new();

    /// <summary>
    /// 扫描命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ScanCommand { get; }

    /// <summary>
    /// 浏览文件夹命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> BrowseCommand { get; }

    /// <summary>
    /// 清空项目列表命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    /// <summary>
    /// 选择所有项目命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> SelectAllCommand { get; }

    /// <summary>
    /// 取消选择所有项目命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeselectAllCommand { get; }

    /// <summary>
    /// 初始化项目扫描视图模型
    /// </summary>
    public ProjectScanViewModel(
        IProjectFinder projectFinder,
        ILoggingService loggingService,
        IDialogService dialogService,
        IHistoryService historyService,
        IProjectSelectionService projectSelectionService)
    {
        _projectFinder = projectFinder;
        _loggingService = loggingService;
        _dialogService = dialogService;
        _historyService = historyService;
        _projectSelectionService = projectSelectionService;

        Title = "项目扫描";

        // 初始化命令
        ScanCommand = ReactiveCommand.CreateFromTask(ScanProjectsAsync);
        BrowseCommand = ReactiveCommand.CreateFromTask(BrowseFolderAsync);
        ClearCommand = ReactiveCommand.Create(ClearProjects);
        SelectAllCommand = ReactiveCommand.Create(SelectAll);
        DeselectAllCommand = ReactiveCommand.Create(DeselectAll);
    }

    /// <summary>
    /// 扫描项目
    /// </summary>
    private async Task ScanProjectsAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchPath))
        {
            await _dialogService.ShowMessageAsync("提示", "请先选择要扫描的文件夹");
            return;
        }

        if (!System.IO.Directory.Exists(SearchPath))
        {
            await _dialogService.ShowMessageAsync("错误", "指定的路径不存在");
            return;
        }

        IsScanning = true;
        IsBusy = true;

        try
        {
            _loggingService.Log($"开始扫描项目: {SearchPath}", Models.LogLevel.Info);

            // 扫描项目文件
            var projectFiles = _projectFinder.FindProjects(SearchPath);

            // 清空现有列表
            Projects.Clear();

            // 添加到列表
            foreach (var projectFile in projectFiles)
            {
                var projectItem = new ProjectItemViewModel
                {
                    ProjectPath = projectFile,
                    ProjectName = System.IO.Path.GetFileNameWithoutExtension(projectFile),
                    IsSelected = true
                };

                Projects.Add(projectItem);
            }

            FoundProjectCount = Projects.Count;

            _loggingService.Log($"扫描完成，找到 {FoundProjectCount} 个项目", Models.LogLevel.Success);

            // 保存到项目选择服务
            _projectSelectionService.SelectedProjects = Projects.ToList();
            _projectSelectionService.SearchPath = SearchPath;

            // 添加到历史记录
            if (FoundProjectCount > 0)
            {
                await _historyService.AddHistoryEntryAsync(new HistoryEntry
                {
                    Id = Guid.NewGuid().ToString(),
                    SearchPath = SearchPath,
                    ProjectNames = Projects.Select(p => p.ProjectName).ToList(),
                    Timestamp = DateTime.Now
                });
            }
        }
        catch (Exception ex)
        {
            _loggingService.Log($"扫描失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync("扫描失败", ex.Message);
        }
        finally
        {
            IsScanning = false;
            IsBusy = false;
        }
    }

    /// <summary>
    /// 浏览文件夹
    /// </summary>
    private async Task BrowseFolderAsync()
    {
        var selectedPath = await _dialogService.ShowFolderDialogAsync();

        if (!string.IsNullOrEmpty(selectedPath))
        {
            SearchPath = selectedPath;
            _loggingService.Log($"已选择路径: {selectedPath}", Models.LogLevel.Info);
        }
    }

    /// <summary>
    /// 清空项目列表
    /// </summary>
    private void ClearProjects()
    {
        Projects.Clear();
        FoundProjectCount = 0;
        _loggingService.Log("已清空项目列表", Models.LogLevel.Info);
    }

    /// <summary>
    /// 选择所有项目
    /// </summary>
    private void SelectAll()
    {
        foreach (var project in Projects)
        {
            project.IsSelected = true;
        }
        _loggingService.Log("已选择所有项目", Models.LogLevel.Info);
    }

    /// <summary>
    /// 取消选择所有项目
    /// </summary>
    private void DeselectAll()
    {
        foreach (var project in Projects)
        {
            project.IsSelected = false;
        }
        _loggingService.Log("已取消选择所有项目", Models.LogLevel.Info);
    }
}
