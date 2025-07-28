/// <summary>
/// 查找 .csproj 文件工具类
/// </summary>
static class ProjectFinder
{
    /// <summary>
    /// 查找指定目录及子目录下所有 .csproj 文件
    /// </summary>
    /// <param name="searchPath">要查找的根目录</param>
    /// <returns>所有找到的 .csproj 文件路径数组</returns>
    public static string[] FindAllCsprojFiles(string searchPath)
    {
        return Directory.GetFiles(searchPath, "*.csproj", SearchOption.AllDirectories);
    }
}