/// <summary>
/// launch.json 配置结构体
/// </summary>
internal class LaunchConfig
{
    public string version { get; set; } = "0.2.0";
    public LaunchConfiguration[] configurations { get; set; } = System.Array.Empty<LaunchConfiguration>();
}

internal class LaunchConfiguration
{
    public required string name { get; set; }
    public string type { get; set; } = "coreclr";
    public string request { get; set; } = "launch";
    public string preLaunchTask { get; set; } = "build";
    public required string program { get; set; }
    public string[] args { get; set; } = System.Array.Empty<string>();
    public string cwd { get; set; } = "${workspaceFolder}";
    public LaunchBrowser launchBrowser { get; set; } = new LaunchBrowser();
    public Dictionary<string, string> env { get; set; } = new Dictionary<string, string> { { "ASPNETCORE_ENVIRONMENT", "Development" } };
    public Dictionary<string, string> sourceFileMap { get; set; } = new Dictionary<string, string> { { "/Views", "${workspaceFolder}/Views" } };
}

internal class LaunchBrowser
{
    public bool enabled { get; set; } = true;
    public string args { get; set; } = "${auto-detect-url}";
    public WindowsCommand windows { get; set; } = new WindowsCommand();
    public OsxCommand osx { get; set; } = new OsxCommand();
    public LinuxCommand linux { get; set; } = new LinuxCommand();
}

internal class WindowsCommand { public string command { get; set; } = "cmd.exe"; public string args { get; set; } = "/C start ${auto-detect-url}"; }
internal class OsxCommand { public string command { get; set; } = "open"; }
internal class LinuxCommand { public string command { get; set; } = "xdg-open"; }

/// <summary>
/// tasks.json 配置结构体
/// </summary>
internal class TasksConfig
{
    public string version { get; set; } = "2.0.0";
    public TaskItem[] tasks { get; set; } = System.Array.Empty<TaskItem>();
}

internal class TaskItem
{
    public string label { get; set; } = "build";
    public string command { get; set; } = "dotnet";
    public string type { get; set; } = "process";
    public string[] args { get; set; } = System.Array.Empty<string>();
    public string problemMatcher { get; set; } = "${msCompile}";
}