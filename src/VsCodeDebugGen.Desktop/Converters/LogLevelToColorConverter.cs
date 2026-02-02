using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using VsCodeDebugGen.Desktop.Models;

namespace VsCodeDebugGen.Desktop.Converters;

/// <summary>
/// 日志级别到颜色转换器
/// </summary>
public class LogLevelToColorConverter : IValueConverter
{
    /// <summary>
    /// 转换日志级别为颜色
    /// </summary>
    /// <param name="value">日志级别</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="parameter">参数</param>
    /// <param name="culture">区域信息</param>
    /// <returns>颜色画刷</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not LogLevel level)
            return new SolidColorBrush(Colors.Black);

        return level switch
        {
            LogLevel.Debug => new SolidColorBrush(Color.Parse("#605E5C")),    // 灰色
            LogLevel.Info => new SolidColorBrush(Color.Parse("#0078D4")),     // 蓝色
            LogLevel.Success => new SolidColorBrush(Color.Parse("#107C10")),  // 绿色
            LogLevel.Warning => new SolidColorBrush(Color.Parse("#FF8C00")),  // 橙色
            LogLevel.Error => new SolidColorBrush(Color.Parse("#D13438")),    // 红色
            _ => new SolidColorBrush(Colors.Black)
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
