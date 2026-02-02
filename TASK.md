# VsCode调试配置生成工具 v3.0 - 实施任务清单

## 📋 任务概览

**总任务数**: 60+
**预计工期**: 8 周
**当前状态**: 🟡 进行中

---

## 🎯 任务执行规则

1. ✅ = 已完成
2. 🔄 = 进行中
3. ⬜ = 待开始
4. ❌ = 已跳过/不需要
5. 任务按顺序执行，前置依赖必须完成

---

## 📦 阶段 1: 项目基础搭建

### 1.1 创建解决方案和项目结构

#### ⬜ Task 1.1.1: 创建 Desktop 项目
**描述**: 创建 Avalonia 桌面应用项目
**命令**:
```bash
cd "d:\mySelfProject\生成VsCode调试文件\C-ProjectToVsCodeDebugJson"
dotnet new avalonia.mvvm -n VsCodeDebugGen.Desktop -o Desktop
```
**验收标准**:
- Desktop 文件夹已创建
- VsCodeDebugGen.Desktop.csproj 文件存在
- 项目可以编译

---

#### ⬜ Task 1.1.2: 重构现有项目结构
**描述**: 将现有代码移动到 src 文件夹
**步骤**:
1. 创建 `src/` 文件夹
2. 移动 `Core/` 到 `src/VsCodeDebugGen.Core/`
3. 移动 `Infrastructure/` 到 `src/VsCodeDebugGen.Infrastructure/`
4. 移动 `CLI/` 到 `src/VsCodeDebugGen.CLI/`
5. 移动 `Desktop/` 到 `src/VsCodeDebugGen.Desktop/`
6. 移动 `Program.cs` 和 `生成调试.csproj` 到 `src/VsCodeDebugGen.CLI/`

**验收标准**:
- src 文件夹结构正确
- 所有项目引用路径已更新

---

#### ⬜ Task 1.1.3: 创建新的解决方案文件
**描述**: 创建新的解决方案文件并添加所有项目
**命令**:
```bash
cd "d:\mySelfProject\生成VsCode调试文件\C-ProjectToVsCodeDebugJson"
dotnet new sln -n VsCodeDebugGen -o .
dotnet sln add src/VsCodeDebugGen.Core/*.csproj
dotnet sln add src/VsCodeDebugGen.Infrastructure/*.csproj
dotnet sln add src/VsCodeDebugGen.CLI/*.csproj
dotnet sln add src/VsCodeDebugGen.Desktop/*.csproj
```
**验收标准**:
- VsCodeDebugGen.sln 文件已创建
- 所有项目已添加到解决方案

---

### 1.2 配置 Desktop 项目

#### ⬜ Task 1.2.1: 修改 Desktop 项目文件
**描述**: 配置项目属性和依赖项
**文件**: `src/VsCodeDebugGen.Desktop/VsCodeDebugGen.Desktop.csproj`
**操作**: 按照 SPEC.md 中的配置更新项目文件
**验收标准**:
- 所有必需的 NuGet 包已添加
- 项目引用已添加
- 项目属性已配置

---

#### ⬜ Task 1.2.2: 添加项目引用
**描述**: 添加对 Core 和 Infrastructure 的引用
**命令**:
```bash
cd src/VsCodeDebugGen.Desktop
dotnet add reference ../VsCodeDebugGen.Core/VsCodeDebugGen.Core.csproj
dotnet add reference ../VsCodeDebugGen.Infrastructure/VsCodeDebugGen.Infrastructure.csproj
```
**验收标准**:
- 项目引用已添加
- 项目可以编译

---

#### ⬜ Task 1.2.3: 安装 Avalonia 模板
**描述**: 如果需要，安装 Avalonia 项目模板
**命令**:
```bash
dotnet new install Avalonia.Templates
```
**验收标准**:
- Avalonia 模板已安装

---

### 1.3 创建基础文件夹结构

#### ⬜ Task 1.3.1: 创建 ViewModels 文件夹结构
**描述**: 创建 ViewModel 层的文件夹
**路径**: `src/VsCodeDebugGen.Desktop/ViewModels/`
**子文件夹**:
- `Base/`

