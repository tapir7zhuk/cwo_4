using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DrugStoreStatistics.Converters;

public class NoImageVisConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var image = values[0];
        var isLoading = values[1] is bool b && b;

        // Показуємо "Фото відсутнє" лише коли не завантажується і зображення немає
        return image == null && !isLoading
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}