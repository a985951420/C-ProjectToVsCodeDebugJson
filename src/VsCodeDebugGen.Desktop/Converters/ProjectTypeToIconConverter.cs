using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace VsCodeDebugGen.Desktop.Converters;

/// <summary>
/// 项目类型到图标转换器
/// </summary>
public class ProjectTypeToIconConverter : IValueConverter
{
    /// <summary>
    /// 转换项目类型为图标字符
    /// </summary>
    /// <param name="value">项目类型</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="parameter">参数</param>
    /// <param name="culture">区域信息</param>
    /// <returns>图标字符</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string projectType)
            return "📄";  // 默认文件图标

        return projectType.ToLowerInvariant() switch
        {
            "web" => "🌐",          // Web 项目
            "webapi" => "🔌",       // Web API 项目
            "console" => "⚙️",      // 控制台项目
            "classlib" => "📚",     // 类库项目
            "test" => "🧪",         // 测试项目
            "blazor" => "⚡",       // Blazor 项目
            "wpf" => "🖥️",         // WPF 项目
            "winforms" => "📐",     // WinForms 项目
            "maui" => "📱",         // MAUI 项目
            "worker" => "🔄",       // Worker 服务
            "grpc" => "🔗",         // gRPC 服务
            _ => "📄"              // 其他类型
        };
    }

    /// <summary>
    /// 反向转换（不支持）
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