**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.2: 创建 Views 文件夹结构
**描述**: 创建 View 层的文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Views/`
**子文件夹**:
- `Dialogs/`

**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.3: 创建 Models 文件夹
**描述**: 创建 UI 模型文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Models/`
**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.4: 创建 Services 文件夹结构
**描述**: 创建服务层文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Services/`
**子文件夹**:
- `Interfaces/`

**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.5: 创建 Converters 文件夹
**描述**: 创建值转换器文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Converters/`
**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.6: 创建 Assets 文件夹结构
**描述**: 创建资源文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Assets/`
**子文件夹**:
- `Icons/`
- `Styles/`
- `Fonts/`

**验收标准**: 文件夹已创建

---

#### ⬜ Task 1.3.7: 创建 Helpers 文件夹
**描述**: 创建辅助类文件夹
**路径**: `src/VsCodeDebugGen.Desktop/Helpers/`
**验收标准**: 文件夹已创建

---

## 📐 阶段 2: MVVM 基础架构

### 2.1 创建 ViewModel 基类

#### ⬜ Task 2.1.1: 创建 ViewModelBase
**描述**: 创建 ViewModel 基类
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/Base/ViewModelBase.cs`
**内容**: 参考 SPEC.md 中的 ViewModelBase 实现
**验收标准**:
- 文件已创建
- 包含 IsBusy 和 Title 属性
- 使用 ReactiveObject

---

#### ⬜ Task 2.1.2: 创建 DialogViewModelBase
**描述**: 创建对话框 ViewModel 基类
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/Base/DialogViewModelBase.cs`
**验收标准**:
- 文件已创建
- 包含对话框相关属性

---

### 2.2 创建服务接口

#### ⬜ Task 2.2.1: 创建 ILoggingService 接口
**描述**: 定义日志服务接口
**文件**: `src/VsCodeDebugGen.Desktop/Services/Interfaces/ILoggingService.cs`
**方法**:
- `void Log(string message, LogLevel level = LogLevel.Info)`
- `ObservableCollection<LogEntry> Logs { get; }`

**验收标准**: 接口已定义

---

#### ⬜ Task 2.2.2: 创建 IDialogService 接口
**描述**: 定义对话框服务接口
**文件**: `src/VsCodeDebugGen.Desktop/Services/Interfaces/IDialogService.cs`
**方法**:
- `Task<string?> ShowFolderDialogAsync()`
- `Task<bool> ShowConfirmationAsync(string message)`
- `Task ShowErrorAsync(string message)`

**验收标准**: 接口已定义

---

#### ⬜ Task 2.2.3: 创建 ITemplateService 接口
**描述**: 定义模板服务接口
**文件**: `src/VsCodeDebugGen.Desktop/Services/Interfaces/ITemplateService.cs`
**方法**:
- `Task SaveTemplateAsync(string name, ConfigTemplate template)`
- `Task<ConfigTemplate> LoadTemplateAsync(string name)`
- `List<string> GetTemplateList()`
- `Task DeleteTemplateAsync(string name)`

**验收标准**: 接口已定义

---

#### ⬜ Task 2.2.4: 创建 IHistoryService 接口
**描述**: 定义历史服务接口
**文件**: `src/VsCodeDebugGen.Desktop/Services/Interfaces/IHistoryService.cs`
**方法**:
- `Task AddHistoryEntryAsync(HistoryEntry entry)`
- `Task<List<HistoryEntry>> LoadHistoryAsync()`
- `Task ClearHistoryAsync()`

**验收标准**: 接口已定义

---

#### ⬜ Task 2.2.5: 创建 IPortConfigurationService 接口
**描述**: 定义端口配置服务接口
**文件**: `src/VsCodeDebugGen.Desktop/Services/Interfaces/IPortConfigurationService.cs`
**方法**:
- `PortConfiguration AutoDetectPorts(ProjectInfo projectInfo)`
- `bool IsPortAvailable(int port)`
- `int GetNextAvailablePort(int startPort)`

**验收标准**: 接口已定义

---

### 2.3 创建 UI 模型

#### ⬜ Task 2.3.1: 创建 LogEntry 模型
**描述**: 创建日志条目模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/LogEntry.cs`
**属性**:
- `DateTime Timestamp`
- `LogLevel Level`
- `string Message`
- `string Icon` (计算属性)

