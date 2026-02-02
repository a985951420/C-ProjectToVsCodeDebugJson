@echo off
REM VSCode Debug Generator - 桌面应用打包脚本
REM 将桌面应用发布为独立的可执行文件

setlocal enabledelayedexpansion

echo ========================================
echo VSCode Debug Generator - 桌面应用打包
echo ========================================
echo.

REM 检查 .NET 是否安装
where dotnet >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo [错误] 未找到 .NET SDK，请先安装 .NET 8.0 或更高版本
    echo 下载地址: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

REM 设置项目路径
set DESKTOP_PROJECT=%~dp0src\VsCodeDebugGen.Desktop\VsCodeDebugGen.Desktop.csproj
set OUTPUT_DIR=%~dp0publish\desktop

REM 检查项目文件是否存在
if not exist "%DESKTOP_PROJECT%" (
    echo [错误] 未找到桌面应用项目文件: %DESKTOP_PROJECT%
    pause
    exit /b 1
)

echo [信息] 发布配置:
echo   - 项目: VsCodeDebugGen.Desktop
echo   - 输出目录: %OUTPUT_DIR%
echo   - 运行时: win-x64
echo   - 自包含: 是（包含 .NET 运行时）
echo   - 单文件: 是
echo.

REM 清理旧的发布文件
if exist "%OUTPUT_DIR%" (
    echo [信息] 清理旧的发布文件...
    rmdir /s /q "%OUTPUT_DIR%"
)

echo [信息] 开始发布桌面应用...
echo [提示] 这可能需要几分钟时间，请稍候...
echo.

REM 发布为单个可执行文件（自包含运行时）
dotnet publish "%DESKTOP_PROJECT%" ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    --output "%OUTPUT_DIR%" ^
    /p:PublishSingleFile=true ^
    /p:IncludeNativeLibrariesForSelfExtract=true ^
    /p:EnableCompressionInSingleFile=true ^
    /p:DebugType=None ^
    /p:DebugSymbols=false

if %ERRORLEVEL% neq 0 (
    echo.
    echo [错误] 桌面应用发布失败
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo ========================================
echo [完成] 桌面应用打包成功！
echo ========================================
echo.
echo 输出位置: %OUTPUT_DIR%
echo 可执行文件: VsCodeDebugGen.exe
echo.
echo 你可以将整个 publish\desktop 文件夹复制到任何 Windows 电脑上运行
echo （无需安装 .NET 运行时）
echo.

REM 打开输出目录
echo [提示] 按任意键打开输出目录...
pause >nul
explorer "%OUTPUT_DIR%"
