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
/// 模板管理器视图模型
/// </summary>
public class TemplateManagerViewModel : ViewModelBase
{
    private readonly ITemplateService _templateService;
    private readonly IDialogService _dialogService;
    private readonly ILoggingService _loggingService;
    private TemplateModel? _selectedTemplate;

    /// <summary>
    /// 模板列表
    /// </summary>
    public ObservableCollection<TemplateModel> Templates { get; }

    /// <summary>
    /// 当前选中的模板
    /// </summary>
    public TemplateModel? SelectedTemplate
    {
        get => _selectedTemplate;
        set => this.RaiseAndSetIfChanged(ref _selectedTemplate, value);
    }

    /// <summary>
    /// 新建模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> NewTemplateCommand { get; }

    /// <summary>
    /// 应用模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ApplyTemplateCommand { get; }

    /// <summary>
    /// 编辑模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> EditTemplateCommand { get; }

    /// <summary>
    /// 删除模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteTemplateCommand { get; }

    /// <summary>
    /// 导入模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ImportTemplateCommand { get; }

    /// <summary>
    /// 导出模板命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ExportTemplateCommand { get; }

    /// <summary>
    /// 刷新模板列表命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    /// <summary>
    /// 初始化模板管理器视图模型
    /// </summary>
    /// <param name="templateService">模板服务</param>
    /// <param name="dialogService">对话框服务</param>
    /// <param name="loggingService">日志服务</param>
    public TemplateManagerViewModel(
        ITemplateService templateService,
        IDialogService dialogService,
        ILoggingService loggingService)
    {
        _templateService = templateService ?? throw new ArgumentNullException(nameof(templateService));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

        Templates = new ObservableCollection<TemplateModel>();
        Title = "模板管理";

        // 初始化命令
        NewTemplateCommand = ReactiveCommand.CreateFromTask(NewTemplateAsync);
        var canExecuteWithSelection = this.WhenAnyValue(
            x => x.SelectedTemplate,
            (TemplateModel? template) => template != null);

        ApplyTemplateCommand = ReactiveCommand.CreateFromTask(ApplyTemplateAsync, canExecuteWithSelection);
        EditTemplateCommand = ReactiveCommand.CreateFromTask(EditTemplateAsync, canExecuteWithSelection);
        DeleteTemplateCommand = ReactiveCommand.CreateFromTask(DeleteTemplateAsync, canExecuteWithSelection);
        ImportTemplateCommand = ReactiveCommand.CreateFromTask(ImportTemplateAsync);
        ExportTemplateCommand = ReactiveCommand.CreateFromTask(ExportTemplateAsync, canExecuteWithSelection);
        RefreshCommand = ReactiveCommand.CreateFromTask(LoadTemplatesAsync);

        // 加载模板列表
        _ = LoadTemplatesAsync();
    }

    /// <summary>
    /// 加载模板列表
    /// </summary>
    private async Task LoadTemplatesAsync()
    {
        try
        {
            IsBusy = true;
            _loggingService.Log("正在加载模板列表...", Models.LogLevel.Info);

            var templates = await _templateService.GetTemplateListAsync();
            Templates.Clear();
            foreach (var template in templates)
            {
                Templates.Add(template);
            }

            _loggingService.Log($"成功加载 {templates.Count} 个模板", Models.LogLevel.Info);
        }
        catch (Exception ex)
        {
            _loggingService.Log($"加载模板列表失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"加载模板列表失败: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 新建模板
    /// </summary>
    private async Task NewTemplateAsync()
    {
        try
        {
            _loggingService.Log("创建新模板...", Models.LogLevel.Info);

            var newTemplate = new TemplateModel
            {
                Name = $"模板 {DateTime.Now:yyyyMMdd_HHmmss}",
                Description = "新建的模板",
                CreatedDate = DateTime.Now
            };

            // 显示编辑对话框
            // TODO: 实现模板编辑对话框
            await _templateService.SaveTemplateAsync(newTemplate);
            Templates.Add(newTemplate);

            _loggingService.Log($"成功创建模板: {newTemplate.Name}", Models.LogLevel.Info);
            await _dialogService.ShowInformationAsync("模板创建成功！");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"创建模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"创建模板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 应用模板
    /// </summary>
    private async Task ApplyTemplateAsync()
    {
        if (SelectedTemplate == null) return;

        try
        {
            _loggingService.Log($"应用模板: {SelectedTemplate.Name}", Models.LogLevel.Info);

            // TODO: 实现模板应用逻辑，将模板配置应用到当前配置

            await _dialogService.ShowInformationAsync($"已应用模板: {SelectedTemplate.Name}");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"应用模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"应用模板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 编辑模板
    /// </summary>
    private async Task EditTemplateAsync()
    {
        if (SelectedTemplate == null) return;

        try
        {
            _loggingService.Log($"编辑模板: {SelectedTemplate.Name}", Models.LogLevel.Info);

            // TODO: 显示编辑对话框

            await _templateService.SaveTemplateAsync(SelectedTemplate);
            _loggingService.Log($"模板已保存: {SelectedTemplate.Name}", Models.LogLevel.Info);
        }
        catch (Exception ex)
        {
            _loggingService.Log($"编辑模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"编辑模板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 删除模板
    /// </summary>
    private async Task DeleteTemplateAsync()
    {
        if (SelectedTemplate == null) return;

        try
        {
            var confirm = await _dialogService.ShowConfirmationAsync(
                $"确定要删除模板 '{SelectedTemplate.Name}' 吗？",
                "确认删除");

            if (!confirm) return;

            _loggingService.Log($"删除模板: {SelectedTemplate.Name}", Models.LogLevel.Info);

            await _templateService.DeleteTemplateAsync(SelectedTemplate.Name);
            Templates.Remove(SelectedTemplate);
            SelectedTemplate = null;

            _loggingService.Log("模板已删除", Models.LogLevel.Info);
            await _dialogService.ShowInformationAsync("模板已成功删除");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"删除模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"删除模板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 导入模板
    /// </summary>
    private async Task ImportTemplateAsync()
    {
        try
        {
            _loggingService.Log("导入模板...", Models.LogLevel.Info);

            // TODO: 显示文件选择对话框
            // var filePath = await _dialogService.ShowOpenFileDialogAsync();
            // if (string.IsNullOrEmpty(filePath)) return;

            // var template = await _templateService.ImportTemplateAsync(filePath);
            // if (template != null)
            // {
            //     Templates.Add(template);
            //     _loggingService.Log($"成功导入模板: {template.Name}", Models.LogLevel.Info);
            //     await _dialogService.ShowInformationAsync("模板导入成功！");
            // }

            await _dialogService.ShowInformationAsync("导入功能即将实现");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"导入模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"导入模板失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 导出模板
    /// </summary>
    private async Task ExportTemplateAsync()
    {
        if (SelectedTemplate == null) return;

        try
        {
            _loggingService.Log($"导出模板: {SelectedTemplate.Name}", Models.LogLevel.Info);

            var filePath = await _dialogService.ShowSaveFileDialogAsync(
                $"{SelectedTemplate.Name}.json",
                "JSON文件 (*.json)|*.json");

            if (string.IsNullOrEmpty(filePath)) return;

            await _templateService.ExportTemplateAsync(SelectedTemplate, filePath);

            _loggingService.Log($"模板已导出到: {filePath}", Models.LogLevel.Info);
            await _dialogService.ShowInformationAsync($"模板已成功导出到:\n{filePath}");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"导出模板失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync($"导出模板失败: {ex.Message}");
        }
    }
}
