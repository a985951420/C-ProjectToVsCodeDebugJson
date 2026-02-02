# 环境变量配置说明

## Windows 系统

### 方式 1: 使用安装脚本（推荐）

运行 `install.bat` 后，工具会自动安装到全局环境，dotnet 会自动处理 PATH 配置。

### 方式 2: 手动配置 PATH

如果命令无法找到，需要将 dotnet tools 目录添加到系统 PATH：

1. **打开环境变量设置**
   - 按 `Win + R`，输入 `sysdm.cpl`
   - 点击"高级"选项卡 → "环境变量"

2. **添加路径**
   - 在"用户变量"或"系统变量"中找到 `Path`
   - 点击"编辑"，添加以下路径：
     ```
     %USERPROFILE%\.dotnet\tools
     ```
   - 或绝对路径：
     ```
     C:\Users\你的用户名\.dotnet\tools
     ```

3. **重启终端**
   - 关闭所有命令提示符/PowerShell 窗口
   - 重新打开，输入 `vscodegen --version` 测试

### 快速验证

```cmd
# 检查工具是否安装
dotnet tool list --global | findstr vscodegen

# 查看 dotnet tools 目录
dir %USERPROFILE%\.dotnet\tools

# 测试命令
vscodegen --version
```

## Linux / macOS 系统

### 方式 1: 使用安装脚本（推荐）

```bash
chmod +x install.sh
./install.sh
```

### 方式 2: 手动配置 PATH

1. **添加到 shell 配置文件**

   对于 **Bash** (~/.bashrc 或 ~/.bash_profile):
   ```bash
   echo 'export PATH="$HOME/.dotnet/tools:$PATH"' >> ~/.bashrc
   source ~/.bashrc
   ```

   对于 **Zsh** (~/.zshrc):
   ```bash
   echo 'export PATH="$HOME/.dotnet/tools:$PATH"' >> ~/.zshrc
   source ~/.zshrc
   ```

   对于 **Fish** (~/.config/fish/config.fish):
   ```fish
   echo 'set -gx PATH $HOME/.dotnet/tools $PATH' >> ~/.config/fish/config.fish
   source ~/.config/fish/config.fish
   ```

2. **重启终端或重新加载配置**
   ```bash
   # 查看当前 PATH
   echo $PATH

   # 验证工具路径
   which vscodegen
   ```

### 快速验证

```bash
# 检查工具是否安装
dotnet tool list --global | grep vscodegen

# 查看 dotnet tools 目录
ls -la ~/.dotnet/tools/

# 测试命令
vscodegen --version
```

## 常见问题

### Q: 安装后命令找不到？

**Windows:**
```cmd
# 方案 1: 使用完整路径
%USERPROFILE%\.dotnet\tools\vscodegen --version

# 方案 2: 重启所有终端窗口

# 方案 3: 手动添加 PATH（见上文）
```

**Linux/macOS:**
```bash
# 方案 1: 使用完整路径
~/.dotnet/tools/vscodegen --version

# 方案 2: 重新加载 shell 配置
source ~/.bashrc  # 或 ~/.zshrc

# 方案 3: 重启终端
```

### Q: 权限不足？

**Windows:**
- 以管理员身份运行 PowerShell 或 CMD

**Linux/macOS:**
```bash
# 检查权限
ls -l ~/.dotnet/tools/vscodegen

# 添加执行权限（如果需要）
chmod +x ~/.dotnet/tools/vscodegen
```

### Q: 多版本 .NET SDK？

```bash
# 查看已安装的 SDK
dotnet --list-sdks

# 查看 tools 路径
dotnet tool list --global

# 确保使用正确的 SDK
# 在项目根目录创建 global.json
{
  "sdk": {
    "version": "8.0.0",
    "rollForward": "latestMinor"
  }
}
```

## 验证安装成功

完成配置后，运行以下命令验证：

```bash
# 1. 检查版本
vscodegen --version
# 输出: VSCode Debug Generator v2.0.0

# 2. 查看帮助
vscodegen --help
# 输出: 帮助信息

# 3. 测试生成（在任意目录）
vscodegen --help

# 如果以上命令都能正常运行，说明配置成功！
```

## 卸载

如果需要卸载工具：

**Windows:**
```cmd
uninstall.bat
```

**Linux/macOS:**
```bash
dotnet tool uninstall --global VsCodeDebugGen
```

## 技术支持

如果仍然遇到问题，请：
1. 查看 [README.md](README.md)
2. 查看 [PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)
3. 在 GitHub 提交 Issue

---

**提示**: 大多数情况下，重启终端即可解决 PATH 相关问题。
