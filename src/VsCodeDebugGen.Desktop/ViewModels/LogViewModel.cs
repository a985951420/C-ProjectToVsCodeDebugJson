using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 日志视图模型
/// </summary>
public class LogViewModel : ViewModelBase
{
    private readonly ILoggingService _loggingService;
    private readonly IDialogService _dialogService;

    private LogLevel _selectedLogLevel = LogLevel.Info;
    private string _searchText = string.Empty;

    /// <summary>
    /// 日志集合
    /// </summary>
    public ObservableCollection<LogEntry> Logs => _loggingService.Logs;

    /// <summary>
    /// 选中的日志级别过滤
    /// </summary>
    public LogLevel SelectedLogLevel
    {
        get => _selectedLogLevel;
        set => this.RaiseAndSetIfChanged(ref _selectedLogLevel, value);
    }

    /// <summary>
    /// 搜索文本
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    /// <summary>
    /// 清空日志命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ClearCommand { get; }

    /// <summary>
    /// 导出日志命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ExportCommand { get; }

    /// <summary>
    /// 刷新命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    /// <summary>
    /// 初始化日志视图模型
    /// </summary>
    public LogViewModel(
        ILoggingService loggingService,
        IDialogService dialogService)
    {
        _loggingService = loggingService;
        _dialogService = dialogService;

        Title = "日志";

        // 初始化命令
        ClearCommand = ReactiveCommand.Create(ClearLogs);
        ExportCommand = ReactiveCommand.CreateFromTask(ExportLogsAsync);
        RefreshCommand = ReactiveCommand.Create(RefreshLogs);

        // 记录视图加载
        _loggingService.Log("日志视图已加载", LogLevel.Debug);
    }

    /// <summary>
    /// 清空日志
    /// </summary>
    private void ClearLogs()
    {
        _loggingService.Clear();
    }

    /// <summary>
    /// 导出日志
    /// </summary>
    private async Task ExportLogsAsync()
    {
        try
        {
            var defaultFileName = $"logs_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            var filePath = await _dialogService.ShowSaveFileDialogAsync(defaultFileName, "*.txt");

            if (!string.IsNullOrEmpty(filePath))
            {
                await _loggingService.ExportToFileAsync(filePath);
                await _dialogService.ShowMessageAsync("导出成功", $"日志已导出到:\n{filePath}");
            }
        }
        catch (Exception ex)
        {
            _loggingService.Log($"导出日志失败: {ex.Message}", LogLevel.Error);
            await _dialogService.ShowErrorAsync("导出失败", ex.Message);
        }
    }

    /// <summary>
    /// 刷新日志显示
    /// </summary>
    private void RefreshLogs()
    {
        _loggingService.Log("日志已刷新", LogLevel.Debug);
        this.RaisePropertyChanged(nameof(Logs));
    }
}
