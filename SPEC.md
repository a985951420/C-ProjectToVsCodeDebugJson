# VsCode调试配置生成工具 v3.0 - 技术规范文档

## 📋 文档信息
- **版本**: 3.0.0
- **创建日期**: 2026-02-02
- **目标框架**: .NET 8.0
- **UI框架**: Avalonia 11.x
- **架构模式**: MVVM + Clean Architecture

---

## 🏗️ 项目架构

### 整体架构图
```
┌─────────────────────────────────────────────────────┐
│              VsCodeDebugGen.Desktop                 │
│         (Avalonia MVVM Presentation Layer)          │
│  ┌─────────────┐  ┌─────────────┐  ┌────────────┐  │
│  │   Views     │  │ ViewModels  │  │  Services  │  │
│  │  (XAML)     │←→│   (Logic)   │←→│  (UI层)   │  │
│  └─────────────┘  └─────────────┘  └────────────┘  │
└────────────────────────┬────────────────────────────┘
                         │ 依赖注入
                         ↓
┌─────────────────────────────────────────────────────┐
│            VsCodeDebugGen.Core (复用)               │
│              (Business Logic Layer)                 │
│  ┌─────────────┐  ┌─────────────┐  ┌────────────┐  │
│  │ Interfaces  │  │   Models    │  │  Services  │  │
│  │    (抽象)   │  │  (领域模型) │  │  (业务层)  │  │
│  └─────────────┘  └─────────────┘  └────────────┘  │
└────────────────────────┬────────────────────────────┘
                         │
                         ↓
┌─────────────────────────────────────────────────────┐
│      VsCodeDebugGen.Infrastructure (复用)           │
│           (Data Access & External Services)         │
│  ┌──────────────────┐  ┌──────────────────────┐    │
│  │  Configuration   │  │   File System I/O    │    │
│  │   (配置持久化)    │  │   (文件读写)         │    │
│  └──────────────────┘  └──────────────────────┘    │
└─────────────────────────────────────────────────────┘
```

---

## 📁 项目结构规范

### 解决方案结构
```
VsCodeDebugGen.sln (重命名为新的解决方案)
│
├── src/
│   ├── VsCodeDebugGen.Core/              # 核心业务层（现有，复用）
│   ├── VsCodeDebugGen.Infrastructure/    # 基础设施层（现有，复用）
│   ├── VsCodeDebugGen.CLI/               # CLI工具（现有，复用）
│   └── VsCodeDebugGen.Desktop/           # 桌面应用（新建）
│
├── docs/
│   ├── SPEC.md                           # 技术规范（本文档）
│   ├── TASK.md                           # 任务清单
│   ├── UPGRADE_PLAN_v3.0.0.md           # 升级计划
│   └── API.md                            # API文档
│
└── tests/                                # 测试项目（可选）
    └── VsCodeDebugGen.Desktop.Tests/
```

