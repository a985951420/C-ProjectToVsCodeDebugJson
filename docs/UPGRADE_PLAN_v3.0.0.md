# VsCode调试文件生成工具 - 3.0 版本升级计划

## 版本信息
- **目标版本**: 3.0.0
- **当前版本**: 2.0.0
- **计划日期**: 2026-02-02
- **项目类型**: 跨平台桌面应用 + CLI工具

---

## 📋 升级目标

将现有的命令行工具升级为**功能丰富的跨平台桌面应用**，同时保留CLI功能，提供：
- ✨ 美观的图形化用户界面
- 🎯 拖拽式文件夹操作
- 🔍 智能项目扫描和勾选
- 🌐 Web应用端口配置
- 📝 详细的生成日志
- 🎨 丰富的扩展功能

---

## 🛠️ 技术栈选择

### UI框架：Avalonia 11.x
**选择理由**:
- ✅ 跨平台支持（Windows、Linux、macOS）
- ✅ 现代化XAML设计，类似WPF
- ✅ 丰富的控件库和主题系统
- ✅ 良好的性能和渲染效果
- ✅ 活跃的社区和文档支持
- ✅ 支持MVVM架构模式

### 核心依赖
```xml
<ItemGroup>
  <!-- Avalonia UI框架 -->
  <PackageReference Include="Avalonia" Version="11.0.*" />
  <PackageReference Include="Avalonia.Desktop" Version="11.0.*" />
  <PackageReference Include="Avalonia.Themes.Fluent" Version="11.0.*" />
  <PackageReference Include="Avalonia.ReactiveUI" Version="11.0.*" />

  <!-- MVVM支持 -->
  <PackageReference Include="ReactiveUI" Version="19.*" />
  <PackageReference Include="ReactiveUI.Fody" Version="19.*" />

  <!-- 日志框架 -->
  <PackageReference Include="Serilog" Version="3.*" />
  <PackageReference Include="Serilog.Sinks.File" Version="5.*" />

  <!-- JSON处理（已有） -->
  <!-- System.Text.Json -->
</ItemGroup>
```

---

## 🏗️ 项目结构设计

### 新的解决方案结构
```
VsCodeDebugGen/
├── VsCodeDebugGen.Core/          # 现有核心库（复用）
│   ├── Interfaces/
│   ├── Models/
│   └── Services/
│
├── VsCodeDebugGen.Infrastructure/ # 现有基础设施（复用）
│   └── Configuration/
│
├── VsCodeDebugGen.CLI/           # 现有CLI工具（复用）
│   ├── Commands/
│   └── UI/
│
├── VsCodeDebugGen.Desktop/       # 🆕 新增桌面应用
│   ├── ViewModels/               # MVVM - ViewModel层
│   │   ├── MainWindowViewModel.cs
│   │   ├── ProjectScanViewModel.cs
│   │   ├── ConfigurationViewModel.cs
│   │   ├── LogViewModel.cs
│   │   └── TemplateManagerViewModel.cs
│   │
│   ├── Views/                    # MVVM - View层
│   │   ├── MainWindow.axaml
│   │   ├── ProjectScanView.axaml
│   │   ├── ConfigurationView.axaml
│   │   ├── LogView.axaml
│   │   └── TemplateManagerView.axaml
│   │
│   ├── Models/                   # UI专用模型
│   │   ├── ProjectItemViewModel.cs
│   │   ├── LogEntryModel.cs
│   │   ├── TemplateModel.cs
│   │   └── HistoryEntryModel.cs
│   │
│   ├── Services/                 # UI层服务
│   │   ├── DialogService.cs
│   │   ├── LoggingService.cs
│   │   ├── TemplateService.cs
│   │   └── HistoryService.cs
│   │
│   ├── Converters/               # 值转换器
│   │   ├── BoolToVisibilityConverter.cs
│   │   └── StatusToColorConverter.cs
│   │
│   ├── Assets/                   # 资源文件
│   │   ├── Icons/
│   │   └── Styles/
│   │
│   ├── App.axaml                 # 应用主入口
│   ├── App.axaml.cs
│   └── Program.cs
│
└── VsCodeDebugGen.Shared/        # 🆕 共享模型和接口
    ├── DTOs/
    └── Enums/
```

