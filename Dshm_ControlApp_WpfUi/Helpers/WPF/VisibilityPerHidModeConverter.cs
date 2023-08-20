﻿using System;
using System.Globalization;
using Nefarius.DsHidMini.ControlApp.MVVM;
using System.Collections.Generic;

namespace Nefarius.DsHidMini.ControlApp.Util.WPF
{
    public class VisibilityPerHidModeConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int VisibilityPerHIDModeFlags = System.Convert.ToInt32((string)parameter, 2);
            SettingsContext context = (SettingsContext)value;
            int amountToBitShift = 0;

            switch (context)
            {
                case SettingsContext.SDF:
                    amountToBitShift = 0;
                    break;
                case SettingsContext.GPJ:
                    amountToBitShift = 1;
                    break;
                case SettingsContext.SXS:
                    amountToBitShift = 2;
                    break;
                case SettingsContext.DS4W:
                    amountToBitShift = 3;
                    break;
                case SettingsContext.XInput:
                    amountToBitShift = 4;
                    break;
                case SettingsContext.General:
                    amountToBitShift = 5;
                    break;
                case SettingsContext.Global:
                    amountToBitShift = 6;
                    break;
                default:
                    return false;
            }

            return (((VisibilityPerHIDModeFlags >> amountToBitShift) & 1U) == 1) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}