### Desktop 项目详细结构
```
VsCodeDebugGen.Desktop/
│
├── App.axaml                             # 应用主入口 XAML
├── App.axaml.cs                          # 应用主入口代码
├── Program.cs                            # 程序入口点
│
├── ViewModels/                           # 视图模型层
│   ├── Base/
│   │   ├── ViewModelBase.cs             # ViewModel 基类
│   │   └── DialogViewModelBase.cs       # 对话框 ViewModel 基类
│   │
│   ├── MainWindowViewModel.cs           # 主窗口 ViewModel
│   ├── ProjectScanViewModel.cs          # 项目扫描视图模型
│   ├── ConfigurationViewModel.cs        # 配置视图模型
│   ├── LogViewModel.cs                  # 日志视图模型
│   ├── TemplateManagerViewModel.cs      # 模板管理视图模型
│   └── HistoryViewModel.cs              # 历史记录视图模型
│
├── Views/                                # 视图层
│   ├── MainWindow.axaml                 # 主窗口
│   ├── MainWindow.axaml.cs
│   │
│   ├── ProjectScanView.axaml            # 项目扫描视图
│   ├── ProjectScanView.axaml.cs
│   │
│   ├── ConfigurationView.axaml          # 配置视图
│   ├── ConfigurationView.axaml.cs
│   │
│   ├── LogView.axaml                    # 日志视图
│   ├── LogView.axaml.cs
│   │
│   ├── TemplateManagerView.axaml        # 模板管理视图
│   ├── TemplateManagerView.axaml.cs
│   │
│   ├── HistoryView.axaml                # 历史记录视图
│   ├── HistoryView.axaml.cs
│   │
│   └── Dialogs/                         # 对话框
│       ├── PreviewDialog.axaml
│       ├── PreviewDialog.axaml.cs
│       ├── TemplateEditDialog.axaml
│       └── TemplateEditDialog.axaml.cs
│
├── Models/                               # UI 模型层
│   ├── ProjectItemViewModel.cs          # 项目项视图模型
│   ├── LogEntry.cs                      # 日志条目
│   ├── TemplateModel.cs                 # 模板模型
│   ├── HistoryEntry.cs                  # 历史条目
│   ├── PortConfiguration.cs             # 端口配置
│   └── MultiSiteConfiguration.cs        # 多站点配置
│
├── Services/                             # UI 层服务
│   ├── Interfaces/
│   │   ├── IDialogService.cs
│   │   ├── ILoggingService.cs
│   │   ├── ITemplateService.cs
│   │   ├── IHistoryService.cs
│   │   └── IPortConfigurationService.cs
│   │
│   ├── DialogService.cs                 # 对话框服务
│   ├── LoggingService.cs                # 日志服务
│   ├── TemplateService.cs               # 模板服务
│   ├── HistoryService.cs                # 历史服务
│   └── PortConfigurationService.cs      # 端口配置服务
│
├── Converters/                           # 值转换器
│   ├── BoolToVisibilityConverter.cs
│   ├── StatusToColorConverter.cs
│   ├── LogLevelToColorConverter.cs
│   └── ProjectTypeToIconConverter.cs
│
├── Behaviors/                            # 行为
│   └── DragDropBehavior.cs              # 拖拽行为
│
├── Controls/                             # 自定义控件
│   └── ProjectTreeView.axaml            # 项目树形视图控件
│
├── Assets/                               # 资源文件
│   ├── Icons/                           # 图标
│   │   ├── app-icon.ico
│   │   ├── folder.png
│   │   ├── project.png
│   │   └── ...
│   │
│   ├── Styles/                          # 样式
│   │   ├── Colors.axaml                 # 颜色定义
│   │   ├── Buttons.axaml                # 按钮样式
│   │   └── TextBlocks.axaml             # 文本样式
│   │
│   └── Fonts/                           # 字体（可选）
│
├── Helpers/                              # 辅助类
│   ├── DialogHelper.cs
│   ├── FileHelper.cs
│   └── ValidationHelper.cs
│
└── VsCodeDebugGen.Desktop.csproj        # 项目文件
```

---

## 📦 依赖项规范

### VsCodeDebugGen.Desktop.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
    <ApplicationIcon>Assets\Icons\app-icon.ico</ApplicationIcon>
    <ApplicationManifest>app.manifest</ApplicationManifest>
    <AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>
    <AssemblyName>VsCodeDebugGen</AssemblyName>
    <RootNamespace>VsCodeDebugGen.Desktop</RootNamespace>
  </PropertyGroup>

  <ItemGroup>
    <!-- Avalonia 核心包 -->
    <PackageReference Include="Avalonia" Version="11.0.10" />
    <PackageReference Include="Avalonia.Desktop" Version="11.0.10" />

    <!-- Avalonia 主题 -->
    <PackageReference Include="Avalonia.Themes.Fluent" Version="11.0.10" />

    <!-- Avalonia 图标 -->
    <PackageReference Include="Avalonia.Controls.DataGrid" Version="11.0.10" />

    <!-- MVVM 支持 -->
    <PackageReference Include="Avalonia.ReactiveUI" Version="11.0.10" />
    <PackageReference Include="ReactiveUI" Version="19.5.31" />
    <PackageReference Include="ReactiveUI.Fody" Version="19.5.31" />

    <!-- 依赖注入 -->
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />

    <!-- 日志框架 -->
    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="Serilog.Extensions.Logging" Version="8.0.0" />

    <!-- Fody（属性通知） -->
    <PackageReference Include="PropertyChanged.Fody" Version="4.1.0" />
  </ItemGroup>

  <ItemGroup>
    <!-- 引用现有项目 -->
    <ProjectReference Include="..\VsCodeDebugGen.Core\VsCodeDebugGen.Core.csproj" />
    <ProjectReference Include="..\VsCodeDebugGen.Infrastructure\VsCodeDebugGen.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

---

## 🎨 UI 设计规范

