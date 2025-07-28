using System.Text;

/// <summary>
/// 主程序入口，负责用户交互和流程控制
/// </summary>
class Program
{
    /// <summary>
    /// 应用程序主入口
    /// </summary>
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("请输入要查找 .csproj 文件的目录路径（留空则使用当前目录）：");
        string basePath = Console.ReadLine();
        if (string.IsNullOrEmpty(basePath))
            basePath = Directory.GetCurrentDirectory();

        Console.WriteLine("请输入 .vscode 目录的保存路径（留空则使用当前目录）：");
        string vscodeSavePath = Console.ReadLine();
        if (string.IsNullOrEmpty(vscodeSavePath))
            vscodeSavePath = Directory.GetCurrentDirectory();

        Console.WriteLine("请输入要排除的 .csproj 文件名，多个文件名用逗号分隔（留空则不排除）：");
        string excludeInput = Console.ReadLine();
        var excludeList = excludeInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(name => name.Trim()).ToList();

        Console.WriteLine("请输入要生成的 .csproj 文件名，多个文件名用逗号分隔（留空则生成所有文件）：");
        string includeInput = Console.ReadLine();
        var includeList = includeInput.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(name => name.Trim()).ToList();

        Console.WriteLine("开始生成 VSCode 调试配置...");

        // 查找所有 .csproj 文件
        var csprojFiles = ProjectFinder.FindAllCsprojFiles(basePath);
        
        // 根据排除名单和只生成名单过滤 .csproj 文件
        csprojFiles = csprojFiles.Where(file => !excludeList.Contains(Path.GetFileName(file))).ToArray();
        if (includeList.Count > 0)
        {
            var allFileNames = new List<string>();

            foreach (var item in csprojFiles)
            {
                var fileName = Path.GetFileName(item);

                foreach (var includeItem in includeList)
                {
                    if (fileName.Contains(includeItem))
                    {
                        allFileNames.Add(item);

                        break;
                    }
                }
            }
            csprojFiles = allFileNames.ToArray();
        }

        if (csprojFiles.Length == 0)
        {
            Console.WriteLine("错误: 未找到 .csproj 文件");
            return;
        }

        // 解析所有项目文件
        var projectInfos = new List<ProjectInfo>();
        foreach (var csprojFile in csprojFiles)
        {
            var info = CsprojParser.Parse(csprojFile);
            if (info != null)
                projectInfos.Add(info);
        }

        // 生成 VSCode 配置文件
        var vscodeDir = VscodeConfigGenerator.CreateVscodeDirectory(vscodeSavePath);
        VscodeConfigGenerator.GenerateLaunchJson(vscodeDir, projectInfos, basePath);
        VscodeConfigGenerator.GenerateTasksJson(vscodeDir, projectInfos, basePath);

        Console.WriteLine($"VSCode 调试配置已成功生成到目录: {vscodeDir}");
    }
}