**验收标准**: 模型已创建

---

#### ⬜ Task 2.3.2: 创建 LogLevel 枚举
**描述**: 创建日志级别枚举
**文件**: `src/VsCodeDebugGen.Desktop/Models/LogLevel.cs`
**值**: `Info, Success, Warning, Error, Debug`
**验收标准**: 枚举已创建

---

#### ⬜ Task 2.3.3: 创建 ProjectItemViewModel 模型
**描述**: 创建项目项视图模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/ProjectItemViewModel.cs`
**属性**:
- `bool IsSelected`
- `string ProjectPath`
- `string ProjectName`
- `string ProjectType`
- `string TargetFramework`
- `ObservableCollection<ProjectItemViewModel> Children`

**验收标准**: 模型已创建

---

#### ⬜ Task 2.3.4: 创建 PortConfiguration 模型
**描述**: 创建端口配置模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/PortConfiguration.cs`
**属性**:
- `string ProjectName`
- `int HttpPort`
- `int HttpsPort`
- `string Environment`

**验收标准**: 模型已创建

---

#### ⬜ Task 2.3.5: 创建 MultiSiteConfiguration 模型
**描述**: 创建多站点配置模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/MultiSiteConfiguration.cs`
**属性**:
- `string ProjectName`
- `List<SiteConfig> Sites`

**验收标准**: 模型已创建

---

#### ⬜ Task 2.3.6: 创建 TemplateModel 模型
**描述**: 创建模板模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/TemplateModel.cs`
**属性**:
- `string Name`
- `string Description`
- `DateTime CreatedDate`
- `AppConfiguration Configuration`
- `List<PortConfiguration> PortConfigurations`

**验收标准**: 模型已创建

---

#### ⬜ Task 2.3.7: 创建 HistoryEntry 模型
**描述**: 创建历史条目模型
**文件**: `src/VsCodeDebugGen.Desktop/Models/HistoryEntry.cs`
**属性**:
- `Guid Id`
- `DateTime Timestamp`
- `string SearchPath`
- `string OutputPath`
- `List<string> ProjectNames`
- `GenerationStatus Status`

**验收标准**: 模型已创建

---

### 2.4 实现服务

#### ⬜ Task 2.4.1: 实现 LoggingService
**描述**: 实现日志服务
**文件**: `src/VsCodeDebugGen.Desktop/Services/LoggingService.cs`
**功能**:
- 添加日志到集合
- 写入文件（Serilog）
- 线程安全

**验收标准**:
- 服务已实现
- 可以记录日志
- 可以在UI中显示

---

#### ⬜ Task 2.4.2: 实现 DialogService
**描述**: 实现对话框服务
**文件**: `src/VsCodeDebugGen.Desktop/Services/DialogService.cs`
**功能**:
- 显示文件夹选择对话框
- 显示确认对话框
- 显示错误对话框

**验收标准**: 服务已实现

---

#### ⬜ Task 2.4.3: 实现 TemplateService
**描述**: 实现模板服务
**文件**: `src/VsCodeDebugGen.Desktop/Services/TemplateService.cs`
**功能**:
- 保存模板到文件
- 加载模板
- 列出所有模板
- 删除模板

**验收标准**:
- 服务已实现
- 模板可以保存和加载

---

#### ⬜ Task 2.4.4: 实现 HistoryService
**描述**: 实现历史服务
**文件**: `src/VsCodeDebugGen.Desktop/Services/HistoryService.cs`
**功能**:
- 添加历史记录
- 加载历史记录
- 清空历史

**验收标准**:
- 服务已实现
- 历史记录可以持久化

---

#### ⬜ Task 2.4.5: 实现 PortConfigurationService
**描述**: 实现端口配置服务
**文件**: `src/VsCodeDebugGen.Desktop/Services/PortConfigurationService.cs`
**功能**:
- 自动检测端口
- 检查端口可用性
- 分配下一个可用端口

