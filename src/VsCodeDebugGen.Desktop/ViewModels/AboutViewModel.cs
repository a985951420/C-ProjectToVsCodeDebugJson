using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VsCodeDebugGen.Desktop.Models;
using VsCodeDebugGen.Desktop.Services.Interfaces;
using VsCodeDebugGen.Desktop.ViewModels.Base;

namespace VsCodeDebugGen.Desktop.ViewModels;

/// <summary>
/// 关于页面 ViewModel
/// </summary>
public class AboutViewModel : ViewModelBase
{
    private readonly ILoggingService _loggingService;
    private readonly IClipboardService _clipboardService;
    private ObservableCollection<DocumentItem> _documents = new();
    private DocumentItem? _selectedDocument;
    private string _documentContent = string.Empty;

    /// <summary>
    /// 文档列表
    /// </summary>
    public ObservableCollection<DocumentItem> Documents
    {
        get => _documents;
        set => this.RaiseAndSetIfChanged(ref _documents, value);
    }

    /// <summary>
    /// 选中的文档
    /// </summary>
    public DocumentItem? SelectedDocument
    {
        get => _selectedDocument;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDocument, value);
            if (value != null)
            {
                _ = LoadDocumentContentAsync(value);
            }
        }
    }

    /// <summary>
    /// 文档内容
    /// </summary>
    public string DocumentContent
    {
        get => _documentContent;
        set => this.RaiseAndSetIfChanged(ref _documentContent, value);
    }

    /// <summary>
    /// 刷新文档列表命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    /// <summary>
    /// 复制文档内容命令
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyContentCommand { get; }

    /// <summary>
    /// 初始化关于页面 ViewModel
    /// </summary>
    /// <param name="loggingService">日志服务</param>
    /// <param name="clipboardService">剪贴板服务</param>
    public AboutViewModel(ILoggingService loggingService, IClipboardService clipboardService)
    {
        _loggingService = loggingService;
        _clipboardService = clipboardService;

        Title = "关于";

        // 初始化命令
        RefreshCommand = ReactiveCommand.CreateFromTask(LoadDocumentsAsync);
        CopyContentCommand = ReactiveCommand.CreateFromTask(CopyContentToClipboardAsync);

        // 加载文档列表
        _ = LoadDocumentsAsync();
    }

    /// <summary>
    /// 加载文档列表
    /// </summary>
    private async Task LoadDocumentsAsync()
    {
        IsBusy = true;

        try
        {
            Documents.Clear();
            DocumentContent = string.Empty;

            // 获取程序根目录
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = appDirectory;
            var docsDirectory = Path.Combine(appDirectory, "docs");

            // 如果 docs 目录不存在于发布目录，尝试查找源代码目录
            if (!Directory.Exists(docsDirectory))
            {
                // 尝试向上查找项目根目录
                var currentDir = new DirectoryInfo(appDirectory);
                while (currentDir != null && currentDir.Name != "C-ProjectToVsCodeDebugJson")
                {
                    currentDir = currentDir.Parent;
                }

                if (currentDir != null)
                {
                    projectRoot = currentDir.FullName;
                    docsDirectory = Path.Combine(projectRoot, "docs");
                }
            }

            // 首先添加根目录的 README.md
            var readmePath = Path.Combine(projectRoot, "README.md");
            if (File.Exists(readmePath))
            {
                Documents.Add(new DocumentItem
                {
                    FileName = "README.md",
                    DisplayName = "📖 项目说明",
                    FilePath = readmePath
                });
            }

            // 如果 docs 目录存在，读取所有 md 文件
            if (Directory.Exists(docsDirectory))
            {
                // 读取所有 md 文件
                var mdFiles = Directory.GetFiles(docsDirectory, "*.md", SearchOption.AllDirectories);

                foreach (var filePath in mdFiles.OrderBy(f => f))
                {
                    var fileName = Path.GetFileName(filePath);
                    var displayName = GetDisplayName(fileName);

                    Documents.Add(new DocumentItem
                    {
                        FileName = fileName,
                        DisplayName = displayName,
                        FilePath = filePath
                    });
                }
            }

            _loggingService.Log($"已加载 {Documents.Count} 个文档", LogLevel.Info);

            // 默认选中第一个文档
            if (Documents.Count > 0)
            {
                SelectedDocument = Documents[0];
            }
            else
            {
                DocumentContent = "# 欢迎使用 VsCode 调试配置生成工具\n\n" +
                                "版本: 3.0.0\n" +
                                "框架: .NET 8.0 + Avalonia 11.x\n\n" +
                                "© 2026 VsCodeDebugGen";
            }
        }
        catch (Exception ex)
        {
            _loggingService.Log($"加载文档失败: {ex.Message}", LogLevel.Error);
            DocumentContent = $"# 加载失败\n\n{ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 加载文档内容
    /// </summary>
    /// <param name="document">文档项</param>
    private async Task LoadDocumentContentAsync(DocumentItem document)
    {
        try
        {
            if (File.Exists(document.FilePath))
            {
                DocumentContent = await File.ReadAllTextAsync(document.FilePath);
                _loggingService.Log($"已加载文档: {document.DisplayName}", LogLevel.Info);
            }
            else
            {
                DocumentContent = $"# 文件不存在\n\n无法找到文件: {document.FileName}";
                _loggingService.Log($"文档文件不存在: {document.FilePath}", LogLevel.Warning);
            }
        }
        catch (Exception ex)
        {
            _loggingService.Log($"读取文档失败: {ex.Message}", LogLevel.Error);
            DocumentContent = $"# 读取失败\n\n{ex.Message}";
        }
    }

    /// <summary>
    /// 获取友好的显示名称
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>显示名称</returns>
    private string GetDisplayName(string fileName)
    {
        var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);

        // 常见文档名称映射
        return nameWithoutExt switch
        {
            "QUICK_START" => "快速开始",
            "PROJECT_SUMMARY" => "项目总结",
            "ENVIRONMENT_SETUP" => "环境设置",
            "完成说明" => "完成说明",
            "RELEASE_NOTES_v2.0.0" => "版本说明 v2.0.0",
            "DEPLOYMENT_CHECKLIST" => "部署检查清单",
            "UPGRADE_PLAN_v3.0.0" => "升级计划 v3.0.0",
            "SPEC" => "技术规范",
            "TASK" => "任务列表",
            "TASK_PROGRESS" => "任务进度",
            _ => nameWithoutExt
        };
    }

    /// <summary>
    /// 复制文档内容到剪贴板
    /// </summary>
    private async Task CopyContentToClipboardAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(DocumentContent))
            {
                _loggingService.Log("没有内容可以复制", LogLevel.Warning);
                return;
            }

            await _clipboardService.SetTextAsync(DocumentContent);
            _loggingService.Log("文档内容已复制到剪贴板", LogLevel.Info);
        }
        catch (Exception ex)
        {
            _loggingService.Log($"复制到剪贴板失败: {ex.Message}", LogLevel.Error);
        }
    }
}

/// <summary>
/// 文档项
/// </summary>
public class DocumentItem
{
    /// <summary>
    /// 文件名
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 文件路径
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
}
