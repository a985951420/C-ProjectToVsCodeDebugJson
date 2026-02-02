# 🎉 VSCode Debug Generator v2.0.0

> 企业级架构重构，全面升级！

## 📦 下载安装

### 作为全局工具安装（推荐）

```bash
# 克隆仓库
git clone https://github.com/a985951420/C-ProjectToVsCodeDebugJson.git
cd C-ProjectToVsCodeDebugJson

# Windows 用户
install.bat

# Linux/macOS 用户
chmod +x install.sh
./install.sh
```

### 从源代码构建

```bash
git clone https://github.com/a985951420/C-ProjectToVsCodeDebugJson.git
cd C-ProjectToVsCodeDebugJson
dotnet build -c Release
dotnet pack -c Release
dotnet tool install --global --add-source ./bin/Release VsCodeDebugGen
```

## ✨ 主要更新

### 🏗️ 架构重构

- **企业级分层架构**: Core（核心业务） / Infrastructure（基础设施） / CLI（命令行界面）
- **SOLID 原则应用**: 单一职责、开闭、里氏替换、接口隔离、依赖倒置
- **依赖注入**: 通过接口抽象实现松耦合
- **模块化设计**: 清晰的职责分离，易于维护和扩展

### 🚀 新增功能

#### 命令行参数支持
```bash
vscodegen --path ./src --output ./ --include "MyProject" --exclude "*.Tests"
```

#### 双模式支持
- **交互式模式**: 美化的用户界面，带 emoji 和进度提示
- **非交互式模式**: 适合 CI/CD 集成的静默模式

#### 配置管理
- JSON 格式配置文件持久化
- 自动迁移旧版本配置
- 项目过滤功能（包含/排除）

#### 其他功能
- `--help` 完整的帮助文档
- `--version` 版本信息
- `--verbose` 详细日志输出
- 统一的错误处理机制

### 📦 全局工具

现在可以作为 dotnet global tool 安装：

```bash
vscodegen          # 交互模式
vscodegen --help   # 查看帮助
vscodegen -v       # 查看版本
```

## 📚 文档

完善的文档体系：

- **README.md** - 完整的项目文档
- **QUICK_START.md** - 快速开始指南
- **PROJECT_SUMMARY.md** - 项目架构和技术细节
- **ENVIRONMENT_SETUP.md** - 环境配置说明

## 🎯 使用示例

### 示例 1: 交互模式

```bash
# 最简单的使用方式
vscodegen
```

### 示例 2: 命令行模式

```bash
# 为整个解决方案生成配置
vscodegen --path ./src --output ./

# 只包含特定项目
vscodegen --include "WebApi,Services"

# 排除测试项目
vscodegen --exclude "*.Tests,*.Test"
```

### 示例 3: CI/CD 集成

```yaml
# GitHub Actions 示例
- name: Generate VSCode Config
  run: vscodegen --path ./src --output ./ --verbose
```

## 📊 技术统计

- **代码行数**: ~1500 行（v1.0: ~500）
- **文件数量**: 20+ 个（v1.0: 6 个）
- **接口数**: 4 个
- **服务类**: 4 个
- **文档页数**: 5 个

## 🔄 版本对比

| 特性 | v1.0 | v2.0 |
|------|------|------|
| 架构 | 单文件混合 | 企业级分层 ✅ |
| SOLID 原则 | ❌ | ✅ |
| 命令行参数 | ❌ | ✅ |
| 交互模式 | 基础 | 美化界面 ✅ |
| 配置持久化 | 文本文件 | JSON ✅ |
| 全局工具 | ❌ | ✅ |
| 文档 | 简单 | 完整 ✅ |
| 可维护性 | 低 | 高 ✅ |
| 可扩展性 | 低 | 高 ✅ |

## 💡 核心改进

### 代码质量
- ✅ 类型安全（sealed 类和 required 属性）
- ✅ 空引用安全（Nullable reference types）
- ✅ 完整的 XML 文档注释
- ✅ 统一的异常处理

### 用户体验
- ✅ 美化的终端输出
- ✅ 清晰的进度提示
- ✅ 友好的错误信息
- ✅ 完整的帮助系统

### 开发体验
- ✅ 清晰的项目结构
- ✅ 接口抽象设计
- ✅ 易于测试
- ✅ 易于扩展

## 🐛 Bug 修复

- 修复空 catch 块导致的错误吞没
- 修复配置文件读取的编码问题
- 修复相对路径计算错误
- 改进 XML 解析的异常处理

## ⚠️ 破坏性变更

### 配置文件格式
- 旧版本: `csproj_include_config.txt` (逗号分隔文本)
- 新版本: `vscodegen.config.json` (JSON 格式)
- **迁移**: 首次运行时自动迁移，无需手动操作

### 项目结构
- 代码已重构为分层架构
- 如果你基于 v1.0 进行了二次开发，需要适配新架构

## 📝 迁移指南

从 v1.0 升级到 v2.0：

1. **配置文件**: 自动迁移，无需操作
2. **安装**: 使用新的安装脚本
3. **使用**: 命令保持兼容，新增命令行参数可选使用

## 🔧 技术栈

- .NET 8.0
- C# 12
- System.Text.Json
- 无第三方依赖

## 🙏 致谢

感谢所有提出建议和反馈的用户！

## 📞 支持

- 📖 [完整文档](https://github.com/a985951420/C-ProjectToVsCodeDebugJson)
- 🐛 [提交 Issue](https://github.com/a985951420/C-ProjectToVsCodeDebugJson/issues)
- 💬 [讨论区](https://github.com/a985951420/C-ProjectToVsCodeDebugJson/discussions)

---

**如果觉得有用，请给个 ⭐️ Star 支持一下！**
