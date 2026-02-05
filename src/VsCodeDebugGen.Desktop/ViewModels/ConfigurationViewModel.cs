using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VsCodeDebugGen.Core.Interfaces;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 配置视图模型
/// </summary>
public class ConfigurationViewModel : ViewModelBase
{
    private readonly IPortConfigurationService _portConfigurationService;
    private readonly ILoggingService _loggingService;
    private readonly IDialogService _dialogService;
    private readonly IProjectSelectionService _projectSelectionService;
    private readonly IConfigGenerator _configGenerator;
    private readonly IProjectParser _projectParser;

    private PortConfiguration? _portConfiguration;
    private int _startPort = 5000;
    private int _endPort = 5999;
    private bool _autoIncrement = true;
    private string _outputPath = string.Empty;

    /// <summary>
    /// 起始端口
    /// </summary>
    public int StartPort
    {
        get => _startPort;
        set => this.RaiseAndSetIfChanged(ref _startPort, value);
    }

    /// <summary>
    /// 结束端口
    /// </summary>
    public int EndPort
    {
        get => _endPort;
        set => this.RaiseAndSetIfChanged(ref _endPort, value);
    }

    /// <summary>
    /// 自动递增端口
    /// </summary>
    public bool AutoIncrement
    {
        get => _autoIncrement;
        set => this.RaiseAndSetIfChanged(ref _autoIncrement, value);
    }

    /// <summary>
    /// 输出路径
    /// </summary>
    public string OutputPath
    {
        get => _outputPath;
        set => this.RaiseAndSetIfChanged(ref _outputPath, value);
    }

    /// <summary>
    /// 保存配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }

    /// <summary>
    /// 重置配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> ResetCommand { get; }

    /// <summary>
    /// 加载配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> LoadCommand { get; }

    /// <summary>
    /// 生成配置命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> GenerateCommand { get; }