---

## 🎨 UI界面设计

### 主窗口布局（MainWindow）

```
┌─────────────────────────────────────────────────────────────┐
│  VsCode调试配置生成工具 v3.0                    [—] [□] [×] │
├─────────────────────────────────────────────────────────────┤
│  📁 项目扫描  │  ⚙️ 配置  │  📝 日志  │  📋 模板  │  📚 历史  │
├───────────────┴─────────────────────────────────────────────┤
│                                                               │
│  主内容区域 (根据选中的Tab显示不同的View)                    │
│                                                               │
│                                                               │
│                                                               │
│                                                               │
│                                                               │
├───────────────────────────────────────────────────────────┤
│  状态栏: 就绪 | 找到 0 个项目 | 选中 0 个          v3.0.0   │
└───────────────────────────────────────────────────────────┘
```

### 1️⃣ 项目扫描视图（ProjectScanView）

```
┌─────────────────────────────────────────────────────────────┐
│  📂 选择项目文件夹                                            │
│  ┌─────────────────────────────────────────┐                │
│  │  D:\Projects\MyWorkspace                 │ [浏览...] │
│  └─────────────────────────────────────────┘  [扫描]   │
│  💡 提示: 可以直接拖拽文件夹到此处                           │
│                                                               │
│  ✓ 递归扫描子目录    🔍 [快速搜索...]                       │
│                                                               │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ ☑ 全选  ⬜ 反选  📁 展开全部  📂 折叠全部               ││
│  ├─────────────────────────────────────────────────────────┤│
│  │ ☑ 📁 MyWorkspace                                        ││
│  │   ☑ 📄 WebApp.csproj        [Web应用]  net8.0         ││
│  │   ☑ 📄 WebAPI.csproj        [Web应用]  net8.0         ││
│  │   ⬜ 📄 Tests.csproj         [类库]     net8.0         ││
│  │   ☑ 📄 ConsoleApp.csproj    [控制台]   net8.0         ││
│  │                                                          ││
│  │ 找到 4 个项目 | 选中 3 个                                ││
│  └─────────────────────────────────────────────────────────┘│
│                                                               │
│  📤 输出路径: D:\Projects\MyWorkspace\.vscode  [浏览...] │
│                                                               │
│  [← 上一步]              [生成配置 →]             [取消]   │
└───────────────────────────────────────────────────────────┘
```

**功能特性**:
- 🎯 支持拖拽文件夹
- 🔍 实时搜索过滤
- ☑️ 批量勾选/取消
- 📁 树形结构显示
- 🏷️ 项目类型标签（Web应用、控制台、类库）
- 🎨 不同项目类型用不同颜色/图标区分

### 2️⃣ 配置视图（ConfigurationView）

```
┌─────────────────────────────────────────────────────────────┐
│  ⚙️ 调试配置选项                                             │
│                                                               │
│  ┌─ 通用设置 ──────────────────────────────────────────┐   │
│  │ 控制台类型:  ◉ internalConsole  ○ integratedTerminal  │  │
│  │ 入口处停止:  ☐ 启用                                    │  │
│  │ 环境变量:    ASPNETCORE_ENVIRONMENT = Development      │  │
│  └───────────────────────────────────────────────────────┘   │
│                                                               │
│  ┌─ 🌐 Web应用端口配置 ─────────────────────────────────┐   │
│  │ ☑ 自动配置Web应用端口                                  │  │
│  │                                                         │  │
│  │ 📋 端口配置列表:                                       │  │
│  │ ┌─────────────────────────────────────────────────┐  │  │
│  │ │ 项目名称          HTTP端口    HTTPS端口         │  │  │
│  │ ├─────────────────────────────────────────────────┤  │  │
│  │ │ WebApp.csproj     5000        5001       [编辑]│  │  │
│  │ │ WebAPI.csproj     5100        5101       [编辑]│  │  │
│  │ └─────────────────────────────────────────────────┘  │  │
│  │                                                         │  │
│  │ [+ 添加站点配置]                                       │  │
│  └───────────────────────────────────────────────────────┘   │
│                                                               │
│  ┌─ 📊 多站点配置 ───────────────────────────────────────┐   │
│  │ ☑ 为同一项目生成多个调试配置                           │  │
│  │                                                         │  │
│  │ WebApp.csproj:                                         │  │
│  │   • Development (端口 5000/5001)                      │  │
│  │   • Staging (端口 5002/5003)     [删除]              │  │
│  │   [+ 添加配置]                                         │  │
│  └───────────────────────────────────────────────────────┘   │
│                                                               │
│  [← 上一步]      [生成配置]      [预览配置 →]      [取消]  │
└───────────────────────────────────────────────────────────┘
```

