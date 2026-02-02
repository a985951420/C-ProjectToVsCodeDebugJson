# VSCode Debug Generator

> 🚀 为 C# 项目快速生成 VSCode 调试配置文件的强大工具

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## 📋 项目简介

**VSCode Debug Generator** 是一个企业级的开发工具，提供 **命令行 (CLI)** 和 **桌面应用 (GUI)** 两种使用方式，可以自动扫描 C# 项目（.csproj文件），并生成 VSCode 所需的 `launch.json` 和 `tasks.json` 配置文件，让你无需手动配置即可直接在 VSCode 中调试 C# 项目。

### ✨ 核心特性

- **🔍 智能扫描**: 自动递归查找目录下所有 .csproj 文件
- **🎯 灵活过滤**: 支持包含/排除特定项目
- **⚙️ 配置记忆**: 保存用户选择，下次使用更便捷
- **🎨 双界面支持**:
  - **CLI 命令行工具**: 交互式/非交互式模式，适合命令行操作和 CI/CD 集成
  - **桌面应用 (GUI)**: 基于 Avalonia 的跨平台图形界面，可视化操作更便捷
- **🏗️ 企业级架构**: 分层设计，遵循SOLID原则
- **📦 灵活部署**: 支持全局工具安装或独立可执行文件

## 🚀 快速开始

### 方式 1: 一键启动（推荐）

项目提供了统一的启动脚本，可以轻松选择启动模式：

```bash
# Windows 用户：双击或运行
start.bat
```

启动后可选择：
- **CLI 命令行工具** - 命令行界面
- **桌面应用 (GUI)** - 图形界面
- **构建桌面应用** - 编译桌面应用
- **打包桌面应用** - 生成独立可执行文件

### 方式 2: 独立启动

#### 启动 CLI 命令行工具

```bash
# Windows
run-cli.bat

# 或者直接使用 dotnet
dotnet run --project src/VsCodeDebugGen.CLI/生成调试.csproj
```

#### 启动桌面应用

```bash
# Windows - 运行桌面应用
run-desktop.bat

# Windows - 构建桌面应用
build-desktop.bat

# Windows - 打包为独立可执行文件（包含运行时）
publish-desktop.bat
```

打包后的可执行文件位于 `publish/desktop/` 目录，可直接在任何 Windows 电脑上运行，无需安装 .NET 运行时。

### 方式 3: 作为全局工具安装

```bash
# 使用安装脚本
scripts/install.bat        # Windows
scripts/install.sh         # Linux/macOS

# 或手动安装
dotnet pack -c Release
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen

# 全局使用
vscodegen
```

### CLI 命令行使用

#### 交互式模式（推荐）

```bash
# 直接运行，进入交互式向导
vscodegen
```

#### 非交互式模式

```bash
# 在当前目录搜索并生成配置
vscodegen --path . --output .

# 包含特定项目
vscodegen --path ./src --include "MyProject,WebApi"

# 排除测试项目
vscodegen --exclude "*.Tests"
```

## 📖 命令行参数

```
用法: vscodegen [选项]

选项:
  -h, --help              显示帮助信息
  -v, --version           显示版本信息
  -i, --interactive       交互模式（默认）
  -p, --path <路径>       指定搜索 .csproj 文件的目录路径
  -o, --output <路径>     指定 .vscode 目录的保存路径
  --include <项目>        包含的项目名称（逗号分隔）
  --exclude <项目>        排除的项目名称（逗号分隔）
  --verbose               显示详细输出
```

## 🏗️ 项目架构

采用企业级分层架构，遵循 SOLID 原则：

