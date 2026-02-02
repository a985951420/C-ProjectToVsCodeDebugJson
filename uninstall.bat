@echo off
REM VSCode Debug Generator - 卸载脚本

echo ========================================
echo VSCode Debug Generator 卸载脚本
echo ========================================
echo.

echo 正在卸载全局工具...
dotnet tool uninstall --global VsCodeDebugGen

if errorlevel 1 (
    echo 错误: 卸载失败或工具未安装
    pause
    exit /b 1
)

echo.
echo ========================================
echo ✓ 卸载完成！
echo ========================================
echo.
pause