**功能特性**:
- 🌐 Web应用端口自动检测和配置
- 🔢 支持HTTP/HTTPS端口分别配置
- 🎯 多站点配置（Development、Staging、Production等）
- 👁️ 配置预览功能
- 💾 配置模板保存和加载

### 3️⃣ 日志视图（LogView）

```
┌─────────────────────────────────────────────────────────────┐
│  📝 生成日志                                                  │
│                                                               │
│  🔍 [过滤日志...]   📊 ◉ 全部  ○ 信息  ○ 警告  ○ 错误     │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ [14:32:15] ✓ 开始扫描项目文件...                        ││
│  │ [14:32:15] ℹ 找到项目: WebApp.csproj                    ││
│  │ [14:32:15] ℹ 找到项目: WebAPI.csproj                    ││
│  │ [14:32:16] ✓ 扫描完成，共找到 4 个项目                  ││
│  │ [14:32:20] ✓ 开始生成配置文件...                        ││
│  │ [14:32:20] ℹ 生成 launch.json                          ││
│  │ [14:32:20] ℹ 生成 tasks.json                           ││
│  │ [14:32:21] ✓ 配置文件生成成功！                         ││
│  │ [14:32:21] ✓ 输出路径: D:\Projects\.vscode              ││
│  │                                                          ││
│  │ ✓ 生成完成！共处理 3 个项目，用时 6 秒                   ││
│  └─────────────────────────────────────────────────────────┘│
│                                                               │
│  [清空日志]  [导出日志]  [复制]                          [×]│
└───────────────────────────────────────────────────────────┘
```

**功能特性**:
- 📝 实时显示生成过程
- 🎨 不同级别日志用不同颜色显示
- 🔍 日志过滤和搜索
- 💾 日志导出功能
- ⏱️ 显示时间戳和耗时统计

### 4️⃣ 模板管理视图（TemplateManagerView）

```
┌─────────────────────────────────────────────────────────────┐
│  📋 配置模板管理                                              │
│                                                               │
│  我的模板:                              [+ 新建模板]         │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ 📄 默认Web应用配置                      [应用] [编辑] [×]││
│  │    包含: Web端口配置, HTTPS支持                          ││
│  │    修改时间: 2026-02-01 10:30                            ││
│  │                                                          ││
│  │ 📄 微服务配置                          [应用] [编辑] [×]││
│  │    包含: 多站点, Docker支持                              ││
│  │    修改时间: 2026-01-28 15:20                            ││
│  │                                                          ││
│  │ 📄 控制台应用配置                      [应用] [编辑] [×]││
│  │    包含: 基本调试配置                                    ││
│  │    修改时间: 2026-01-25 09:15                            ││
│  └─────────────────────────────────────────────────────────┘│
│                                                               │
│  [导入模板]  [导出模板]                                      │
└───────────────────────────────────────────────────────────┘
```

**功能特性**:
- 💾 保存常用配置为模板
- 📥 导入/导出模板
- ✏️ 编辑和删除模板
- 🎯 快速应用模板

### 5️⃣ 历史记录视图（HistoryView）

