@echo off
REM VSCode Debug Generator - 主启动脚本
REM 选择启动 CLI 或桌面应用

setlocal enabledelayedexpansion

:menu
cls
echo ========================================
echo  VSCode Debug Generator
echo  C# 项目调试配置生成器
echo ========================================
echo.
echo 请选择启动模式:
echo.
echo  [1] CLI 命令行工具
echo  [2] 桌面应用 (GUI)
echo  [3] 构建桌面应用
echo  [4] 打包桌面应用 (生成独立可执行文件)
echo  [0] 退出
echo.
echo ========================================
set /p choice=请输入选项 (0-4):

if "%choice%"=="1" goto run_cli
if "%choice%"=="2" goto run_desktop
if "%choice%"=="3" goto build_desktop
if "%choice%"=="4" goto publish_desktop
if "%choice%"=="0" goto exit
echo.
echo [错误] 无效的选项，请重新选择
timeout /t 2 >nul
goto menu

:run_cli
cls
echo 正在启动 CLI 命令行工具...
echo.
call "%~dp0run-cli.bat"
goto menu

:run_desktop
cls
echo 正在启动桌面应用...
echo.
call "%~dp0run-desktop.bat"
goto menu

:build_desktop
cls
echo 正在构建桌面应用...
echo.
call "%~dp0build-desktop.bat"
goto menu

:publish_desktop
cls
echo 正在打包桌面应用...
echo.
call "%~dp0publish-desktop.bat"
goto menu

:exit
echo.
echo 感谢使用 VSCode Debug Generator！
echo.
exit /b 0
