#!/bin/bash
# VSCode Debug Generator - 安装脚本
# 此脚本将打包并安装工具到全局环境

echo "========================================"
echo "VSCode Debug Generator 安装脚本"
echo "========================================"
echo ""

# 检查 dotnet 是否安装
if ! command -v dotnet &> /dev/null; then
    echo "错误: 未检测到 .NET SDK"
    echo "请先安装 .NET SDK 8.0 或更高版本"
    echo "下载地址: https://dotnet.microsoft.com/download"
    exit 1
fi

echo "[1/4] 清理旧的构建文件..."
dotnet clean -c Release > /dev/null 2>&1

echo "[2/4] 打包项目..."
dotnet pack -c Release -o ./nupkg
if [ $? -ne 0 ]; then
    echo "错误: 打包失败"
    exit 1
fi

echo "[3/4] 卸载旧版本（如果存在）..."
dotnet tool uninstall --global VsCodeDebugGen > /dev/null 2>&1

echo "[4/4] 安装工具到全局环境..."
dotnet tool install --global --add-source ./nupkg VsCodeDebugGen
if [ $? -ne 0 ]; then
    echo "错误: 安装失败"
    exit 1
fi

echo ""
echo "========================================"
echo "✓ 安装完成！"
echo "========================================"
echo ""
echo "现在你可以在任何地方使用以下命令:"
echo "  vscodegen --help"
echo "  vscodegen"
echo ""
