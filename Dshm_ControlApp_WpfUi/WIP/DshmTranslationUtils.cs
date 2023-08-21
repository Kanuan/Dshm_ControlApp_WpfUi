﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.DSHM_Settings
{
    public class SaveLoadUtils
    {
        public static Dictionary<SettingsContext, DSHM_HidDeviceModes> Get_DSHM_HIDDeviceMode_From_ControlApp = new()
        {
            {SettingsContext.Global , DSHM_HidDeviceModes.XInput},
            {SettingsContext.General , DSHM_HidDeviceModes.XInput},
            {SettingsContext.SDF , DSHM_HidDeviceModes.SDF},
            {SettingsContext.GPJ , DSHM_HidDeviceModes.GPJ},
            {SettingsContext.SXS , DSHM_HidDeviceModes.SXS},
            {SettingsContext.DS4W , DSHM_HidDeviceModes.DS4Windows},
            {SettingsContext.XInput , DSHM_HidDeviceModes.XInput},
        };

        //---------------------------------------------------- LEDsModes

        public static Dictionary<ControlApp_LEDsModes, DSHM_LEDsModes> Get_DSHM_LEDModes_From_ControlApp = new()
        {
            { ControlApp_LEDsModes.BatteryIndicatorPlayerIndex, DSHM_LEDsModes.BatteryIndicatorPlayerIndex },
            { ControlApp_LEDsModes.BatteryIndicatorBarGraph, DSHM_LEDsModes.BatteryIndicatorBarGraph },
            { ControlApp_LEDsModes.CustomStatic, DSHM_LEDsModes.CustomPattern },
            { ControlApp_LEDsModes.CustomPattern, DSHM_LEDsModes.CustomPattern },
        };

        public static Dictionary<DSHM_LEDsModes, ControlApp_LEDsModes> Get_ControlApp_LEDModes_From_DSHM = new()
        {
            { DSHM_LEDsModes.BatteryIndicatorPlayerIndex, ControlApp_LEDsModes.BatteryIndicatorPlayerIndex },
            { DSHM_LEDsModes.BatteryIndicatorBarGraph, ControlApp_LEDsModes.BatteryIndicatorBarGraph },
            { DSHM_LEDsModes.CustomPattern, ControlApp_LEDsModes.CustomPattern },
        };

        //---------------------------------------------------- DPadModes

        public static Dictionary<ControlApp_DPADModes, DSHM_DPadExposureModes> Get_DSHM_DPadMode_From_ControlApp = new()
        {
            { ControlApp_DPADModes.Default, DSHM_DPadExposureModes.Default },
            { ControlApp_DPADModes.HAT, DSHM_DPadExposureModes.HAT },
            { ControlApp_DPADModes.Buttons, DSHM_DPadExposureModes.IndividualButtons },
        };

        public static Dictionary<DSHM_DPadExposureModes, ControlApp_DPADModes> Get_ControlApp_DPadMode_From_DSHM = new()
        {
            { DSHM_DPadExposureModes.Default, ControlApp_DPADModes.Default },
            { DSHM_DPadExposureModes.HAT, ControlApp_DPADModes.HAT },
            { DSHM_DPadExposureModes.IndividualButtons, ControlApp_DPADModes.Buttons },
        };

        //---------------------------------------------------- PressureModes

        public static Dictionary<ControlApp_DsPressureMode, DSHM_PressureModes> Get_DSHM_DsPressureMode_From_ControlApp = new()
        {
            { ControlApp_DsPressureMode.Default, DSHM_PressureModes.Default },
            { ControlApp_DsPressureMode.Analogue, DSHM_PressureModes.Analogue },
            { ControlApp_DsPressureMode.Digital, DSHM_PressureModes.Digital },
        };

        public static Dictionary<DSHM_PressureModes, ControlApp_DsPressureMode> Get_ControlApp_DsPressureMode_From_DSHM = new()
        {
            { DSHM_PressureModes.Default, ControlApp_DsPressureMode.Default },
            { DSHM_PressureModes.Analogue, ControlApp_DsPressureMode.Analogue },
            { DSHM_PressureModes.Digital, ControlApp_DsPressureMode.Digital },
        };
    }
}
