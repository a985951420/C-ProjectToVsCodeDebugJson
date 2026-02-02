using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace VsCodeDebugGen.Desktop.Converters;

/// <summary>
/// 布尔值到可见性转换器
/// </summary>
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// 转换布尔值为可见性
    /// </summary>
    /// <param name="value">布尔值</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="parameter">参数（"Inverse"表示反转）</param>
    /// <param name="culture">区域信息</param>
    /// <returns>可见性</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
            return false;

        // 检查是否需要反转
        var inverse = parameter is string str && str.Equals("Inverse", StringComparison.OrdinalIgnoreCase);

        return inverse ? !boolValue : boolValue;
    }

    /// <summary>
    /// 反向转换（从可见性到布尔值）
    /// </summary>
    /// <param name="value">可见性</param>
    /// <param name="targetType">目标类型</param>
    /// <param name="parameter">参数</param>
    /// <param name="culture">区域信息</param>
    /// <returns>布尔值</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
            return false;

        // 检查是否需要反转
        var inverse = parameter is string str && str.Equals("Inverse", StringComparison.OrdinalIgnoreCase);

        return inverse ? !boolValue : boolValue;
    }
}