### 颜色方案（Fluent Design）
```xml
<!-- Assets/Styles/Colors.axaml -->
<ResourceDictionary>
  <!-- 主题色 -->
  <Color x:Key="PrimaryColor">#0078D4</Color>
  <Color x:Key="PrimaryHoverColor">#106EBE</Color>
  <Color x:Key="PrimaryPressedColor">#005A9E</Color>

  <!-- 状态色 -->
  <Color x:Key="SuccessColor">#107C10</Color>
  <Color x:Key="WarningColor">#FF8C00</Color>
  <Color x:Key="ErrorColor">#D13438</Color>
  <Color x:Key="InfoColor">#0078D4</Color>

  <!-- 背景色 -->
  <Color x:Key="BackgroundColor">#F3F3F3</Color>
  <Color x:Key="SurfaceColor">#FFFFFF</Color>

  <!-- 文本色 -->
  <Color x:Key="TextPrimaryColor">#1F1F1F</Color>
  <Color x:Key="TextSecondaryColor">#605E5C</Color>
  <Color x:Key="TextDisabledColor">#A19F9D</Color>

  <!-- 边框色 -->
  <Color x:Key="BorderColor">#EDEBE9</Color>
  <Color x:Key="DividerColor">#E1DFDD</Color>
</ResourceDictionary>
```

### 字体规范
- **标题**: Segoe UI Semibold, 20px
- **副标题**: Segoe UI Semibold, 16px
- **正文**: Segoe UI, 14px
- **小字**: Segoe UI, 12px
- **代码**: Cascadia Code / Consolas, 13px

### 间距规范
- **Extra Small**: 4px
- **Small**: 8px
- **Medium**: 16px
- **Large**: 24px
- **Extra Large**: 32px

### 图标规范
- 使用 Material Design Icons 或 Fluent UI Icons
- 标准尺寸: 16x16, 24x24, 32x32, 48x48
- 格式: SVG（优先）或 PNG

---

## 🔧 代码规范

### C# 命名规范
```csharp
// 命名空间
namespace VsCodeDebugGen.Desktop.ViewModels

// 类名 - PascalCase
public class ProjectScanViewModel : ViewModelBase

// 接口 - I + PascalCase
public interface ILoggingService

// 属性 - PascalCase
public string ProjectName { get; set; }

// 字段（私有）- _camelCase
private readonly ILoggingService _loggingService;

// 方法 - PascalCase
public async Task ScanProjectsAsync(string path)

// 参数 - camelCase
public void SetConfiguration(string configPath, bool isEnabled)

// 常量 - PascalCase
private const string DefaultOutputPath = ".vscode";

// 枚举 - PascalCase
public enum LogLevel { Info, Warning, Error }
```

### XAML 命名规范
```xml
<!-- 文件名: PascalCase.axaml -->
<!-- MainWindow.axaml, ProjectScanView.axaml -->

<!-- 控件命名: PascalCase -->
<Button x:Name="ScanButton" />
<TextBox x:Name="SearchPathTextBox" />

<!-- 资源键: PascalCase -->
<SolidColorBrush x:Key="PrimaryBrush" Color="#0078D4" />

<!-- 样式: Target + Style -->
<Style x:Key="PrimaryButtonStyle" TargetType="Button">
```

### ViewModelBase 规范
```csharp
using ReactiveUI;
using System.Reactive;

namespace VsCodeDebugGen.Desktop.ViewModels.Base
{
    public abstract class ViewModelBase : ReactiveObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }

        private string _title = string.Empty;
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
}
```

### 异步方法规范
```csharp
// ✅ 正确：使用 Async 后缀，返回 Task
public async Task<List<ProjectInfo>> ScanProjectsAsync(string path)
{
    await Task.Run(() => { /* ... */ });
}

// ✅ 正确：使用 CancellationToken
public async Task GenerateConfigAsync(
    List<ProjectInfo> projects,
    CancellationToken cancellationToken = default)
{
    cancellationToken.ThrowIfCancellationRequested();
    // ...
}

// ❌ 错误：async void（除事件处理器外）
public async void BadMethod() { }
```

---

## 📐 MVVM 模式规范

### View 与 ViewModel 绑定
```xml
<!-- MainWindow.axaml -->
<Window xmlns="https://github.com/avaloniaui"
        xmlns:vm="using:VsCodeDebugGen.Desktop.ViewModels"
        x:DataType="vm:MainWindowViewModel">

    <!-- 使用编译时绑定 -->
    <TextBlock Text="{Binding Title}" />

    <!-- 命令绑定 -->
    <Button Content="扫描" Command="{Binding ScanCommand}" />

    <!-- 双向绑定 -->
    <TextBox Text="{Binding SearchPath, Mode=TwoWay}" />
</Window>
```

