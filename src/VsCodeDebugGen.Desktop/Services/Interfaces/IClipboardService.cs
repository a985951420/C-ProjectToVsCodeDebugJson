using System.Threading.Tasks;

namespace VsCodeDebugGen.Desktop.Services.Interfaces;

/// <summary>
/// 剪贴板服务接口
/// </summary>
public interface IClipboardService
{
    /// <summary>
    /// 复制文本到剪贴板
    /// </summary>
    /// <param name="text">要复制的文本</param>
    Task SetTextAsync(string text);
}
