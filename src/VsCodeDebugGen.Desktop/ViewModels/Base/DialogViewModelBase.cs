using ReactiveUI;
using System.Reactive;

namespace VsCodeDebugGen.Desktop.ViewModels.Base;

/// <summary>
/// 对话框 ViewModel 基类
/// </summary>
public abstract class DialogViewModelBase : ViewModelBase
{
    private bool _dialogResult;

    /// <summary>
    /// 对话框结果
    /// </summary>
    public bool DialogResult
    {
        get => _dialogResult;
        set => this.RaiseAndSetIfChanged(ref _dialogResult, value);
    }

    /// <summary>
    /// 确认命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> OkCommand { get; }

    /// <summary>
    /// 取消命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    protected DialogViewModelBase()
    {
        OkCommand = ReactiveCommand.Create(OnOk);
        CancelCommand = ReactiveCommand.Create(OnCancel);
    }

    protected virtual void OnOk()
    {
        DialogResult = true;
    }

    protected virtual void OnCancel()
    {
        DialogResult = false;
    }
}