### ViewModel 实现规范
```csharp
public class ProjectScanViewModel : ViewModelBase
{
    private readonly IProjectFinder _projectFinder;
    private readonly ILoggingService _loggingService;

    // 属性
    private string _searchPath = string.Empty;
    public string SearchPath
    {
        get => _searchPath;
        set => this.RaiseAndSetIfChanged(ref _searchPath, value);
    }

    private ObservableCollection<ProjectItemViewModel> _projects = new();
    public ObservableCollection<ProjectItemViewModel> Projects
    {
        get => _projects;
        set => this.RaiseAndSetIfChanged(ref _projects, value);
    }

    // 命令
    public ReactiveCommand<Unit, Unit> ScanCommand { get; }
    public ReactiveCommand<string, Unit> BrowseCommand { get; }

    // 构造函数（依赖注入）
    public ProjectScanViewModel(
        IProjectFinder projectFinder,
        ILoggingService loggingService)
    {
        _projectFinder = projectFinder;
        _loggingService = loggingService;

        // 初始化命令
        ScanCommand = ReactiveCommand.CreateFromTask(ScanProjectsAsync);
        BrowseCommand = ReactiveCommand.CreateFromTask<string>(BrowseFolderAsync);
    }

    // 业务逻辑
    private async Task ScanProjectsAsync()
    {
        IsBusy = true;
        try
        {
            _loggingService.Log("开始扫描项目...");
            var projectFiles = await _projectFinder.FindProjectsAsync(SearchPath);

            Projects.Clear();
            foreach (var project in projectFiles)
            {
                Projects.Add(new ProjectItemViewModel(project));
            }

            _loggingService.Log($"找到 {Projects.Count} 个项目");
        }
        catch (Exception ex)
        {
            _loggingService.Log($"扫描失败: {ex.Message}", LogLevel.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
```

---

## 🔌 依赖注入规范