**验收标准**:
- 服务已实现
- 可以检测和分配端口

---

### 2.5 配置依赖注入

#### ⬜ Task 2.5.1: 修改 App.axaml.cs
**描述**: 配置服务和依赖注入
**文件**: `src/VsCodeDebugGen.Desktop/App.axaml.cs`
**操作**: 按照 SPEC.md 中的配置实现依赖注入
**验收标准**:
- 所有服务已注册
- ViewModels 已注册
- 日志已配置

---

#### ⬜ Task 2.5.2: 修改 Program.cs
**描述**: 配置应用程序入口
**文件**: `src/VsCodeDebugGen.Desktop/Program.cs`
**验收标准**:
- 应用可以启动
- 支持 CLI 模式切换

---

## 🎨 阶段 3: UI 样式和资源

### 3.1 创建样式文件

#### ⬜ Task 3.1.1: 创建 Colors.axaml
**描述**: 定义颜色资源
**文件**: `src/VsCodeDebugGen.Desktop/Assets/Styles/Colors.axaml`
**内容**: 按照 SPEC.md 中的颜色方案定义
**验收标准**: 颜色资源已定义

---

#### ⬜ Task 3.1.2: 创建 Buttons.axaml
**描述**: 定义按钮样式
**文件**: `src/VsCodeDebugGen.Desktop/Assets/Styles/Buttons.axaml`
**样式**:
- PrimaryButton
- SecondaryButton
- DangerButton

**验收标准**: 按钮样式已定义

---

#### ⬜ Task 3.1.3: 创建 TextBlocks.axaml
**描述**: 定义文本样式
**文件**: `src/VsCodeDebugGen.Desktop/Assets/Styles/TextBlocks.axaml`
**样式**:
- Heading1, Heading2, Heading3
- BodyText, CaptionText

**验收标准**: 文本样式已定义

---

#### ⬜ Task 3.1.4: 更新 App.axaml
**描述**: 引入样式资源
**文件**: `src/VsCodeDebugGen.Desktop/App.axaml`
**操作**: 添加样式资源字典引用
**验收标准**: 样式可以在全局使用

---

### 3.2 添加图标资源

#### ⬜ Task 3.2.1: 准备图标文件
**描述**: 添加应用图标和UI图标
**路径**: `src/VsCodeDebugGen.Desktop/Assets/Icons/`
**文件**:
- `app-icon.ico` (应用图标)
- `folder.png` (文件夹图标)
- `project.png` (项目图标)
- `console.png` (控制台图标)
- `web.png` (Web应用图标)
- `library.png` (类库图标)

**验收标准**: 图标文件已添加到项目

---

## 🖥️ 阶段 4: 主窗口和导航

### 4.1 创建主窗口

#### ⬜ Task 4.1.1: 创建 MainWindowViewModel
**描述**: 创建主窗口视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/MainWindowViewModel.cs`
**属性**:
- `ViewModelBase? CurrentView`
- `string StatusText`
- `string Version`

**命令**:
- `NavigateToProjectScanCommand`
- `NavigateToConfigurationCommand`
- `NavigateToLogCommand`
- `NavigateToTemplateCommand`
- `NavigateToHistoryCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 4.1.2: 创建 MainWindow.axaml
**描述**: 创建主窗口视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/MainWindow.axaml`
**布局**:
- 顶部：标题栏
- 中间：Tab导航
- 内容区：ContentControl 绑定到 CurrentView
- 底部：状态栏

**验收标准**:
- 窗口布局正确
- 导航正常工作

---

#### ⬜ Task 4.1.3: 实现 MainWindow.axaml.cs
**描述**: 实现主窗口代码
**文件**: `src/VsCodeDebugGen.Desktop/Views/MainWindow.axaml.cs`
**验收标准**: 窗口可以正常显示

---

### 4.2 实现导航系统

#### ⬜ Task 4.2.1: 实现视图切换逻辑
**描述**: 在 MainWindowViewModel 中实现视图切换
**方法**: 切换 CurrentView 属性到不同的 ViewModel
**验收标准**:
- 可以切换视图
- 每个 Tab 显示正确的内容

---

## 📂 阶段 5: 项目扫描视图

### 5.1 创建项目扫描视图

#### ⬜ Task 5.1.1: 创建 ProjectScanViewModel
**描述**: 创建项目扫描视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/ProjectScanViewModel.cs`
**属性**:
- `string SearchPath`
- `string OutputPath`
- `bool RecursiveScan`
- `ObservableCollection<ProjectItemViewModel> Projects`
- `string FilterText`