```
┌─────────────────────────────────────────────────────────────┐
│  📚 生成历史                                                  │
│                                                               │
│  🔍 [搜索历史...]                          [清空历史]        │
│  ┌─────────────────────────────────────────────────────────┐│
│  │ 📅 2026-02-02 14:32                            [重新生成]││
│  │    路径: D:\Projects\MyWorkspace                         ││
│  │    项目: WebApp, WebAPI, ConsoleApp (3个)               ││
│  │    状态: ✓ 成功                                          ││
│  │                                                          ││
│  │ 📅 2026-02-01 10:15                            [重新生成]││
│  │    路径: D:\Projects\OldProject                          ││
│  │    项目: MainApp, Services (2个)                         ││
│  │    状态: ✓ 成功                                          ││
│  │                                                          ││
│  │ 📅 2026-01-31 16:45                            [重新生成]││
│  │    路径: D:\Work\ClientProject                           ││
│  │    项目: API, Web, Admin (3个)                           ││
│  │    状态: ⚠ 部分成功 (1个警告)                            ││
│  └─────────────────────────────────────────────────────────┘│
│                                                               │
│  显示最近 50 条记录                                           │
└───────────────────────────────────────────────────────────┘
```

**功能特性**:
- 📜 记录所有生成操作
- 🔄 快速重新生成
- 🔍 历史搜索
- 📊 生成统计

### 6️⃣ 配置预览对话框

```
┌─────────────────────────────────────────────────────────────┐
│  👁️ 配置预览                                      [—] [×]   │
├─────────────────────────────────────────────────────────────┤
│  📑 launch.json  │  📑 tasks.json                           │
├─────────────────────────────────────────────────────────────┤
│  {                                                            │
│    "version": "0.2.0",                                        │
│    "configurations": [                                        │
│      {                                                        │
│        "name": "Launch WebApp",                              │
│        "type": "coreclr",                                     │
│        "request": "launch",                                   │
│        "preLaunchTask": "build-WebApp",                      │
│        "program": "${workspaceFolder}/WebApp/bin/...",       │
│        "args": [],                                            │
│        "cwd": "${workspaceFolder}/WebApp",                   │
│        "env": {                                               │
│          "ASPNETCORE_ENVIRONMENT": "Development",            │
│          "ASPNETCORE_URLS": "http://localhost:5000;..."      │
│        }                                                      │
│      }                                                        │
│    ]                                                          │
│  }                                                            │
│                                                               │
├─────────────────────────────────────────────────────────────┤
│           [复制到剪贴板]  [确认生成]  [返回修改]            │
└───────────────────────────────────────────────────────────┘
```

---

## 🔧 核心功能实现

### 1. 文件夹拖拽功能
```csharp
// View: MainWindow.axaml
<Border AllowDrop="True"
        DragDrop.DragEnter="OnDragEnter"
        DragDrop.DragOver="OnDragOver"
        DragDrop.Drop="OnDrop">
    <!-- 内容 -->
</Border>

// ViewModel: ProjectScanViewModel.cs
public async Task HandleDropAsync(string[] paths)
{
    var directory = paths.FirstOrDefault(p => Directory.Exists(p));
    if (directory != null)
    {
        await ScanProjectsAsync(directory);
    }
}
```

### 2. 项目扫描和勾选
```csharp
public class ProjectItemViewModel : ReactiveObject
{
    [Reactive] public bool IsSelected { get; set; }
    [Reactive] public string ProjectPath { get; set; }
    [Reactive] public string ProjectName { get; set; }
    [Reactive] public string ProjectType { get; set; }  // "Web", "Console", "Library"
    [Reactive] public string TargetFramework { get; set; }
    [Reactive] public ObservableCollection<ProjectItemViewModel> Children { get; set; }
}

public class ProjectScanViewModel : ReactiveObject
{
    public ObservableCollection<ProjectItemViewModel> Projects { get; set; }

    public async Task ScanProjectsAsync(string searchPath)
    {
        LoggingService.Log("开始扫描项目文件...");

        var projectFiles = await _projectFinder.FindProjectsAsync(searchPath);

        foreach (var projectPath in projectFiles)
        {
            var projectInfo = await _projectParser.ParseAsync(projectPath, searchPath);
            Projects.Add(new ProjectItemViewModel
            {
                IsSelected = true,  // 默认全选
                ProjectPath = projectPath,
                ProjectName = projectInfo.AssemblyName,
                ProjectType = GetProjectType(projectInfo.OutputType),
                TargetFramework = projectInfo.TargetFramework
            });

            LoggingService.Log($"找到项目: {projectInfo.AssemblyName}");
        }

        LoggingService.Log($"扫描完成，共找到 {Projects.Count} 个项目");
    }
}
```

