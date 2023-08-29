using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.DshmConfiguration
{
    public class DshmManagerToDriverConversion
    {
        public static Dictionary<SettingsContext, DSHM_HidDeviceMode> DictHidDeviceModes = new()
        {
            {SettingsContext.Global , DSHM_HidDeviceMode.XInput},
            {SettingsContext.General , DSHM_HidDeviceMode.XInput},
            {SettingsContext.SDF , DSHM_HidDeviceMode.SDF},
            {SettingsContext.GPJ , DSHM_HidDeviceMode.GPJ},
            {SettingsContext.SXS , DSHM_HidDeviceMode.SXS},
            {SettingsContext.DS4W , DSHM_HidDeviceMode.DS4Windows},
            {SettingsContext.XInput , DSHM_HidDeviceMode.XInput},
        };

        //---------------------------------------------------- LEDsModes

        public static Dictionary<ControlApp_LEDsModes, DSHM_LEDsMode> LedModeManagerToDriver = new()
        {
            { ControlApp_LEDsModes.BatteryIndicatorPlayerIndex, DSHM_LEDsMode.BatteryIndicatorPlayerIndex },
            { ControlApp_LEDsModes.BatteryIndicatorBarGraph, DSHM_LEDsMode.BatteryIndicatorBarGraph },
            { ControlApp_LEDsModes.CustomStatic, DSHM_LEDsMode.CustomPattern },
            { ControlApp_LEDsModes.CustomPattern, DSHM_LEDsMode.CustomPattern },
        };

        //---------------------------------------------------- DPadModes

        public static Dictionary<ControlApp_DPADModes, DSHM_DPadExposureMode> DPadExposureModeManagerToDriver = new()
        {
            { ControlApp_DPADModes.Default, DSHM_DPadExposureMode.Default },
            { ControlApp_DPADModes.HAT, DSHM_DPadExposureMode.HAT },
            { ControlApp_DPADModes.Buttons, DSHM_DPadExposureMode.IndividualButtons },
        };

        //---------------------------------------------------- PressureModes

        public static Dictionary<ControlApp_DsPressureMode, DSHM_PressureMode> DsPressureModeManagerToDriver = new()
        {
            { ControlApp_DsPressureMode.Default, DSHM_PressureMode.Default },
            { ControlApp_DsPressureMode.Analogue, DSHM_PressureMode.Analogue },
            { ControlApp_DsPressureMode.Digital, DSHM_PressureMode.Digital },
        };

        public static Dictionary<Manager_Button, DSHM_Button> ButtonManagerToDriver = new()
        {
            { Manager_Button.None, DSHM_Button.None },
            { Manager_Button.PS, DSHM_Button.PS },
            { Manager_Button.START, DSHM_Button.START },
            { Manager_Button.SELECT, DSHM_Button.SELECT },
            { Manager_Button.R1, DSHM_Button.R1 },
            { Manager_Button.L1, DSHM_Button.L1 },
            { Manager_Button.R2, DSHM_Button.R2 },
            { Manager_Button.L2, DSHM_Button.L2 },
            { Manager_Button.R3, DSHM_Button.R3 },
            { Manager_Button.L3, DSHM_Button.L3 },
            { Manager_Button.Triangle, DSHM_Button.Triangle },
            { Manager_Button.Circle, DSHM_Button.Circle },
            { Manager_Button.Cross, DSHM_Button.Cross },
            { Manager_Button.Square, DSHM_Button.Square },
            { Manager_Button.Up, DSHM_Button.Up },
            { Manager_Button.Right, DSHM_Button.Right },
            { Manager_Button.Dowm, DSHM_Button.Dowm },
            { Manager_Button.Left, DSHM_Button.Left },
        };
    }
}