**命令**:
- `BrowseSearchPathCommand`
- `BrowseOutputPathCommand`
- `ScanCommand`
- `GenerateCommand`
- `SelectAllCommand`
- `InvertSelectionCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 5.1.2: 创建 ProjectScanView.axaml
**描述**: 创建项目扫描视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/ProjectScanView.axaml`
**布局**: 参考 UPGRADE_PLAN 中的设计
**控件**:
- 文件夹选择区域
- 拖拽区域
- 项目树形列表
- 操作按钮

**验收标准**:
- 视图布局正确
- 绑定正常工作

---

#### ⬜ Task 5.1.3: 实现拖拽功能
**描述**: 实现文件夹拖拽
**文件**: `src/VsCodeDebugGen.Desktop/Views/ProjectScanView.axaml.cs`
**功能**:
- 拖拽进入时高亮
- 拖拽释放时扫描
- 验证是否为文件夹

**验收标准**:
- 可以拖拽文件夹
- 自动开始扫描

---

#### ⬜ Task 5.1.4: 实现项目扫描逻辑
**描述**: 在 ViewModel 中实现扫描逻辑
**操作**: 调用 IProjectFinder 服务
**验收标准**:
- 可以扫描项目
- 结果显示在列表中
- 日志正确输出

---

#### ⬜ Task 5.1.5: 实现项目过滤功能
**描述**: 实现搜索过滤
**操作**: 根据 FilterText 过滤项目列表
**验收标准**:
- 输入搜索文本时列表实时过滤
- 过滤包含名称、类型、框架

---

#### ⬜ Task 5.1.6: 实现批量选择功能
**描述**: 实现全选、反选功能
**命令**: `SelectAllCommand`, `InvertSelectionCommand`
**验收标准**:
- 全选按钮工作正常
- 反选按钮工作正常

---

## ⚙️ 阶段 6: 配置视图

### 6.1 创建配置视图

#### ⬜ Task 6.1.1: 创建 ConfigurationViewModel
**描述**: 创建配置视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/ConfigurationViewModel.cs`
**属性**:
- `string ConsoleType`
- `bool StopAtEntry`
- `bool AutoConfigurePorts`
- `ObservableCollection<PortConfiguration> PortConfigurations`
- `ObservableCollection<MultiSiteConfiguration> MultiSiteConfigurations`

**命令**:
- `PreviewCommand`
- `GenerateCommand`
- `AddSiteConfigCommand`
- `EditPortCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 6.1.2: 创建 ConfigurationView.axaml
**描述**: 创建配置视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/ConfigurationView.axaml`
**布局**: 参考 UPGRADE_PLAN 中的设计
**区域**:
- 通用设置
- Web应用端口配置
- 多站点配置

**验收标准**:
- 视图布局正确
- 绑定正常工作

---

#### ⬜ Task 6.1.3: 实现端口自动检测
**描述**: 实现端口自动配置
**操作**: 调用 IPortConfigurationService
**验收标准**:
- 扫描后自动检测Web应用
- 自动分配端口

---

#### ⬜ Task 6.1.4: 实现多站点配置
**描述**: 实现多站点配置功能
**操作**: 支持添加、编辑、删除站点配置
**验收标准**:
- 可以添加站点配置
- 每个站点有独立端口

---

#### ⬜ Task 6.1.5: 创建配置预览对话框
**描述**: 创建预览对话框
**文件**:
- `src/VsCodeDebugGen.Desktop/Views/Dialogs/PreviewDialog.axaml`
- `src/VsCodeDebugGen.Desktop/ViewModels/PreviewDialogViewModel.cs`

**功能**:
- 显示 launch.json 预览
- 显示 tasks.json 预览
- 支持复制到剪贴板

**验收标准**:
- 对话框正确显示
- 可以预览配置

---

#### ⬜ Task 6.1.6: 实现配置生成
**描述**: 实现生成配置功能
**操作**: 调用 IConfigGenerator 服务
**验收标准**:
- 可以生成配置
- 考虑端口配置
- 日志正确输出

---

## 📝 阶段 7: 日志视图

### 7.1 创建日志视图

#### ⬜ Task 7.1.1: 创建 LogViewModel
**描述**: 创建日志视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/LogViewModel.cs`
**属性**:
- `ObservableCollection<LogEntry> FilteredLogs`
- `string FilterText`
- `LogLevel? FilterLevel`

