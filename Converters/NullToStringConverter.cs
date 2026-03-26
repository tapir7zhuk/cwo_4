using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;

namespace DrugStoreStatistics.Converters;

public class NullToStringConverter : IValueConverter
{
    public string NullText { get; set; } = "Додати";
    public string NotNullText { get; set; } = "Редагувати";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value == null ? NullText : NotNullText;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}