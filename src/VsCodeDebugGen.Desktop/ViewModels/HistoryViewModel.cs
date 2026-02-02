using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 历史记录视图模型
/// </summary>
public class HistoryViewModel : ViewModelBase
{
    private readonly IHistoryService _historyService;
    private readonly IDialogService _dialogService;
    private readonly ILoggingService _loggingService;
    private string _filterText = string.Empty;
    private HistoryEntry? _selectedEntry;

    /// <summary>
    /// 历史记录列表
    /// </summary>
    public ObservableCollection<HistoryEntry> History { get; }

    /// <summary>
    /// 过滤后的历史记录列表
    /// </summary>
    public ObservableCollection<HistoryEntry> FilteredHistory { get; }

    /// <summary>
    /// 过滤文本
    /// </summary>
    public string FilterText
    {
        get => _filterText;
        set
        {
            this.RaiseAndSetIfChanged(ref _filterText, value);
            ApplyFilter();
        }
    }

    /// <summary>
    /// 选中的历史记录
    /// </summary>
    public HistoryEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => this.RaiseAndSetIfChanged(ref _selectedEntry, value);
    }

    /// <summary>
    /// 重新生成命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RegenerateCommand { get; }

    /// <summary>
    /// 清空历史命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ClearHistoryCommand { get; }

    /// <summary>
    /// 删除条目命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteEntryCommand { get; }

    /// <summary>
    /// 搜索命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    /// <summary>
    /// 刷新命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    /// <summary>
    /// 初始化历史记录视图模型
    /// </summary>
    /// <param name="historyService">历史服务</param>
    /// <param name="dialogService">对话框服务</param>
    /// <param name="loggingService">日志服务</param>
    public HistoryViewModel(
        IHistoryService historyService,
        IDialogService dialogService,
        ILoggingService loggingService)
    {
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

        History = new ObservableCollection<HistoryEntry>();
        FilteredHistory = new ObservableCollection<HistoryEntry>();
        Title = "历史记录";

        // 初始化命令
        var canExecuteWithSelection = this.WhenAnyValue(
            x => x.SelectedEntry,
            (HistoryEntry? entry) => entry != null);

        RegenerateCommand = ReactiveCommand.CreateFromTask(RegenerateAsync, canExecuteWithSelection);
        ClearHistoryCommand = ReactiveCommand.CreateFromTask(ClearHistoryAsync);
        DeleteEntryCommand = ReactiveCommand.CreateFromTask(DeleteEntryAsync, canExecuteWithSelection);
        SearchCommand = ReactiveCommand.Create(ApplyFilter);
        RefreshCommand = ReactiveCommand.CreateFromTask(LoadHistoryAsync);

        // 加载历史记录
        _ = LoadHistoryAsync();
    }

    /// <summary>
    /// 加载历史记录
    /// </summary>
    private async Task LoadHistoryAsync()
    {
        try
        {
            IsBusy = true;
            _loggingService.Log("正在加载历史记录...", Models.LogLevel.Info);

            var history = await _historyService.LoadHistoryAsync();
            History.Clear();
            FilteredHistory.Clear();

            // 按时间倒序排列
            var sortedHistory = history.OrderByDescending(h => h.Timestamp).ToList();
            foreach (var entry in sortedHistory)
            {
                History.Add(entry);
                FilteredHistory.Add(entry);
            }

            _loggingService.Log($"成功加载 {history.Count} 条历史记录", Models.LogLevel.Info);
        }
        catch (Exception ex)
        {
            _loggingService.Log($"加载历史记录失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"加载历史记录失败: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 应用过滤
    /// </summary>
    private void ApplyFilter()
    {
        FilteredHistory.Clear();

        if (string.IsNullOrWhiteSpace(FilterText))
        {
            foreach (var entry in History)
            {
                FilteredHistory.Add(entry);
            }
        }
        else
        {
            var filter = FilterText.ToLower();
            var filtered = History.Where(entry =>
                entry.SearchPath.ToLower().Contains(filter) ||
                entry.ProjectPath.ToLower().Contains(filter) ||
                entry.ProjectNames.Any(name => name.ToLower().Contains(filter)));

            foreach (var entry in filtered)
            {
                FilteredHistory.Add(entry);
            }
        }

        _loggingService.Log($"过滤结果: {FilteredHistory.Count} / {History.Count} 条记录", Models.LogLevel.Info);
    }

    /// <summary>
    /// 重新生成配置文件
    /// </summary>
    private async Task RegenerateAsync()
    {
        if (SelectedEntry == null) return;

        try
        {
            _loggingService.Log($"重新生成配置: {SelectedEntry.ProjectPath}", Models.LogLevel.Info);

            // TODO: 实现重新生成逻辑
            // 需要调用配置生成服务，使用历史记录中保存的参数

            await _dialogService.ShowInformationAsync("配置文件重新生成功能即将实现");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"重新生成失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"重新生成失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 清空历史记录
    /// </summary>
    private async Task ClearHistoryAsync()
    {
        try
        {
            var confirm = await _dialogService.ShowConfirmationAsync(
                "确定要清空所有历史记录吗？此操作无法撤销。",
                "确认清空");

            if (!confirm) return;

            _loggingService.Log("清空历史记录...", Models.LogLevel.Info);

            await _historyService.ClearHistoryAsync();
            History.Clear();
            FilteredHistory.Clear();
            SelectedEntry = null;

            _loggingService.Log("历史记录已清空", Models.LogLevel.Info);
            await _dialogService.ShowInformationAsync("历史记录已成功清空");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"清空历史记录失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"清空历史记录失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 删除历史记录条目
    /// </summary>
    private async Task DeleteEntryAsync()
    {
        if (SelectedEntry == null) return;

        try
        {
            var confirm = await _dialogService.ShowConfirmationAsync(
                $"确定要删除该历史记录吗？\n项目: {SelectedEntry.ProjectPath}",
                "确认删除");

            if (!confirm) return;

            _loggingService.Log($"删除历史记录: {SelectedEntry.Id}", Models.LogLevel.Info);

            if (Guid.TryParse(SelectedEntry.Id, out var guid))
            {
                await _historyService.DeleteHistoryEntryAsync(guid);
                History.Remove(SelectedEntry);
                FilteredHistory.Remove(SelectedEntry);
                SelectedEntry = null;

                _loggingService.Log("历史记录已删除", Models.LogLevel.Info);
            }
            else
            {
                _loggingService.Log("无效的历史记录 ID", Models.LogLevel.Error);
                await _dialogService.ShowErrorAsync("无效的历史记录 ID");
            }
        }
        catch (Exception ex)
        {
            _loggingService.Log($"删除历史记录失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"删除历史记录失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 添加历史记录
    /// </summary>
    /// <param name="entry">历史记录条目</param>
    public void AddEntry(HistoryEntry entry)
    {
        History.Insert(0, entry);
        if (string.IsNullOrWhiteSpace(FilterText) || MatchesFilter(entry))
        {
            FilteredHistory.Insert(0, entry);
        }
    }

    /// <summary>
    /// 检查条目是否匹配过滤条件
    /// </summary>
    private bool MatchesFilter(HistoryEntry entry)
    {
        var filter = FilterText.ToLower();
        return entry.SearchPath.ToLower().Contains(filter) ||
               entry.ProjectPath.ToLower().Contains(filter) ||
               entry.ProjectNames.Any(name => name.ToLower().Contains(filter));
    }
}
