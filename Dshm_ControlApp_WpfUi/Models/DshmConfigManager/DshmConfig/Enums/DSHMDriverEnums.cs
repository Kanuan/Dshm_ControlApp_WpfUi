using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nefarius.DsHidMini.ControlApp.MVVM;

namespace Nefarius.DsHidMini.ControlApp.DSHM_Settings
{
    public enum DSHM_HidDeviceMode
    {
        SDF,
        GPJ,
        SXS,
        DS4Windows,
        XInput,
    }

    public enum DSHM_PressureMode
    {
        Digital,
        Analogue,
        Default,
    }

    public enum DSHM_DPadExposureMode
    {
        HAT,
        IndividualButtons,
        Default,
    }

    public enum DSHM_LEDsMode
    {
        BatteryIndicatorPlayerIndex,
        BatteryIndicatorBarGraph,
        CustomPattern,
    }

    public enum DSHM_Button
    {
        None,
        PS,
        START,
        SELECT,
        R1,
        L1,
        R2,
        L2,
        R3,
        L3,
        Triangle,
        Circle,
        Cross,
        Square,
        Up,
        Right,
        Dowm,
        Left,
    }

    public enum DSHM_LEDsAuthority
    {
        Automatic,
        Driver,
        Application,
    }
}
