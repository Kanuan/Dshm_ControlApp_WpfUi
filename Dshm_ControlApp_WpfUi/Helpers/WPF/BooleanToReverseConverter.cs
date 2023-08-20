using System;
using System.Globalization;
using System.Collections.Generic;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Serilog.Formatting.Display;
using System.Windows.Data;

namespace Nefarius.DsHidMini.ControlApp.Util.WPF
{
    public class BooleanToReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return !(bool?)value ?? true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
         => !(value as bool?);
    }
}