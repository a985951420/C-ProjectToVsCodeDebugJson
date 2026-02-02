# VSCode Debug Generator

> 🚀 为 C# 项目快速生成 VSCode 调试配置文件的命令行工具

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

## 📋 项目简介

**VSCode Debug Generator** 是一个企业级的命令行工具，可以自动扫描 C# 项目（.csproj文件），并生成 VSCode 所需的 `launch.json` 和 `tasks.json` 配置文件，让你无需手动配置即可直接在 VSCode 中调试 C# 项目。

### ✨ 核心特性

- **🔍 智能扫描**: 自动递归查找目录下所有 .csproj 文件
- **🎯 灵活过滤**: 支持包含/排除特定项目
- **⚙️ 配置记忆**: 保存用户选择，下次使用更便捷
- **🎨 双模式支持**:
  - 交互式模式：友好的命令行界面
  - 非交互式模式：适合CI/CD集成
- **🏗️ 企业级架构**: 分层设计，遵循SOLID原则
- **📦 全局工具**: 可作为 dotnet global tool 安装

## 🚀 快速开始

### 安装

#### 方式 1: 作为全局工具安装

```bash
# 打包项目
dotnet pack -c Release

# 从本地源安装
dotnet tool install --global --add-source ./bin/Release VsCodeDebugGen
```

#### 方式 2: 从源代码运行

```bash
git clone <repository-url>
cd C-ProjectToVsCodeDebugJson
dotnet build
dotnet run
```

### 基本使用

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
├── Core/                    # 核心业务层
│   ├── Interfaces/         # 接口抽象
│   ├── Models/             # 数据模型
│   └── Services/           # 业务服务
├── Infrastructure/          # 基础设施层
│   └── Configuration/      # 配置管理
├── CLI/                    # 命令行界面层
│   ├── Commands/           # 命令处理
│   └── UI/                 # 用户交互
└── Program.cs              # 应用入口
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

## 🔧 开发

### 构建项目

```bash
# 还原依赖
dotnet restore

# 构建
dotnet build

# 发布
dotnet build -c Release
```

### 打包为全局工具

```bash
# 打包
dotnet pack -c Release -o ./nupkg

# 本地安装
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen

# 卸载
dotnet tool uninstall --global VsCodeDebugGen
```

## 📝 更新日志

### v2.0.0 (2025-02-02)
- ✨ 完全重构，采用企业级分层架构
- ✨ 新增非交互式命令行模式
- ✨ 支持配置文件持久化
- ✨ 改进错误处理和用户提示
- ✨ 美化终端输出
- ✨ 支持作为全局工具安装

## 📄 许可证

MIT License

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

---

**如果这个工具对你有帮助，请给个 ⭐️ Star！**