### 服务注册（App.axaml.cs）
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // 配置日志
        ConfigureLogging(services);

        // 注册 Core 层服务（现有）
        services.AddSingleton<IProjectFinder, ProjectFinderService>();
        services.AddSingleton<IProjectParser, ProjectParserService>();
        services.AddSingleton<IConfigGenerator, ConfigGeneratorService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // 注册 UI 层服务（新增）
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ITemplateService, TemplateService>();
        services.AddSingleton<IHistoryService, HistoryService>();
        services.AddSingleton<IPortConfigurationService, PortConfigurationService>();

        // 注册 ViewModels（Transient - 每次请求创建新实例）
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ProjectScanViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LogViewModel>();
        services.AddTransient<TemplateManagerViewModel>();
        services.AddTransient<HistoryViewModel>();

        Services = services.BuildServiceProvider();
    }

    private void ConfigureLogging(IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(
                path: "logs/vscodegen-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();

        services.AddLogging(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog();
        });
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

---

## 📝 日志规范

### 日志级别
- **Debug**: 详细的调试信息（开发时使用）
- **Info**: 一般信息（正常操作）
- **Warning**: 警告信息（潜在问题）
- **Error**: 错误信息（操作失败）
- **Critical**: 严重错误（应用崩溃）

### 日志格式
```
[2026-02-02 14:32:15] [INFO] 开始扫描项目...
[2026-02-02 14:32:15] [INFO] 找到项目: WebApp.csproj
[2026-02-02 14:32:16] [SUCCESS] 扫描完成，共找到 4 个项目
[2026-02-02 14:32:20] [WARNING] 项目 Tests.csproj 未找到输出路径
[2026-02-02 14:32:25] [ERROR] 生成配置失败: 文件访问被拒绝
```

### 日志使用示例
```csharp
_loggingService.Log("开始扫描项目...", LogLevel.Info);
_loggingService.Log($"找到项目: {projectName}", LogLevel.Info);
_loggingService.Log("扫描完成", LogLevel.Success);
_loggingService.Log($"警告: {warningMessage}", LogLevel.Warning);
_loggingService.Log($"错误: {ex.Message}", LogLevel.Error);
```

---

## 🧪 测试规范

### 单元测试命名
```csharp
// 格式: MethodName_Scenario_ExpectedBehavior
[Fact]
public void ScanProjects_ValidPath_ReturnsProjectList()

[Fact]
public async Task GenerateConfig_EmptyProjectList_ThrowsException()

[Theory]
[InlineData("")]
[InlineData(null)]
public void ValidatePath_InvalidInput_ReturnsFalse(string path)
```

---

## 🔒 安全规范

### 文件访问
- 始终验证文件路径
- 使用 try-catch 处理文件 I/O 异常
- 避免硬编码路径

### 用户输入验证
```csharp
public bool ValidateSearchPath(string path)
{
    if (string.IsNullOrWhiteSpace(path))
        return false;

    if (!Directory.Exists(path))
        return false;

    // 检查路径安全性
    var fullPath = Path.GetFullPath(path);
    if (fullPath.Contains(".."))  // 防止路径遍历
        return false;

    return true;
}
```

---

## 📊 性能规范

### 异步操作
- 所有 I/O 操作必须异步
- 使用 `ConfigureAwait(false)` 在库代码中
- 避免在 UI 线程上执行长时间操作

### 大数据处理
```csharp
// ✅ 使用流式处理大文件
public async IAsyncEnumerable<ProjectInfo> ScanProjectsStreamAsync(string path)
{
    await foreach (var file in EnumerateProjectFilesAsync(path))
    {
        yield return await ParseProjectAsync(file);
    }
}

// ✅ 使用虚拟化处理大列表
<DataGrid ItemsSource="{Binding Projects}"
          VirtualizingPanel.IsVirtualizing="True" />
```

### 内存管理
- 及时释放大对象
- 使用 `using` 语句管理资源
- 避免内存泄漏（取消订阅事件）

---

## 🌍 国际化规范（可选）

### 资源文件结构
```
Resources/
├── Strings.resx                 # 默认（中文）
├── Strings.en-US.resx          # 英语
└── Strings.zh-CN.resx          # 简体中文
```

### 使用示例
```csharp
// 代码中使用
var message = Resources.Strings.ScanCompleted;

// XAML 中使用
<TextBlock Text="{x:Static resources:Strings.ScanCompleted}" />
```

---

## 📦 打包和部署规范

### 输出配置
```xml
<PropertyGroup>
  <!-- 单文件发布 -->
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>win-x64</RuntimeIdentifier>

  <!-- 裁剪未使用代码 -->
  <PublishTrimmed>true</PublishTrimmed>
  <TrimMode>link</TrimMode>

  <!-- 压缩 -->
  <EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
</PropertyGroup>
```

### 发布命令
```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained

# macOS x64
dotnet publish -c Release -r osx-x64 --self-contained

# macOS ARM64
dotnet publish -c Release -r osx-arm64 --self-contained
```

---

## 📝 文档规范

### 代码注释
```csharp
/// <summary>
/// 扫描指定路径下的所有项目文件
/// </summary>
/// <param name="searchPath">要扫描的根目录路径</param>
/// <param name="recursive">是否递归扫描子目录</param>
/// <returns>找到的项目信息列表</returns>
/// <exception cref="DirectoryNotFoundException">目录不存在时抛出</exception>
public async Task<List<ProjectInfo>> ScanProjectsAsync(
    string searchPath,
    bool recursive = true)
{
    // 实现代码
}
```

### XML 文档生成
```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

---

## 🔧 开发工具

### 推荐 IDE
- **Visual Studio 2022** (17.8+)
- **Visual Studio Code** + Avalonia 扩展
- **JetBrains Rider** (2023.3+)

### 必需的扩展/工具
- Avalonia for Visual Studio
- Avalonia XAML Intellisense
- .NET 8.0 SDK
- Git

### 有用的工具
- AvaloniaUI Previewer（实时预览）
- XAML Styler（格式化 XAML）
- ResXManager（管理资源文件）

---

## 🎯 验收标准

### 功能验收
- ✅ 所有核心功能正常工作
- ✅ UI 响应流畅（无卡顿）
- ✅ 拖拽功能正常
- ✅ 端口配置正确生成
- ✅ 日志实时显示
- ✅ 模板保存和加载正常
- ✅ 历史记录功能正常

### 性能验收
- ✅ 扫描 100 个项目 < 5 秒
- ✅ 生成配置文件 < 2 秒
- ✅ UI 响应时间 < 100ms
- ✅ 内存占用 < 200MB

### 跨平台验收
- ✅ Windows 10/11 正常运行
- ✅ Linux (Ubuntu 22.04+) 正常运行
- ✅ macOS (12.0+) 正常运行

### 代码质量验收
- ✅ 无编译警告
- ✅ 无明显的代码异味
- ✅ 遵循命名规范
- ✅ 关键方法有注释

---

## 📚 参考资料

- [Avalonia 官方文档](https://docs.avaloniaui.net/)
- [ReactiveUI 文档](https://www.reactiveui.net/)
- [.NET 8.0 文档](https://learn.microsoft.com/dotnet/)
- [Fluent Design System](https://www.microsoft.com/design/fluent/)
- [Material Design Icons](https://materialdesignicons.com/)

---

**文档版本**: 1.0
**最后更新**: 2026-02-02
**维护者**: Development Team
