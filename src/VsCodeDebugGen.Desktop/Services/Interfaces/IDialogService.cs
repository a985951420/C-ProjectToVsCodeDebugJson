namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 对话框服务接口
/// </summary>
public interface IDialogService
{
    /// <summary>
    /// 显示文件夹选择对话框
    /// </summary>
    /// <returns>选中的文件夹路径，如果取消则返回 null</returns>
    Task<string?> ShowFolderDialogAsync();

    /// <summary>
    /// 显示确认对话框
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="title">对话框标题</param>
    /// <returns>true 表示确认，false 表示取消</returns>
    Task<bool> ShowConfirmationAsync(string message, string title = "确认");

    /// <summary>
    /// 显示错误对话框
    /// </summary>
    /// <param name="message">错误消息</param>
    /// <param name="title">对话框标题</param>
    Task ShowErrorAsync(string message, string title = "错误");

    /// <summary>
    /// 显示信息对话框
    /// </summary>
    /// <param name="message">信息消息</param>
    /// <param name="title">对话框标题</param>
    Task ShowInformationAsync(string message, string title = "信息");

    /// <summary>
    /// 显示消息对话框
    /// </summary>
    /// <param name="message">消息内容</param>
    /// <param name="title">对话框标题</param>
    Task ShowMessageAsync(string message, string title = "提示");

    /// <summary>
    /// 显示保存文件对话框
    /// </summary>
    /// <param name="defaultFileName">默认文件名</param>
    /// <param name="filter">文件过滤器</param>
    /// <returns>选中的文件路径，如果取消则返回 null</returns>
    Task<string?> ShowSaveFileDialogAsync(string defaultFileName = "", string filter = "All files (*.*)|*.*");

    /// <summary>
    /// 设置主窗口（用于对话框的所有者）
    /// </summary>
    /// <param name="window">主窗口实例</param>
    void SetMainWindow(object window);
}
