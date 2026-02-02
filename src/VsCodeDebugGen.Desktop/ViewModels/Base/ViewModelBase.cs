using ReactiveUI;

namespace VsCodeDebugGen.Desktop.ViewModels.Base;

/// <summary>
/// ViewModel 基类，提供通用属性和功能
/// </summary>
public abstract class ViewModelBase : ReactiveObject
{
    private bool _isBusy;
    private string _title = string.Empty;

    /// <summary>
    /// 指示 ViewModel 是否正在执行操作
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    /// <summary>
    /// 视图标题
    /// </summary>
    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    protected ViewModelBase()
    {
        // 初始化代码
    }
}