**命令**:
- `ClearLogsCommand`
- `ExportLogsCommand`
- `CopyLogsCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 7.1.2: 创建 LogView.axaml
**描述**: 创建日志视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/LogView.axaml`
**布局**: 参考 UPGRADE_PLAN 中的设计
**控件**:
- 过滤选项
- 日志列表（ListBox）
- 操作按钮

**验收标准**:
- 视图布局正确
- 日志实时显示

---

#### ⬜ Task 7.1.3: 创建日志级别转颜色转换器
**描述**: 创建值转换器
**文件**: `src/VsCodeDebugGen.Desktop/Converters/LogLevelToColorConverter.cs`
**功能**: 根据日志级别返回不同颜色
**验收标准**:
- 转换器工作正常
- 日志显示不同颜色

---

#### ⬜ Task 7.1.4: 实现日志过滤
**描述**: 实现日志过滤逻辑
**操作**: 根据级别和文本过滤日志
**验收标准**:
- 可以按级别过滤
- 可以按文本搜索

---

#### ⬜ Task 7.1.5: 实现日志导出
**描述**: 实现导出日志功能
**格式**: 导出为文本文件
**验收标准**:
- 可以导出日志
- 包含时间戳和级别

---

## 📋 阶段 8: 模板管理视图

### 8.1 创建模板管理视图

