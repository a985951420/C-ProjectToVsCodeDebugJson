# C-ProjectToVsCodeDebugJson
C# 项目 生成 VsCode  调试 JSON 文件

# 打包单个文件
dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:PublishTrimmed=true