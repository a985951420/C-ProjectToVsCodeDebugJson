using ReactiveUI;
using System;
using System.ComponentModel.DataAnnotations;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 模板编辑对话框视图模型
/// </summary>
public class TemplateEditDialogViewModel : DialogViewModelBase
{
    private TemplateModel _template;
    private string _templateName = string.Empty;
    private string _templateDescription = string.Empty;
    private string? _nameError;
    private string? _descriptionError;

    /// <summary>
    /// 模板对象
    /// </summary>
    public TemplateModel Template
    {
        get => _template;
        set
        {
            this.RaiseAndSetIfChanged(ref _template, value);
            if (value != null)
            {
                TemplateName = value.Name;
                TemplateDescription = value.Description;
            }
        }
    }

    /// <summary>
    /// 模板名称
    /// </summary>
    [Required(ErrorMessage = "模板名称不能为空")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "模板名称长度必须在 1-100 个字符之间")]
    public string TemplateName
    {
        get => _templateName;
        set
        {
            this.RaiseAndSetIfChanged(ref _templateName, value);
            ValidateName();
        }
    }

    /// <summary>
    /// 模板描述
    /// </summary>
    [StringLength(500, ErrorMessage = "模板描述长度不能超过 500 个字符")]
    public string TemplateDescription
    {
        get => _templateDescription;
        set
        {
            this.RaiseAndSetIfChanged(ref _templateDescription, value);
            ValidateDescription();
        }
    }

    /// <summary>
    /// 名称验证错误信息
    /// </summary>
    public string? NameError
    {
        get => _nameError;
        set => this.RaiseAndSetIfChanged(ref _nameError, value);
    }

    /// <summary>
    /// 描述验证错误信息
    /// </summary>
    public string? DescriptionError
    {
        get => _descriptionError;
        set => this.RaiseAndSetIfChanged(ref _descriptionError, value);
    }

    /// <summary>
    /// 是否有验证错误
    /// </summary>
    public bool HasErrors => !string.IsNullOrEmpty(NameError) || !string.IsNullOrEmpty(DescriptionError);

    /// <summary>
    /// 初始化模板编辑对话框视图模型
    /// </summary>
    public TemplateEditDialogViewModel()
    {
        _template = new TemplateModel();
        Title = "编辑模板";

        // 监听属性变化以更新命令状态
        this.WhenAnyValue(
            x => x.TemplateName,
            x => x.TemplateDescription,
            x => x.HasErrors)
            .Subscribe(_ => { });
    }

    /// <summary>
    /// 初始化模板编辑对话框视图模型（带模板参数）
    /// </summary>
    /// <param name="template">要编辑的模板</param>
    public TemplateEditDialogViewModel(TemplateModel template) : this()
    {
        Template = template ?? throw new ArgumentNullException(nameof(template));
        Title = $"编辑模板 - {template.Name}";
    }

    /// <summary>
    /// 验证模板名称
    /// </summary>
    private void ValidateName()
    {
        if (string.IsNullOrWhiteSpace(TemplateName))
        {
            NameError = "模板名称不能为空";
        }
        else if (TemplateName.Length > 100)
        {
            NameError = "模板名称长度不能超过 100 个字符";
        }
        else
        {
            NameError = null;
        }
    }

    /// <summary>
    /// 验证模板描述
    /// </summary>
    private void ValidateDescription()
    {
        if (TemplateDescription != null && TemplateDescription.Length > 500)
        {
            DescriptionError = "模板描述长度不能超过 500 个字符";
        }
        else
        {
            DescriptionError = null;
        }
    }

    /// <summary>
    /// 确认操作
    /// </summary>
    protected override void OnOk()
    {
        ValidateName();
        ValidateDescription();

        if (HasErrors)
        {
            return;
        }

        // 更新模板对象
        Template.Name = TemplateName;
        Template.Description = TemplateDescription;

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
