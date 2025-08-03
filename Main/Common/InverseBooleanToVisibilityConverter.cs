using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Main.Common
{
    /// <summary>
    /// 反轉BOOL並轉換成Visibility列舉
    /// </summary>
    internal class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibilityValue)
            {
                return visibilityValue == Visibility.Collapsed;
            }
            return false;
        }
    }
}