#### ⬜ Task 8.1.1: 创建 TemplateManagerViewModel
**描述**: 创建模板管理视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/TemplateManagerViewModel.cs`
**属性**:
- `ObservableCollection<TemplateModel> Templates`
- `TemplateModel? SelectedTemplate`

**命令**:
- `NewTemplateCommand`
- `ApplyTemplateCommand`
- `EditTemplateCommand`
- `DeleteTemplateCommand`
- `ImportTemplateCommand`
- `ExportTemplateCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 8.1.2: 创建 TemplateManagerView.axaml
**描述**: 创建模板管理视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/TemplateManagerView.axaml`
**布局**: 参考 UPGRADE_PLAN 中的设计
**控件**:
- 模板列表
- 操作按钮

**验收标准**:
- 视图布局正确
- 绑定正常工作

---

#### ⬜ Task 8.1.3: 创建模板编辑对话框
**描述**: 创建模板编辑对话框
**文件**:
- `src/VsCodeDebugGen.Desktop/Views/Dialogs/TemplateEditDialog.axaml`
- `src/VsCodeDebugGen.Desktop/ViewModels/TemplateEditDialogViewModel.cs`

**功能**:
- 编辑模板名称和描述
- 配置模板内容

**验收标准**: 对话框正常工作

---

#### ⬜ Task 8.1.4: 实现模板应用
**描述**: 实现应用模板到当前配置
**操作**: 加载模板并应用到配置视图
**验收标准**:
- 可以应用模板
- 配置自动更新

---

#### ⬜ Task 8.1.5: 实现模板导入导出
**描述**: 实现导入导出功能
**格式**: JSON 文件
**验收标准**:
- 可以导出模板
- 可以导入模板

---

## 📚 阶段 9: 历史记录视图

### 9.1 创建历史记录视图

#### ⬜ Task 9.1.1: 创建 HistoryViewModel
**描述**: 创建历史记录视图模型
**文件**: `src/VsCodeDebugGen.Desktop/ViewModels/HistoryViewModel.cs`
**属性**:
- `ObservableCollection<HistoryEntry> History`
- `string FilterText`

**命令**:
- `RegenerateCommand`
- `ClearHistoryCommand`

**验收标准**: ViewModel 已创建

---

#### ⬜ Task 9.1.2: 创建 HistoryView.axaml
**描述**: 创建历史记录视图
**文件**: `src/VsCodeDebugGen.Desktop/Views/HistoryView.axaml`
**布局**: 参考 UPGRADE_PLAN 中的设计
**控件**:
- 搜索框
- 历史列表
- 操作按钮

**验收标准**:
- 视图布局正确
- 历史记录显示

---

#### ⬜ Task 9.1.3: 实现重新生成功能
**描述**: 实现从历史记录重新生成
**操作**: 加载历史配置并重新生成
**验收标准**:
- 可以从历史重新生成
- 配置自动应用

---

#### ⬜ Task 9.1.4: 实现历史搜索
**描述**: 实现历史记录搜索
**操作**: 按路径或项目名搜索
**验收标准**:
- 搜索功能正常
- 实时过滤

---

## 🔧 阶段 10: 值转换器

### 10.1 创建转换器

#### ⬜ Task 10.1.1: 创建 BoolToVisibilityConverter
**描述**: 创建布尔到可见性转换器
**文件**: `src/VsCodeDebugGen.Desktop/Converters/BoolToVisibilityConverter.cs`
**验收标准**: 转换器工作正常

---

#### ⬜ Task 10.1.2: 创建 StatusToColorConverter
**描述**: 创建状态到颜色转换器
**文件**: `src/VsCodeDebugGen.Desktop/Converters/StatusToColorConverter.cs`
**验收标准**: 转换器工作正常

---

#### ⬜ Task 10.1.3: 创建 ProjectTypeToIconConverter
**描述**: 创建项目类型到图标转换器
**文件**: `src/VsCodeDebugGen.Desktop/Converters/ProjectTypeToIconConverter.cs`
**验收标准**:
- 转换器工作正常
- 不同项目类型显示不同图标

---

## 🧪 阶段 11: 测试和优化

### 11.1 功能测试

#### ⬜ Task 11.1.1: 测试项目扫描功能
**描述**: 全面测试项目扫描
**场景**:
- 空文件夹
- 单个项目
- 多个项目
- 深层嵌套

**验收标准**: 所有场景正常工作

---

#### ⬜ Task 11.1.2: 测试端口配置功能
**描述**: 测试端口配置
**场景**:
- 自动检测
- 手动配置
- 端口冲突

**验收标准**: 端口配置正确

---

#### ⬜ Task 11.1.3: 测试配置生成
**描述**: 测试配置文件生成
**验收标准**:
- launch.json 正确
- tasks.json 正确
- 包含端口配置

---

#### ⬜ Task 11.1.4: 测试模板功能
**描述**: 测试模板管理
**操作**: 保存、加载、应用、删除
**验收标准**: 所有操作正常

---

#### ⬜ Task 11.1.5: 测试历史功能
**描述**: 测试历史记录
**操作**: 添加、查看、重新生成、清空
**验收标准**: 所有操作正常

---

### 11.2 UI/UX 优化

#### ⬜ Task 11.2.1: 优化响应速度
**描述**: 优化UI响应性能
**操作**:
- 异步操作
- 虚拟化长列表
- 减少UI更新

**验收标准**: UI 流畅无卡顿

---

#### ⬜ Task 11.2.2: 添加加载指示器
**描述**: 添加加载状态提示
**位置**: 扫描、生成时显示
**验收标准**:
- 显示进度
- 可以取消

---

#### ⬜ Task 11.2.3: 优化错误处理
**描述**: 改进错误提示
**操作**:
- 友好的错误消息
- 错误对话框
- 日志记录

**验收标准**: 错误提示清晰

---

#### ⬜ Task 11.2.4: 添加键盘快捷键
**描述**: 添加常用快捷键
**快捷键**:
- Ctrl+O: 浏览文件夹
- Ctrl+S: 生成配置
- Ctrl+F: 搜索/过滤
- F5: 刷新扫描

**验收标准**: 快捷键正常工作

---

### 11.3 跨平台测试

#### ⬜ Task 11.3.1: Windows 测试
**描述**: 在 Windows 上测试
**版本**: Windows 10/11
**验收标准**: 所有功能正常

---

#### ⬜ Task 11.3.2: Linux 测试
**描述**: 在 Linux 上测试
**版本**: Ubuntu 22.04+
**验收标准**: 所有功能正常

---

#### ⬜ Task 11.3.3: macOS 测试（可选）
**描述**: 在 macOS 上测试
**版本**: macOS 12.0+
**验收标准**: 所有功能正常

---

## 📦 阶段 12: 打包和文档

### 12.1 打包应用

#### ⬜ Task 12.1.1: 配置发布设置
**描述**: 配置项目发布属性
**操作**: 修改 .csproj 文件
**验收标准**: 发布配置正确

---

#### ⬜ Task 12.1.2: 发布 Windows 版本
**描述**: 发布 Windows 可执行文件
**命令**: `dotnet publish -c Release -r win-x64 --self-contained`
**验收标准**:
- 可执行文件生成
- 应用正常运行

---

#### ⬜ Task 12.1.3: 发布 Linux 版本
**描述**: 发布 Linux 可执行文件
**命令**: `dotnet publish -c Release -r linux-x64 --self-contained`
**验收标准**: 可执行文件生成

---

#### ⬜ Task 12.1.4: 创建安装程序（可选）
**描述**: 创建 Windows 安装程序
**工具**: Inno Setup 或 WiX
**验收标准**: 安装程序可用

---

### 12.2 更新文档

#### ⬜ Task 12.2.1: 更新 README.md
**描述**: 更新项目说明
**内容**: 添加桌面版说明和截图
**验收标准**: 文档完整

---

#### ⬜ Task 12.2.2: 创建 USER_GUIDE_v3.0.md
**描述**: 创建用户指南
**内容**:
- 安装说明
- 使用教程
- 常见问题

**验收标准**: 文档完整

---

#### ⬜ Task 12.2.3: 创建 RELEASE_NOTES_v3.0.0.md
**描述**: 创建发布说明
**内容**:
- 新功能
- 改进
- 已知问题

**验收标准**: 文档完整

---

#### ⬜ Task 12.2.4: 更新 QUICK_START.md
**描述**: 更新快速开始指南
**内容**: 添加桌面版快速开始
**验收标准**: 文档完整

---

## 🎉 完成检查清单

### ✅ 最终验收

#### ⬜ Task Final.1: 功能完整性检查
**检查项**:
- [ ] 项目扫描功能
- [ ] 配置生成功能
- [ ] 端口配置功能
- [ ] 多站点配置
- [ ] 日志功能
- [ ] 模板管理
- [ ] 历史记录
- [ ] 拖拽功能

---

#### ⬜ Task Final.2: 性能检查
**检查项**:
- [ ] 扫描 100 个项目 < 5 秒
- [ ] 生成配置 < 2 秒
- [ ] UI 响应 < 100ms
- [ ] 内存占用 < 200MB

---

#### ⬜ Task Final.3: 跨平台检查
**检查项**:
- [ ] Windows 测试通过
- [ ] Linux 测试通过
- [ ] macOS 测试通过（可选）

---

#### ⬜ Task Final.4: 文档检查
**检查项**:
- [ ] README.md 已更新
- [ ] USER_GUIDE_v3.0.md 已创建
- [ ] RELEASE_NOTES_v3.0.0.md 已创建
- [ ] API 文档已更新

---

#### ⬜ Task Final.5: 发布准备
**检查项**:
- [ ] 版本号已更新
- [ ] 发布包已生成
- [ ] 安装测试通过
- [ ] GitHub Release 已准备

---

## 📊 进度统计

- **总任务数**: 60+
- **已完成**: 0
- **进行中**: 0
- **待开始**: 60+
- **完成度**: 0%

---

## 📝 任务执行记录

### 执行日志
```
[开始时间] 任务开始
[结束时间] 任务完成
```

---

**文档版本**: 1.0
**创建日期**: 2026-02-02
**最后更新**: 2026-02-02
**状态**: ⬜ 待开始
