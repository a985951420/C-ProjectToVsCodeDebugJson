using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using VsCodeDebugGen.Desktop.Services.Interfaces;

namespace VsCodeDebugGen.Desktop.Services;

/// <summary>
/// 剪贴板服务实现
/// </summary>
public class ClipboardService : IClipboardService
{
    /// <summary>
    /// 复制文本到剪贴板
    /// </summary>
    /// <param name="text">要复制的文本</param>
    public async Task SetTextAsync(string text)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = desktop.MainWindow;
            if (mainWindow != null)
            {
                var clipboard = mainWindow.Clipboard;
                if (clipboard != null)
                {
                    await clipboard.SetTextAsync(text);
                }
            }
        }
    }
}
