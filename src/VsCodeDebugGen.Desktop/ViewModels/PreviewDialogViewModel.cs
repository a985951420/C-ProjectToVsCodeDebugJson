using ReactiveUI;
using System;
using System.Reactive;
using System.Windows.Input;
using Avalonia;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 预览对话框视图模型
/// </summary>
public class PreviewDialogViewModel : DialogViewModelBase
{
    private string _launchJson = string.Empty;
    private string _tasksJson = string.Empty;

    /// <summary>
    /// launch.json 内容
    /// </summary>
    public string LaunchJson
    {
        get => _launchJson;
        set => this.RaiseAndSetIfChanged(ref _launchJson, value);
    }

    /// <summary>
    /// tasks.json 内容
    /// </summary>
    public string TasksJson
    {
        get => _tasksJson;
        set => this.RaiseAndSetIfChanged(ref _tasksJson, value);
    }

    /// <summary>
    /// 复制 Launch 配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyLaunchCommand { get; }

    /// <summary>
    /// 复制 Tasks 配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyTasksCommand { get; }

    /// <summary>
    /// 初始化预览对话框视图模型
    /// </summary>
    public PreviewDialogViewModel()
    {
        Title = "配置预览";

        // 初始化命令
        CopyLaunchCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CopyToClipboardAsync(LaunchJson);
        });

        CopyTasksCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            await CopyToClipboardAsync(TasksJson);
        });
    }

    /// <summary>
    /// 初始化预览对话框视图模型（带参数）
    /// </summary>
    /// <param name="launchJson">launch.json 内容</param>
    /// <param name="tasksJson">tasks.json 内容</param>
    public PreviewDialogViewModel(string launchJson, string tasksJson) : this()
    {
        LaunchJson = launchJson;
        TasksJson = tasksJson;
    }

    /// <summary>
    /// 复制到剪贴板
    /// </summary>
    /// <param name="text">要复制的文本</param>
    private async System.Threading.Tasks.Task CopyToClipboardAsync(string text)
    {
        try
        {
            var clipboard = Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
                ? desktop.MainWindow?.Clipboard
                : null;

            if (clipboard != null)
            {
                await clipboard.SetTextAsync(text);
            }
        }
        catch (Exception ex)
        {
            // 记录错误，但不影响用户操作
            System.Diagnostics.Debug.WriteLine($"复制到剪贴板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 确认操作
    /// </summary>
    protected override void OnOk()
    {
        DialogResult = true;
    }

    /// <summary>
    /// 取消操作
    /// </summary>
    protected override void OnCancel()
    {
        DialogResult = false;
    }
}