```
├── src/
│   ├── VsCodeDebugGen.Core/          # 核心业务层
│   │   ├── Interfaces/               # 接口抽象
│   │   ├── Models/                   # 数据模型
│   │   └── Services/                 # 业务服务
│   ├── VsCodeDebugGen.Infrastructure/  # 基础设施层
│   │   └── Configuration/            # 配置管理
│   ├── VsCodeDebugGen.CLI/           # 命令行界面层
│   │   ├── Commands/                 # 命令处理
│   │   ├── UI/                       # 用户交互
│   │   └── Program.cs                # CLI 入口
│   └── VsCodeDebugGen.Desktop/       # 桌面应用层（Avalonia）
│       ├── ViewModels/               # MVVM 视图模型
│       ├── Views/                    # XAML 视图
│       ├── Services/                 # 桌面应用服务
│       └── Models/                   # UI 模型
├── docs/                             # 项目文档
├── scripts/                          # 安装/卸载脚本
├── start.bat                         # 统一启动脚本
├── run-cli.bat                       # CLI 启动脚本
├── run-desktop.bat                   # 桌面应用启动脚本
├── build-desktop.bat                 # 桌面应用构建脚本
└── publish-desktop.bat               # 桌面应用打包脚本
```

## 💡 使用示例

### 示例 1: 交互模式生成配置

```bash
vscodegen
# 按提示输入路径和选择项目
```

### 示例 2: 自动化脚本

```bash
#!/bin/bash
vscodegen --path ./src --output ./ --exclude "*.Tests"
echo "配置生成完成！"
```

## 📁 生成的文件

工具会在指定目录创建 `.vscode` 文件夹，包含：

- **launch.json**: VSCode 调试配置
- **tasks.json**: 构建任务配置

## 📚 文档

- [快速开始指南](docs/QUICK_START.md)
- [项目总览](docs/PROJECT_SUMMARY.md)
- [环境配置说明](docs/ENVIRONMENT_SETUP.md)
- [部署检查清单](docs/DEPLOYMENT_CHECKLIST.md)
- [发布说明 v2.0.0](docs/RELEASE_NOTES_v2.0.0.md)
- [升级计划 v3.0.0](docs/UPGRADE_PLAN_v3.0.0.md)
- [项目规范](docs/SPEC.md)
- [任务清单](docs/TASK.md)
- [任务进度](docs/TASK_PROGRESS.md)

## 🔧 开发

### 环境要求

- .NET 8.0 SDK 或更高版本
- Windows 10/11（桌面应用）
- Visual Studio 2022 或 VS Code（可选）

### 构建项目

```bash
# 还原依赖
dotnet restore

# 构建整个解决方案
dotnet build VsCodeDebugGen.sln

# 构建 CLI
dotnet build src/VsCodeDebugGen.CLI/生成调试.csproj

# 构建桌面应用
dotnet build src/VsCodeDebugGen.Desktop/VsCodeDebugGen.Desktop.csproj -c Release
```

### 运行项目

```bash
# 运行 CLI
dotnet run --project src/VsCodeDebugGen.CLI/生成调试.csproj

# 运行桌面应用
dotnet run --project src/VsCodeDebugGen.Desktop/VsCodeDebugGen.Desktop.csproj
```

### 打包发布

#### 打包 CLI 为全局工具

```bash
# 打包
dotnet pack src/VsCodeDebugGen.CLI/生成调试.csproj -c Release -o ./nupkg

# 本地安装
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen

# 卸载
dotnet tool uninstall --global VsCodeDebugGen
```

#### 打包桌面应用

```bash
# 使用打包脚本（推荐）
publish-desktop.bat

# 或手动打包
dotnet publish src/VsCodeDebugGen.Desktop/VsCodeDebugGen.Desktop.csproj ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    --output publish/desktop ^
    /p:PublishSingleFile=true
```

## 📝 更新日志

### v2.0.0 (2025-02-02)
- ✨ 完全重构，采用企业级分层架构
- ✨ 新增 Avalonia 桌面应用 (GUI)
- ✨ 新增非交互式命令行模式
- ✨ 支持配置文件持久化
- ✨ 改进错误处理和用户提示
- ✨ 美化终端输出
- ✨ 支持作为全局工具安装
- ✨ 提供统一启动脚本
- ✨ 支持桌面应用独立打包
- 📁 文档和脚本文件重新组织

## 📄 许可证

MIT License

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

---

**如果这个工具对你有帮助，请给个 ⭐️ Star！**