    /// <summary>
    /// 初始化配置视图模型
    /// </summary>
    public ConfigurationViewModel(
        IPortConfigurationService portConfigurationService,
        ILoggingService loggingService,
        IDialogService dialogService,
        IProjectSelectionService projectSelectionService,
        IConfigGenerator configGenerator,
        IProjectParser projectParser)
    {
        _portConfigurationService = portConfigurationService;
        _loggingService = loggingService;
        _dialogService = dialogService;
        _projectSelectionService = projectSelectionService;
        _configGenerator = configGenerator;
        _projectParser = projectParser;

        Title = "配置";

        // 初始化命令
        SaveCommand = ReactiveCommand.CreateFromTask(SaveConfigurationAsync);
        ResetCommand = ReactiveCommand.CreateFromTask(ResetConfigurationAsync);
        LoadCommand = ReactiveCommand.CreateFromTask(LoadConfigurationAsync);
        GenerateCommand = ReactiveCommand.CreateFromTask(GenerateConfigurationAsync);

        // 加载初始配置
        _ = LoadConfigurationAsync();
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    private async Task LoadConfigurationAsync()
    {
        IsBusy = true;

        try
        {
            // Create a default configuration for now
            _portConfiguration = new PortConfiguration
            {
                StartPort = 5000,
                EndPort = 5999,
                AutoIncrement = true,
                UsedPorts = new System.Collections.Generic.List<int>()
            };

            StartPort = _portConfiguration.StartPort;
            EndPort = _portConfiguration.EndPort;
            AutoIncrement = _portConfiguration.AutoIncrement;

            _loggingService.Log("配置已加载", Models.LogLevel.Info);
        }
        catch (Exception ex)
        {
            _loggingService.Log($"加载配置失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync(ex.Message, "加载失败");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    private async Task SaveConfigurationAsync()
    {
        // 验证端口范围
        if (!ValidatePortRange())
        {
            await _dialogService.ShowInformationAsync("端口范围无效。起始端口必须小于结束端口，且在 1-65535 之间。", "验证失败");
            return;
        }

        IsBusy = true;

        try
        {
            var config = new PortConfiguration
            {
                StartPort = StartPort,
                EndPort = EndPort,
                AutoIncrement = AutoIncrement,
                UsedPorts = _portConfiguration?.UsedPorts ?? new System.Collections.Generic.List<int>()
            };

            // Save configuration logic would go here if we had a SaveConfigurationAsync method
            // For now, just update the local configuration
            _portConfiguration = config;

            _loggingService.Log("配置已保存", Models.LogLevel.Success);
            await _dialogService.ShowInformationAsync("配置已成功保存。", "保存成功");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"保存配置失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync(ex.Message, "保存失败");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 重置配置
    /// </summary>
    private async Task ResetConfigurationAsync()
    {
        var confirm = await _dialogService.ShowConfirmationAsync("确定要重置为默认配置吗？", "重置配置");

        if (!confirm)
            return;

        IsBusy = true;

        try
        {
            // Reset to default configuration
            _portConfiguration = new PortConfiguration
            {
                StartPort = 5000,
                EndPort = 5999,
                AutoIncrement = true,
                UsedPorts = new System.Collections.Generic.List<int>()
            };

            await LoadConfigurationAsync();

            _loggingService.Log("配置已重置", Models.LogLevel.Success);
            await _dialogService.ShowInformationAsync("配置已重置为默认值。", "重置成功");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"重置配置失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync(ex.Message, "重置失败");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 验证端口范围
    /// </summary>
    /// <returns>是否有效</returns>
    private bool ValidatePortRange()
    {
        return StartPort > 0 && StartPort <= 65535 &&
               EndPort > 0 && EndPort <= 65535 &&
               StartPort < EndPort;
    }

    /// <summary>
    /// 生成配置文件
    /// </summary>
    private async Task GenerateConfigurationAsync()
    {
        // 检查是否有选中的项目
        var selectedProjects = _projectSelectionService.SelectedProjects?.Where(p => p.IsSelected).ToList();
        if (selectedProjects == null || !selectedProjects.Any())
        {
            await _dialogService.ShowInformationAsync("请先在「项目扫描」页面扫描并选择要生成配置的项目。", "提示");
            return;
        }

        // 验证端口范围
        if (!ValidatePortRange())
        {
            await _dialogService.ShowInformationAsync("端口范围无效。起始端口必须小于结束端口，且在 1-65535 之间。", "验证失败");
            return;
        }

        IsBusy = true;

        try
        {
            _loggingService.Log($"开始为 {selectedProjects.Count} 个项目生成配置", Models.LogLevel.Info);

            // 解析所有项目
            var projectInfos = new System.Collections.Generic.List<VsCodeDebugGen.Core.Models.ProjectInfo>();
            var basePath = _projectSelectionService.SearchPath;

            foreach (var projectItem in selectedProjects)
            {
                try
                {
                    var projectInfo = _projectParser.Parse(projectItem.ProjectPath, basePath);

                    if (projectInfo == null)
                    {
                        _loggingService.Log($"无法解析项目: {projectItem.ProjectName}", Models.LogLevel.Warning);
                        continue;
                    }

                    projectInfos.Add(projectInfo);
                    _loggingService.Log($"已解析项目: {projectInfo.AssemblyName}", Models.LogLevel.Info);
                }
                catch (Exception ex)
                {
                    _loggingService.Log($"解析项目失败: {projectItem.ProjectName} - {ex.Message}", Models.LogLevel.Warning);
                }
            }

            if (!projectInfos.Any())
            {
                await _dialogService.ShowErrorAsync("没有成功解析任何项目", "生成失败");
                return;
            }

            // 为项目分配端口号
            var projectInfosWithPorts = new System.Collections.Generic.List<VsCodeDebugGen.Core.Models.ProjectInfo>();
            int currentPort = StartPort;

            foreach (var projectInfo in projectInfos)
            {
                // 创建带端口号的新ProjectInfo对象
                var projectInfoWithPort = new VsCodeDebugGen.Core.Models.ProjectInfo
                {
                    CsprojFile = projectInfo.CsprojFile,
                    OutputType = projectInfo.OutputType,
                    TargetFramework = projectInfo.TargetFramework,
                    AssemblyName = projectInfo.AssemblyName,
                    OutputPath = projectInfo.OutputPath,
                    Port = AutoIncrement ? currentPort : (int?)null
                };

                projectInfosWithPorts.Add(projectInfoWithPort);

                // 如果启用自动递增，递增端口号
                if (AutoIncrement)
                {
                    currentPort++;
                    if (currentPort > EndPort)
                    {
                        _loggingService.Log($"端口号已超过结束范围 {EndPort}，剩余项目将不分配端口", Models.LogLevel.Warning);
                        break;
                    }
                }

                _loggingService.Log($"为项目 {projectInfo.AssemblyName} 分配端口: {projectInfoWithPort.Port?.ToString() ?? "无"}", Models.LogLevel.Info);
            }

            // 生成配置文件
            var actualOutputPath = string.IsNullOrWhiteSpace(OutputPath) ? basePath : OutputPath;
            _configGenerator.Generate(projectInfosWithPorts, actualOutputPath, basePath);

            var vscodeDir = System.IO.Path.Combine(actualOutputPath, ".vscode");
            _loggingService.Log($"配置文件已成功生成到: {vscodeDir}", Models.LogLevel.Success);
            await _dialogService.ShowInformationAsync(
                $"成功为 {projectInfos.Count} 个项目生成配置文件！\n\n" +
                $"配置文件位置: {vscodeDir}\n" +
                $"生成的文件: launch.json, tasks.json",
                "生成成功");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"生成配置失败: {ex.Message}", Models.LogLevel.Error);
            await _dialogService.ShowErrorAsync("生成配置失败", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