### 3. Web应用端口配置
```csharp
// Models/PortConfiguration.cs
public class PortConfiguration
{
    public string ProjectName { get; set; }
    public int HttpPort { get; set; } = 5000;
    public int HttpsPort { get; set; } = 5001;
    public string Environment { get; set; } = "Development";
}

// Models/MultiSiteConfiguration.cs
public class MultiSiteConfiguration
{
    public string ProjectName { get; set; }
    public List<SiteConfig> Sites { get; set; } = new();
}

public class SiteConfig
{
    public string Name { get; set; }  // "Development", "Staging"
    public int HttpPort { get; set; }
    public int HttpsPort { get; set; }
    public Dictionary<string, string> EnvironmentVariables { get; set; }
}

// Services/PortConfigurationService.cs
public class PortConfigurationService
{
    public PortConfiguration AutoDetectPorts(ProjectInfo projectInfo)
    {
        // 从launchSettings.json读取现有端口配置
        var launchSettingsPath = Path.Combine(
            Path.GetDirectoryName(projectInfo.ProjectPath),
            "Properties",
            "launchSettings.json"
        );

        if (File.Exists(launchSettingsPath))
        {
            // 解析并返回端口配置
        }

        // 否则使用默认端口
        return new PortConfiguration
        {
            ProjectName = projectInfo.AssemblyName,
            HttpPort = GetNextAvailablePort(5000),
            HttpsPort = GetNextAvailablePort(5001)
        };
    }

    private int GetNextAvailablePort(int startPort)
    {
        // 检查端口可用性逻辑
    }
}
```

### 4. 生成日志输出
```csharp
// Services/LoggingService.cs
public class LoggingService : ILoggingService
{
    public ObservableCollection<LogEntry> Logs { get; } = new();
    private readonly ILogger _logger;  // Serilog

    public void Log(string message, LogLevel level = LogLevel.Info)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.Now,
            Level = level,
            Message = message
        };

        // 添加到UI集合
        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            Logs.Add(entry);
        });

        // 同时写入文件
        _logger.Log(GetSerilogLevel(level), message);
    }
}

// Models/LogEntry.cs
public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; }
    public string Icon => Level switch
    {
        LogLevel.Info => "ℹ",
        LogLevel.Success => "✓",
        LogLevel.Warning => "⚠",
        LogLevel.Error => "✗",
        _ => "•"
    };
}

public enum LogLevel
{
    Info,
    Success,
    Warning,
    Error
}
```

### 5. 配置预览功能
```csharp
// ViewModels/ConfigurationViewModel.cs
public class ConfigurationViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> PreviewCommand { get; }

    private async Task PreviewConfigAsync()
    {
        LoggingService.Log("正在生成预览...");

        var selectedProjects = GetSelectedProjects();

        // 使用现有的ConfigGeneratorService生成配置
        var launchConfig = _configGenerator.GenerateLaunchConfig(
            selectedProjects,
            _portConfigurations
        );

        var tasksConfig = _configGenerator.GenerateTasksConfig(selectedProjects);

        // 序列化为JSON
        var launchJson = JsonSerializer.Serialize(launchConfig, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var tasksJson = JsonSerializer.Serialize(tasksConfig, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        // 显示预览对话框
        var dialog = new PreviewDialog
        {
            LaunchJson = launchJson,
            TasksJson = tasksJson
        };

        var result = await dialog.ShowDialog<bool>(MainWindow);

        if (result)
        {
            await GenerateConfigAsync();
        }
    }
}
```

### 6. 模板管理功能
```csharp
// Services/TemplateService.cs
public class TemplateService : ITemplateService
{
    private const string TemplatesDirectory = "Templates";

    public class ConfigTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public AppConfiguration Configuration { get; set; }
        public List<PortConfiguration> PortConfigurations { get; set; }
        public List<MultiSiteConfiguration> MultiSiteConfigurations { get; set; }
    }

    public async Task SaveTemplateAsync(string name, ConfigTemplate template)
    {
        var templatePath = Path.Combine(TemplatesDirectory, $"{name}.template.json");
        var json = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(templatePath, json);
        LoggingService.Log($"模板已保存: {name}");
    }

    public async Task<ConfigTemplate> LoadTemplateAsync(string name)
    {
        var templatePath = Path.Combine(TemplatesDirectory, $"{name}.template.json");
        var json = await File.ReadAllTextAsync(templatePath);
        return JsonSerializer.Deserialize<ConfigTemplate>(json);
    }

    public List<string> GetTemplateList()
    {
        if (!Directory.Exists(TemplatesDirectory))
            return new List<string>();

        return Directory.GetFiles(TemplatesDirectory, "*.template.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Select(n => n.Replace(".template", ""))
            .ToList();
    }
}
```

