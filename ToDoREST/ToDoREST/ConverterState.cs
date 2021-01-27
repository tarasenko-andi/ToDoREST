using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ToDoREST
{
    public class ConverterState : IValueConverter
    {
        public object Convert(object _value, Type targetType, object _parameter, CultureInfo culture)
        {
            int value = (int)_value;
            string parameter = (string)_parameter;
            if (value == 0)
                return 0;
            if (value.ToString()[0] != parameter[0])
                return 0;
            else
                return  value;
        }
        public object ConvertBack(object _value, Type targetType, object parameter, CultureInfo culture)
        {
            return _value;
        }
    }
}
