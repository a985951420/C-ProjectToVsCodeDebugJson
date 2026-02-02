using ReactiveUI;
using System.Reactive;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 主窗口 ViewModel
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly ILoggingService _loggingService;
    private readonly IDialogService _dialogService;
    private ViewModelBase? _currentView;
    private int _selectedTabIndex;

    /// <summary>
    /// 当前显示的视图
    /// </summary>
    public ViewModelBase? CurrentView
    {
        get => _currentView;
        set => this.RaiseAndSetIfChanged(ref _currentView, value);
    }

    /// <summary>
    /// 选中的标签页索引
    /// </summary>
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
    }

    /// <summary>
    /// 关于命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> AboutCommand { get; }

    /// <summary>
    /// 退出命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    /// <summary>
    /// 初始化主窗口 ViewModel
    /// </summary>
    /// <param name="loggingService">日志服务</param>
    /// <param name="dialogService">对话框服务</param>
    public MainWindowViewModel(
        ILoggingService loggingService,
        IDialogService dialogService)
    {
        _loggingService = loggingService;
        _dialogService = dialogService;

        Title = "VsCode 调试配置生成工具 v3.0";

        // 初始化命令
        AboutCommand = ReactiveCommand.CreateFromTask(ShowAboutAsync);
        ExitCommand = ReactiveCommand.Create(Exit);

        // 记录启动日志
        _loggingService.Log("应用程序已启动", Models.LogLevel.Info);
    }

    /// <summary>
    /// 显示关于对话框
    /// </summary>
    private async System.Threading.Tasks.Task ShowAboutAsync()
    {
        await _dialogService.ShowInformationAsync(
            "VsCode 调试配置生成工具 v3.0\n\n" +
            "自动扫描 .NET 项目并生成 VSCode 调试配置文件。\n\n" +
            "版本: 3.0.0\n" +
            "框架: .NET 8.0 + Avalonia 11.x\n\n" +
            "© 2026 VsCodeDebugGen",
            "关于");
    }

    /// <summary>
    /// 退出应用程序
    /// </summary>
    private void Exit()
    {
        _loggingService.Log("应用程序正在退出", Models.LogLevel.Info);
        System.Environment.Exit(0);
    }
}