### 7. 历史记录功能
```csharp
// Services/HistoryService.cs
public class HistoryService : IHistoryService
{
    private const string HistoryFile = "generation_history.json";

    public class HistoryEntry
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string SearchPath { get; set; }
        public string OutputPath { get; set; }
        public List<string> ProjectNames { get; set; }
        public GenerationStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum GenerationStatus
    {
        Success,
        PartialSuccess,
        Failed
    }

    public async Task AddHistoryEntryAsync(HistoryEntry entry)
    {
        var history = await LoadHistoryAsync();
        history.Insert(0, entry);  // 最新的在前面

        // 保持最近50条记录
        if (history.Count > 50)
        {
            history = history.Take(50).ToList();
        }

        await SaveHistoryAsync(history);
    }

    public async Task<List<HistoryEntry>> LoadHistoryAsync()
    {
        if (!File.Exists(HistoryFile))
            return new List<HistoryEntry>();

        var json = await File.ReadAllTextAsync(HistoryFile);
        return JsonSerializer.Deserialize<List<HistoryEntry>>(json) ?? new();
    }
}
```

### 8. 批量操作功能
```csharp
// ViewModels/BatchOperationViewModel.cs
public class BatchOperationViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit> BatchGenerateCommand { get; }

    private async Task BatchGenerateAsync()
    {
        var folders = await SelectMultipleFoldersAsync();

        LoggingService.Log($"开始批量处理 {folders.Count} 个文件夹...");

        int successCount = 0;
        int failedCount = 0;

        foreach (var folder in folders)
        {
            try
            {
                LoggingService.Log($"处理: {folder}");

                var projects = await _projectFinder.FindProjectsAsync(folder);
                await _configGenerator.GenerateAsync(projects, Path.Combine(folder, ".vscode"));

                successCount++;
                LoggingService.Log($"✓ 完成: {folder}", LogLevel.Success);
            }
            catch (Exception ex)
            {
                failedCount++;
                LoggingService.Log($"✗ 失败: {folder} - {ex.Message}", LogLevel.Error);
            }
        }

        LoggingService.Log($"批量处理完成！成功: {successCount}, 失败: {failedCount}");
    }
}
```

---

## 🔌 集成现有Core层

