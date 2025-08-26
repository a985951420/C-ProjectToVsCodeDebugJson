/// <summary>
/// launch.json 配置结构体
/// </summary>
internal class LaunchConfig
{
    public string version { get; set; }
    public LaunchConfiguration[] configurations { get; set; }

    public LaunchConfig()
    {
        version = "0.2.0";
        configurations = System.Array.Empty<LaunchConfiguration>();
    }
}

internal class LaunchConfiguration
{
    public required string name { get; set; }
    public string type { get; set; }
    public string request { get; set; }
    public string preLaunchTask { get; set; }
    public required string program { get; set; }
    public string[] args { get; set; }
    public string cwd { get; set; }
    public LaunchBrowser launchBrowser { get; set; }
    public Dictionary<string, string> env { get; set; }
    public Dictionary<string, string> sourceFileMap { get; set; }

    public LaunchConfiguration()
    {
        type = "coreclr";
        request = "launch";
        preLaunchTask = "build";
        args = System.Array.Empty<string>();
        cwd = "${workspaceFolder}";
        launchBrowser = new LaunchBrowser();
        env = new Dictionary<string, string> { { "ASPNETCORE_ENVIRONMENT", "Development" } };
        sourceFileMap = new Dictionary<string, string> { { "/Views", "${workspaceFolder}/Views" } };
    }
}

internal class LaunchBrowser
{
    public bool enabled { get; set; }
    public string args { get; set; }
    public WindowsCommand windows { get; set; }
    public OsxCommand osx { get; set; }
    public LinuxCommand linux { get; set; }

    public LaunchBrowser()
    {
        enabled = true;
        args = "${auto-detect-url}";
        windows = new WindowsCommand();
        osx = new OsxCommand();
        linux = new LinuxCommand();
    }
}

internal class WindowsCommand 
{
    public string command { get; set; }
    public string args { get; set; }

    public WindowsCommand()
    {
        command = "cmd.exe";
        args = "/C start ${auto-detect-url}";
    }
}
internal class OsxCommand 
{
    public string command { get; set; }

    public OsxCommand()
    {
        command = "open";
    }
}
internal class LinuxCommand 
{
    public string command { get; set; }

    public LinuxCommand()
    {
        command = "xdg-open";
    }
}

/// <summary>
/// tasks.json 配置结构体
/// </summary>
internal class TasksConfig
{
    public string version { get; set; }
    public TaskItem[] tasks { get; set; }

    public TasksConfig()
    {
        version = "2.0.0";
        tasks = System.Array.Empty<TaskItem>();
    }
}

internal class TaskItem
{
    public string label { get; set; }
    public string command { get; set; }
    public string type { get; set; }
    public string[] args { get; set; }
    public string problemMatcher { get; set; }

    public TaskItem()
    {
        label = "build";
        command = "dotnet";
        type = "process";
        args = System.Array.Empty<string>();
        problemMatcher = "${msCompile}";
    }
}