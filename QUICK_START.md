# 快速使用指南

## 安装

### Windows 用户

双击运行 `install.bat` 文件，或在命令行中执行：

```cmd
install.bat
```

### Linux/macOS 用户

```bash
chmod +x install.sh
./install.sh
```

## 使用

安装完成后，可以在任何目录使用 `vscodegen` 命令：

### 交互模式（最简单）

```bash
# 直接运行，按提示操作
vscodegen
```

### 命令行模式

```bash
# 查看帮助
vscodegen --help

# 查看版本
vscodegen --version

# 指定路径生成
vscodegen --path ./src --output ./

# 包含特定项目
vscodegen --path . --include "MyProject,WebApi"

# 排除测试项目
vscodegen --exclude "*.Tests,*.Test"
```

## 示例场景

### 场景 1: 新项目初始化

```bash
cd MyNewProject
vscodegen
# 选择所有项目，生成配置
```

### 场景 2: 只为特定项目生成

```bash
cd MySolution
vscodegen --include "MainProject,ImportantService"
```

### 场景 3: CI/CD 脚本集成

```bash
#!/bin/bash
# 在部署前自动生成配置
vscodegen --path ./src --output ./ --verbose
```

## 卸载

### Windows

```cmd
uninstall.bat
```

### Linux/macOS

```bash
dotnet tool uninstall --global VsCodeDebugGen
```

## 常见问题

**Q: 工具找不到我的项目？**
A: 确保路径正确，且包含 .csproj 文件。使用 `--verbose` 查看详细日志。

**Q: 生成的配置无法使用？**
A: 确保项目至少构建过一次，存在 bin 目录和编译输出。

**Q: 如何更新工具？**
A: 重新运行安装脚本即可自动更新。

## 技术支持

- GitHub Issues: [提交问题](https://github.com/your-repo/issues)
- 文档: [README.md](README.md)
