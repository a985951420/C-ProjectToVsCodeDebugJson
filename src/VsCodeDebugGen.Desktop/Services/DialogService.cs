using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using VsCodeDebugGen.Desktop.Services.Interfaces;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 对话框服务实现
/// </summary>
public class DialogService : IDialogService
{
    private Window? _mainWindow;

    /// <summary>
    /// 设置主窗口引用
    /// </summary>
    /// <param name="window">主窗口</param>
    public void SetMainWindow(object window)
    {
        _mainWindow = window as Window;
    }

    /// <summary>
    /// 显示文件夹选择对话框
    /// </summary>
    /// <returns>选中的文件夹路径，如果取消则返回 null</returns>
    public async Task<string?> ShowFolderDialogAsync()
    {
        if (_mainWindow == null)
            return null;

        var folders = await _mainWindow.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "选择文件夹",
            AllowMultiple = false
        });

        return folders.Count > 0 ? folders[0].Path.LocalPath : null;
    }

    /// <summary>
    /// 显示确认对话框
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="title">对话框标题</param>
    /// <returns>true 表示确认，false 表示取消</returns>
    public async Task<bool> ShowConfirmationAsync(string message, string title = "确认")
    {
        if (_mainWindow == null)
            return false;

        var result = false;
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var textBlock = new TextBlock
        {
            Text = message,
            Margin = new Avalonia.Thickness(20),
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };

        var buttonPanel = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 10, 0, 20)
        };

        var yesButton = new Button
        {
            Content = "确定",
            Margin = new Avalonia.Thickness(5)
        };
        yesButton.Click += (_, _) =>
        {
            result = true;
            dialog.Close();
        };

        var noButton = new Button
        {
            Content = "取消",
            Margin = new Avalonia.Thickness(5)
        };
        noButton.Click += (_, _) => dialog.Close();

        buttonPanel.Children.Add(yesButton);
        buttonPanel.Children.Add(noButton);

        var panel = new StackPanel();
        panel.Children.Add(textBlock);
        panel.Children.Add(buttonPanel);

        dialog.Content = panel;

        await dialog.ShowDialog(_mainWindow);
        return result;
    }

    /// <summary>
    /// 显示错误对话框
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="title">对话框标题</param>
    public async Task ShowErrorAsync(string message, string title = "错误")
    {
        await ShowInformationAsync(message, title);
    }

    /// <summary>
    /// 显示信息对话框
    /// </summary>
    /// <param name="message">信息消息</param>
    /// <param name="title">对话框标题</param>
    public async Task ShowInformationAsync(string message, string title = "信息")
    {
        if (_mainWindow == null)
            return;

        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };

        var textBlock = new TextBlock
        {
            Text = message,
            Margin = new Avalonia.Thickness(20),
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };

        var button = new Button
        {
            Content = "确定",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Margin = new Avalonia.Thickness(0, 10, 0, 20)
        };

        button.Click += (_, _) => dialog.Close();

        var panel = new StackPanel();
        panel.Children.Add(textBlock);
        panel.Children.Add(button);

        dialog.Content = panel;

        await dialog.ShowDialog(_mainWindow);
    }

    /// <summary>
    /// 显示消息对话框
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="title">对话框标题</param>
    public async Task ShowMessageAsync(string message, string title = "提示")
    {
        await ShowInformationAsync(message, title);
    }

    /// <summary>
    /// 显示保存文件对话框
    /// </summary>
    /// <param name="defaultFileName">默认文件名</param>
    /// <param name="filter">文件过滤器</param>
    /// <returns>选中的文件路径，如果取消则返回 null</returns>
    public async Task<string?> ShowSaveFileDialogAsync(string defaultFileName = "", string filter = "All files (*.*)|*.*")
    {
        if (_mainWindow == null)
            return null;

        var file = await _mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存文件",
            SuggestedFileName = defaultFileName,
            DefaultExtension = Path.GetExtension(defaultFileName)
        });

        return file?.Path.LocalPath;
    }
}