### 依赖注入设置
```csharp
// App.axaml.cs
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        ConfigureServices();
    }

    private void ConfigureServices()
    {
        var services = new ServiceCollection();

        // 注册Core层服务（现有）
        services.AddSingleton<IProjectFinder, ProjectFinderService>();
        services.AddSingleton<IProjectParser, ProjectParserService>();
        services.AddSingleton<IConfigGenerator, ConfigGeneratorService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();

        // 注册UI层服务（新增）
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ITemplateService, TemplateService>();
        services.AddSingleton<IHistoryService, HistoryService>();
        services.AddSingleton<IPortConfigurationService, PortConfigurationService>();

        // 注册ViewModels
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<ProjectScanViewModel>();
        services.AddTransient<ConfigurationViewModel>();
        services.AddTransient<LogViewModel>();
        services.AddTransient<TemplateManagerViewModel>();
        services.AddTransient<HistoryViewModel>();

        // 配置Serilog
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/vscodegen-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        services.AddLogging(builder => builder.AddSerilog());

        Services = services.BuildServiceProvider();
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

## 📦 实施步骤

### 阶段1: 项目基础搭建（1周）
1. ✅ 创建Avalonia桌面项目
2. ✅ 配置项目结构和依赖
3. ✅ 设置MVVM架构
4. ✅ 配置依赖注入
5. ✅ 集成现有Core层服务

### 阶段2: 核心UI实现（2周）
1. ✅ 实现主窗口和导航
2. ✅ 实现项目扫描视图
3. ✅ 实现拖拽功能
4. ✅ 实现项目树形列表
5. ✅ 实现勾选和过滤功能

### 阶段3: 配置功能实现（2周）
1. ✅ 实现配置视图UI
2. ✅ 实现端口配置功能
3. ✅ 实现多站点配置
4. ✅ 实现配置预览
5. ✅ 集成ConfigGeneratorService

### 阶段4: 扩展功能实现（2周）
1. ✅ 实现日志视图
2. ✅ 实现模板管理
3. ✅ 实现历史记录
4. ✅ 实现批量操作
5. ✅ 添加导入/导出功能

### 阶段5: 测试和优化（1周）
1. ✅ 功能测试
2. ✅ 性能优化
3. ✅ UI/UX优化
4. ✅ 跨平台测试（Windows/Linux/macOS）
5. ✅ 文档更新

---

## 🎯 保持CLI功能

桌面应用和CLI将共存，用户可以选择使用：

### 双模式支持
```csharp
// Program.cs (Desktop)
public static void Main(string[] args)
{
    if (args.Length > 0 && args[0] == "--cli")
    {
        // 启动CLI模式
        var cliArgs = args.Skip(1).ToArray();
        CLI.Program.Main(cliArgs);
    }
    else
    {
        // 启动GUI模式
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
}
```

### 安装选项
```bash
# 安装桌面版（包含GUI和CLI）
dotnet tool install --global VsCodeDebugGen.Desktop

# 使用GUI
vscodegen

# 使用CLI
vscodegen --cli -p /path/to/project

# 或保留原有的CLI工具
dotnet tool install --global VsCodeDebugGen.CLI
vscodegen-cli -p /path/to/project
```

---

## 📊 版本对比

| 功能 | v2.0 CLI | v3.0 Desktop |
|------|---------|--------------|
| 项目扫描 | ✅ 命令行 | ✅ GUI + 拖拽 |
| 项目选择 | ✅ 交互式命令行 | ✅ 树形勾选 |
| 配置生成 | ✅ | ✅ + 预览 |
| 端口配置 | ❌ | ✅ |
| 多站点配置 | ❌ | ✅ |
| 日志输出 | ✅ 控制台 | ✅ GUI实时显示 |
| 模板管理 | ❌ | ✅ |
| 历史记录 | ❌ | ✅ |
| 批量操作 | ❌ | ✅ |
| 跨平台 | ✅ | ✅ |

---

## 🎨 UI主题

使用 **Avalonia Fluent Theme**，支持：
- 🌞 浅色主题
- 🌙 深色主题
- 🎨 自定义配色方案

---

## 📝 文档更新

需要更新的文档：
- ✅ README.md - 添加桌面版说明
- ✅ QUICK_START.md - 添加GUI使用指南
- ✅ USER_GUIDE_v3.0.md - 新建详细用户指南
- ✅ DEVELOPER_GUIDE_v3.0.md - 新建开发者文档
- ✅ RELEASE_NOTES_v3.0.0.md - 发布说明

---

## 🚀 发布计划

### 测试版（Beta）
- 发布时间: 开发完成后2周
- 平台: Windows x64
- 收集用户反馈

### 正式版（v3.0.0）
- 发布时间: Beta测试后2周
- 平台: Windows x64, Linux x64, macOS (Intel & ARM)
- 发布渠道:
  - NuGet (dotnet tool)
  - GitHub Releases (独立可执行文件)
  - Microsoft Store (可选)

---

## ⚠️ 风险和注意事项

1. **学习曲线**: Avalonia框架需要学习时间
2. **跨平台测试**: 需要在多个平台上测试
3. **性能**: 大型工作区（100+项目）的性能优化
4. **向后兼容**: 确保CLI功能继续可用
5. **文件安全**: 防止覆盖现有配置文件

---

## 📞 反馈和支持

- GitHub Issues: 报告问题和建议
- 文档: 完善的使用文档和API文档
- 示例: 提供示例项目和视频教程

---

**升级计划版本**: 1.0
**创建日期**: 2026-02-02
**最后更新**: 2026-02-02
**计划状态**: ✅ 已批准，待实施
