using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    public class ButtonsCombo
    {

        private ControlApp_ComboButtons button1;
        private ControlApp_ComboButtons button2;
        private ControlApp_ComboButtons button3;

        public ControlApp_ComboButtons Button1
        {
            get => button1;
            set
            {
                if (value != button2 && value != button3)
                    button1 = value;
            }
        }
        public ControlApp_ComboButtons Button2
        {
            get => button2;
            set
            {
                if (value != button1 && value != button3)
                    button2 = value;
            }
        }
        public ControlApp_ComboButtons Button3
        {
            get => button3;
            set
            {
                if (value != button1 && value != button2)
                    button3 = value;
            }
        }

        public ButtonsCombo() { }

        public ButtonsCombo(ButtonsCombo comboToCopy)
        {
            copyCombo(comboToCopy);
        }

        public void copyCombo(ButtonsCombo comboToCopy)
        {
            Button1 = comboToCopy.Button1;
            Button2 = comboToCopy.Button2;
            Button3 = comboToCopy.Button3;
        }

    }
    
    public class BackingDataContainer : IBackingData
    {

        public BackingData_ModesUnique modesUniqueData { get; set; } = new();
        public BackingData_LEDs ledsData { get; set; } = new();
        public BackingData_Wireless wirelessData { get; set; } = new();
        public BackingData_Sticks sticksData { get; set; } = new();
        public BackingData_RumbleGeneral rumbleGeneralData { get; set; } = new();
        public BackingData_OutRepControl outRepData { get; set; } = new();
        public BackingData_LeftRumbleRescale leftRumbleRescaleData { get; set; } = new();
        public BackingData_VariablaRightRumbleEmulAdjusts rightVariableEmulData { get; set; } = new();

        public static readonly BackingDataContainer DefaultContainer = new BackingDataContainer();

        public BackingDataContainer()
        {
            this.ResetToDefault();
        }

        public void ResetToDefault()
        {
            modesUniqueData.ResetToDefault();
            ledsData.ResetToDefault();
            wirelessData.ResetToDefault();
            sticksData.ResetToDefault();
            rumbleGeneralData.ResetToDefault();
            outRepData.ResetToDefault();
            leftRumbleRescaleData.ResetToDefault();
            rightVariableEmulData.ResetToDefault();
        }

        public void CopySettingsFromContainer(BackingDataContainer container)
        {
            modesUniqueData.CopySettingsFromContainer(container);
            ledsData.CopySettingsFromContainer(container);
            wirelessData.CopySettingsFromContainer(container);
            sticksData.CopySettingsFromContainer(container);
            rumbleGeneralData.CopySettingsFromContainer(container);
            outRepData.CopySettingsFromContainer(container);
            leftRumbleRescaleData.CopySettingsFromContainer(container);
            rightVariableEmulData.CopySettingsFromContainer(container);
        }

        public void CopySettingsToContainer(BackingDataContainer container)
        {
            modesUniqueData.CopySettingsToContainer(container);
            ledsData.CopySettingsToContainer(container);
            wirelessData.CopySettingsToContainer(container);
            sticksData.CopySettingsToContainer(container);
            rumbleGeneralData.CopySettingsToContainer(container);
            outRepData.CopySettingsToContainer(container);
            leftRumbleRescaleData.CopySettingsToContainer(container);
            rightVariableEmulData.CopySettingsToContainer(container);
        }

        public void ConvertAllToDSHM(DshmCustomSettings dshm_data)
        {
            modesUniqueData.SaveToDSHMSettings(dshm_data);
            ledsData.SaveToDSHMSettings(dshm_data);
            wirelessData.SaveToDSHMSettings(dshm_data);
            sticksData.SaveToDSHMSettings(dshm_data);
            rumbleGeneralData.SaveToDSHMSettings(dshm_data);
            outRepData.SaveToDSHMSettings(dshm_data);
            leftRumbleRescaleData.SaveToDSHMSettings(dshm_data);
            rightVariableEmulData.SaveToDSHMSettings(dshm_data);

            if (modesUniqueData.SettingsContext == SettingsContext.DS4W)
            {
                dshm_data.ContextSettings.DeadZoneLeft.Apply = false;
                dshm_data.ContextSettings.DeadZoneRight.Apply = false;
            }
        }
    }

    public interface IBackingData
    {
        void ResetToDefault();

        void CopySettingsFromContainer(BackingDataContainer container);

        void CopySettingsToContainer(BackingDataContainer container);
    }

    public abstract class SettingsBackingData : IBackingData
    {
        public abstract void ResetToDefault();

        public abstract void CopySettingsFromContainer(BackingDataContainer container);

        public abstract void CopySettingsToContainer(BackingDataContainer container);

        public abstract void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings);

        
    }

    public class BackingData_ModesUnique : SettingsBackingData
    {
        public SettingsContext SettingsContext { get; set; } = SettingsContext.XInput;
        public ControlApp_DsPressureMode PressureExposureMode { get; set; } = ControlApp_DsPressureMode.Default;
        public ControlApp_DPADModes DPadExposureMode { get; set; } = ControlApp_DPADModes.HAT;
        public bool IsLEDsAsXInputSlotEnabled { get; set; } = false;
        public bool PreventRemappingConflictsInSXSMode { get; set; } = false;
        public bool PreventRemappingConflictsInDS4WMode { get; set; } = true;
        public bool IsDS4LightbarTranslationEnabled { get; set; } = false;
        public bool AllowAppsToOverrideLEDsInSXSMode { get; set; } = false;

        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_ModesUnique destiny, BackingData_ModesUnique source)
        {
            destiny.SettingsContext = source.SettingsContext;
            destiny.PressureExposureMode = source.PressureExposureMode;
            destiny.DPadExposureMode = source.DPadExposureMode;
            destiny.IsLEDsAsXInputSlotEnabled = source.IsLEDsAsXInputSlotEnabled;
            destiny.IsDS4LightbarTranslationEnabled = source.IsDS4LightbarTranslationEnabled;
            destiny.PreventRemappingConflictsInSXSMode = source.PreventRemappingConflictsInSXSMode;
            destiny.PreventRemappingConflictsInDS4WMode = source.PreventRemappingConflictsInDS4WMode;
            destiny.AllowAppsToOverrideLEDsInSXSMode = source.AllowAppsToOverrideLEDsInSXSMode;
        }


        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            if (SettingsContext != SettingsContext.General)
            {
                dshmContextSettings.HIDDeviceMode = SaveLoadUtils.Get_DSHM_HIDDeviceMode_From_ControlApp[SettingsContext];
                dshmContextSettings.ContextSettings.HIDDeviceMode = dshmContextSettings.HIDDeviceMode;
            }

            dshmContextSettings.ContextSettings.PressureExposureMode = dshmContextSettings.ContextSettings.PressureExposureMode =
                (this.SettingsContext == SettingsContext.SDF
                || this.SettingsContext == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DsPressureMode_From_ControlApp[this.PressureExposureMode] : null;

            dshmContextSettings.ContextSettings.DPadExposureMode = dshmContextSettings.ContextSettings.DPadExposureMode =
                (this.SettingsContext == SettingsContext.SDF
                || this.SettingsContext == SettingsContext.GPJ)
                ? SaveLoadUtils.Get_DSHM_DPadMode_From_ControlApp[this.DPadExposureMode] : null;

            dshmContextSettings.ContextSettings.LEDSettings.Authority = this.AllowAppsToOverrideLEDsInSXSMode ? DSHM_LEDsAuthority.Application : DSHM_LEDsAuthority.Driver;
            dshmContextSettings.ContextSettings.LEDSettings.Authority = this.IsDS4LightbarTranslationEnabled ? DSHM_LEDsAuthority.Application : DSHM_LEDsAuthority.Driver;
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.modesUniqueData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.modesUniqueData, this); 
        }
    }

    public class BackingData_LEDs : SettingsBackingData
    {
        public ControlApp_LEDsModes LEDMode { get; set; } = ControlApp_LEDsModes.BatteryIndicatorPlayerIndex;
        public All4LEDsCustoms LEDsCustoms { get; set; } = new();


        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_LEDs destiny, BackingData_LEDs source)
        {
            destiny.LEDMode = source.LEDMode;
            destiny.LEDsCustoms.CopyLEDsCustoms(source.LEDsCustoms);
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.ledsData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.ledsData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            DshmCustomSettings.AllLEDSettings dshm_AllLEDsSettings = dshmContextSettings.ContextSettings.LEDSettings;

            dshm_AllLEDsSettings.Mode = SaveLoadUtils.Get_DSHM_LEDModes_From_ControlApp[this.LEDMode];

            var dshm_Customs = dshm_AllLEDsSettings.CustomPatterns;

            var dshm_singleLED = new DshmCustomSettings.SingleLEDCustoms[]
            { dshm_Customs.Player1, dshm_Customs.Player2,dshm_Customs.Player3,dshm_Customs.Player4, };


            dshm_Customs.LEDFlags = 0;
            for (int i = 0; i < LEDsCustoms.LED_x_Customs.Length; i++)
            {
                All4LEDsCustoms.singleLEDCustoms singleLEDCustoms = this.LEDsCustoms.LED_x_Customs[i];

                if (singleLEDCustoms.IsLedEnabled)
                {
                    dshm_Customs.LEDFlags |= (byte)(1 << (1 + i));
                }

                switch (this.LEDMode)
                {
                    case ControlApp_LEDsModes.CustomPattern:
                        dshm_singleLED[i].EnabledFlags = singleLEDCustoms.UseLEDEffects ? (byte)0x10 : (byte)0x00;
                        dshm_singleLED[i].Duration = singleLEDCustoms.Duration;
                        dshm_singleLED[i].IntervalDuration = (byte)singleLEDCustoms.IntervalDuration; // FIX THIS
                        dshm_singleLED[i].IntervalPortionOn = singleLEDCustoms.IntervalPortionON;
                        dshm_singleLED[i].IntervalPortionOff = singleLEDCustoms.IntervalPortionOFF;
                        break;
                    case ControlApp_LEDsModes.CustomStatic:
                        dshm_singleLED[i].EnabledFlags = 0x10; // false
                        /*
                        dshm_singleLED[i].Duration = null;
                        dshm_singleLED[i].IntervalDuration = null;
                        dshm_singleLED[i].IntervalPortionOn = null;
                        dshm_singleLED[i].IntervalPortionOff = null;
                        */
                        break;
                    case ControlApp_LEDsModes.BatteryIndicatorPlayerIndex:
                    case ControlApp_LEDsModes.BatteryIndicatorBarGraph:
                    default:
                        dshm_singleLED[i] = null;
                        break;
                }
            }
        }

        public class All4LEDsCustoms
        {
            public singleLEDCustoms[] LED_x_Customs = new singleLEDCustoms[4];
            public All4LEDsCustoms()
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i] = new(i);
                }
            }

            public void CopyLEDsCustoms(All4LEDsCustoms customsToCopy)
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i].CopyCustoms(customsToCopy.LED_x_Customs[i]);
                }
            }

            public void ResetLEDsCustoms()
            {
                for (int i = 0; i < LED_x_Customs.Length; i++)
                {
                    LED_x_Customs[i].Reset();
                }
            }



            public class singleLEDCustoms
            {
                [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
                public int LEDIndex { get; }
                public bool IsLedEnabled { get; set; } = false;
                public bool UseLEDEffects { get; set; } = false;

                public byte Duration { get; set; } = 0x00;
                public int IntervalDuration { get; set; } = 0x4000;
                public byte IntervalPortionON { get; set; } = 0xFF;
                public byte IntervalPortionOFF { get; set; } = 0xFF;
                public singleLEDCustoms(int ledIndex)
                {
                    this.LEDIndex = ledIndex;
                    IsLedEnabled = LEDIndex == 0 ? true : false;
                }

                internal void Reset()
                {
                    CopyCustoms(new(LEDIndex));
                }

                public void CopyCustoms(singleLEDCustoms copySource)
                {
                    this.IsLedEnabled = copySource.IsLedEnabled;
                    this.UseLEDEffects = copySource.UseLEDEffects;
                    this.Duration = copySource.Duration;
                    this.IntervalDuration = copySource.IntervalDuration;
                    this.IntervalPortionON = copySource.IntervalPortionON;
                }
            }
        }

    }

    public class BackingData_Wireless : SettingsBackingData
    {
        public bool IsWirelessIdleDisconnectEnabled { get; set; } = true;
        public int WirelessIdleDisconnectTime { get; set; } = 5;
        public bool IsQuickDisconnectComboEnabled { get; set; } = true;
        public ButtonsCombo QuickDisconnectCombo { get; set; } = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.R1,
            Button3 = ControlApp_ComboButtons.L1,
        };

        public override void ResetToDefault()
        {
            CopySettings(this,new());
        }

        public static void CopySettings(BackingData_Wireless destiny, BackingData_Wireless source)
        {
            destiny.IsQuickDisconnectComboEnabled = source.IsQuickDisconnectComboEnabled;
            destiny.IsWirelessIdleDisconnectEnabled = source.IsWirelessIdleDisconnectEnabled;
            destiny.QuickDisconnectCombo.copyCombo(source.QuickDisconnectCombo);
            destiny.WirelessIdleDisconnectTime = source.WirelessIdleDisconnectTime;
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.wirelessData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.wirelessData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            dshmContextSettings.DisableWirelessIdleTimeout = !this.IsWirelessIdleDisconnectEnabled;
            dshmContextSettings.WirelessIdleTimeoutPeriodMs = this.WirelessIdleDisconnectTime * 60 * 1000;
            //dshmContextSettings.QuickDisconnectCombo = dictionary combo pair;
        }
    }

    public class BackingData_Sticks : SettingsBackingData
    {
        public StickData LeftStickData { get; set; } = new();
        public StickData RightStickData { get; set; } = new();

        public override void ResetToDefault()
        {
            LeftStickData.Reset();
            RightStickData.Reset();
        }

        public static void CopySettings(BackingData_Sticks destiny, BackingData_Sticks source)
        {
            destiny.LeftStickData.CopyStickDataFromOtherStick(source.LeftStickData);
            destiny.RightStickData.CopyStickDataFromOtherStick(source.RightStickData);
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.sticksData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.sticksData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            DshmCustomSettings.DeadZoneSettings dshmLeftDZSettings = dshmContextSettings.ContextSettings.DeadZoneLeft;
            DshmCustomSettings.DeadZoneSettings dshmRightDZSettings = dshmContextSettings.ContextSettings.DeadZoneRight;
            DshmCustomSettings.AxesFlipping axesFlipping = dshmContextSettings.ContextSettings.FlipAxis;


            dshmLeftDZSettings.Apply = this.LeftStickData.IsDeadZoneEnabled;
            dshmLeftDZSettings.PolarValue = (byte)(this.LeftStickData.DeadZone * 181 / 142);
            axesFlipping.LeftX = this.LeftStickData.InvertXAxis;
            axesFlipping.LeftY = this.LeftStickData.InvertYAxis;

            dshmRightDZSettings.Apply = this.RightStickData.IsDeadZoneEnabled;
            dshmRightDZSettings.PolarValue = (byte)(this.RightStickData.DeadZone * 181 / 142);
            axesFlipping.RightX = this.RightStickData.InvertXAxis;
            axesFlipping.RightY = this.RightStickData.InvertYAxis;
        }

        public class StickData
        {
            public bool IsDeadZoneEnabled { get; set; } = true;
            public int DeadZone { get; set; } = 0;

            public bool InvertXAxis { get; set; } = false;
            public bool InvertYAxis { get; set; } = false;

            public StickData()
            {
               
            }

            public void Reset()
            {
                CopyStickDataFromOtherStick(new());
            }

            public void CopyStickDataFromOtherStick(StickData copySource)
            {
                this.IsDeadZoneEnabled = copySource.IsDeadZoneEnabled;
                this.DeadZone = copySource.DeadZone;
                this.InvertXAxis = copySource.InvertXAxis;
                this.InvertYAxis = copySource.InvertYAxis;
            }

        }

    }

    public class BackingData_RumbleGeneral : SettingsBackingData
    {

        // -------------------------------------------- DEFAULT SETTINGS END

        private bool isLeftMotorDisabled = false;
        private bool isRightMotorDisabled = false;

        public bool IsLeftMotorDisabled { get => IsAltRumbleModeEnabled ? false : isLeftMotorDisabled; set => isLeftMotorDisabled = value; }
        public bool IsRightMotorDisabled { get => IsAltRumbleModeEnabled ? false : isRightMotorDisabled; set => isRightMotorDisabled = value; }

        public bool IsAltRumbleModeEnabled { get; set; } = false;
        public bool AlwaysStartInNormalMode { get; set; } = false;
        public bool IsAltModeToggleButtonComboEnabled { get; set; } = false;
        public ButtonsCombo AltModeToggleButtonCombo { get; set; } = new()
        {
            Button1 = ControlApp_ComboButtons.PS,
            Button2 = ControlApp_ComboButtons.SELECT,
            Button3 = ControlApp_ComboButtons.None,
        };

        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_RumbleGeneral destiny, BackingData_RumbleGeneral source)
        {
            destiny.IsAltRumbleModeEnabled = source.IsAltRumbleModeEnabled;
            destiny.IsLeftMotorDisabled = source.IsLeftMotorDisabled;
            destiny.IsRightMotorDisabled = source.IsRightMotorDisabled;
            destiny.AlwaysStartInNormalMode = source.IsAltModeToggleButtonComboEnabled;
            destiny.AltModeToggleButtonCombo.copyCombo(source.AltModeToggleButtonCombo);
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.rumbleGeneralData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.rumbleGeneralData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            DshmCustomSettings.AllRumbleSettings dshmRumbleSettings = dshmContextSettings.ContextSettings.RumbleSettings;

            dshmRumbleSettings.DisableBM = this.IsLeftMotorDisabled;
            dshmRumbleSettings.DisableSM = this.IsLeftMotorDisabled;

            dshmRumbleSettings.SMToBMConversion.Enabled = AlwaysStartInNormalMode ? false : this.IsAltRumbleModeEnabled;
            dshmRumbleSettings.SMToBMConversion.IsSMToBMConversionToggleComboEnabled = IsAltRumbleModeEnabled ? this.IsAltModeToggleButtonComboEnabled : false;
            // button combo


        }
    }

    public class BackingData_OutRepControl : SettingsBackingData
    {
        public bool IsOutputReportRateControlEnabled { get; set; } = true;
        public int MaxOutputRate { get; set; } = 150;
        public bool IsOutputReportDeduplicatorEnabled { get; set; } = false;

        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_OutRepControl destiny, BackingData_OutRepControl source)
        {
            destiny.IsOutputReportDeduplicatorEnabled = source.IsOutputReportDeduplicatorEnabled;
            destiny.IsOutputReportRateControlEnabled = source.IsOutputReportRateControlEnabled;
            destiny.MaxOutputRate = source.MaxOutputRate;
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.outRepData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.outRepData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            dshmContextSettings.IsOutputRateControlEnabled = this.IsOutputReportRateControlEnabled;
            dshmContextSettings.OutputRateControlPeriodMs = (byte)this.MaxOutputRate;
            dshmContextSettings.IsOutputDeduplicatorEnabled = this.IsOutputReportDeduplicatorEnabled;
        }
    }

    public class BackingData_LeftRumbleRescale : SettingsBackingData
    {
        private int leftMotorStrRescalingUpperRange = 255;
        private int leftMotorStrRescalingLowerRange = 64;

        public bool IsLeftMotorStrRescalingEnabled { get; set; } = true;
        public int LeftMotorStrRescalingUpperRange
        {
            get => leftMotorStrRescalingUpperRange;
            set
            {
                int tempInt = (value < leftMotorStrRescalingLowerRange) ? leftMotorStrRescalingLowerRange + 1 : value;
                leftMotorStrRescalingUpperRange = tempInt;

            }
        }
        public int LeftMotorStrRescalingLowerRange
        {
            get => leftMotorStrRescalingLowerRange;
            set
            {
                int tempInt = (value > leftMotorStrRescalingUpperRange) ? leftMotorStrRescalingUpperRange - 1 : value;
                leftMotorStrRescalingLowerRange = tempInt;
            }
        }

        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_LeftRumbleRescale destiny, BackingData_LeftRumbleRescale source)
        {
            destiny.IsLeftMotorStrRescalingEnabled = source.IsLeftMotorStrRescalingEnabled;
            destiny.leftMotorStrRescalingLowerRange = source.LeftMotorStrRescalingLowerRange;
            destiny.leftMotorStrRescalingUpperRange = source.LeftMotorStrRescalingUpperRange;
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.leftRumbleRescaleData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.leftRumbleRescaleData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            DshmCustomSettings.BMStrRescaleSettings dshmLeftRumbleRescaleSettings = dshmContextSettings.ContextSettings.RumbleSettings.BMStrRescale;

            dshmLeftRumbleRescaleSettings.Enabled = this.IsLeftMotorStrRescalingEnabled;
            dshmLeftRumbleRescaleSettings.MinValue = (byte)this.LeftMotorStrRescalingLowerRange;
            dshmLeftRumbleRescaleSettings.MaxValue = (byte)this.LeftMotorStrRescalingUpperRange;
        }
    }

    public class BackingData_VariablaRightRumbleEmulAdjusts : SettingsBackingData
    {
        public int ForcedRightMotorHeavyThreshold { get; set; } = 230;
        public int ForcedRightMotorLightThreshold { get; set; } = 230;
        public bool IsForcedRightMotorHeavyThreasholdEnabled { get; set; } = false;
        public bool IsForcedRightMotorLightThresholdEnabled { get; set; } = false;

        public int RightRumbleConversionUpperRange { get; set; } = 140;
        public int RightRumbleConversionLowerRange { get; set; } = 1;


        public override void ResetToDefault()
        {
            CopySettings(this, new());
        }

        public static void CopySettings(BackingData_VariablaRightRumbleEmulAdjusts destiny, BackingData_VariablaRightRumbleEmulAdjusts source)
        {
            destiny.RightRumbleConversionLowerRange = source.RightRumbleConversionLowerRange;
            destiny.RightRumbleConversionUpperRange = source.RightRumbleConversionUpperRange;
            // Right rumble (light) threshold
            destiny.IsForcedRightMotorLightThresholdEnabled = source.IsForcedRightMotorLightThresholdEnabled;
            destiny.ForcedRightMotorLightThreshold = source.ForcedRightMotorLightThreshold;
            // Left rumble (Heavy) threshold
            destiny.IsForcedRightMotorHeavyThreasholdEnabled = source.IsForcedRightMotorHeavyThreasholdEnabled;
            destiny.ForcedRightMotorHeavyThreshold = source.ForcedRightMotorHeavyThreshold;
        }

        public override void CopySettingsFromContainer(BackingDataContainer container)
        {
            CopySettings(this, container.rightVariableEmulData);
        }

        public override void CopySettingsToContainer(BackingDataContainer container)
        {
            CopySettings(container.rightVariableEmulData, this);
        }

        public override void SaveToDSHMSettings(DshmCustomSettings dshmContextSettings)
        {
            DshmCustomSettings.SMToBMConversionSettings dshmSMConversionSettings = dshmContextSettings.ContextSettings.RumbleSettings.SMToBMConversion;
            DshmCustomSettings.ForcedSMSettings dshmForcedSMSettings = dshmContextSettings.ContextSettings.RumbleSettings.ForcedSM;

            // Right rumble conversion rescaling adjustment
            if(RightRumbleConversionLowerRange < RightRumbleConversionUpperRange)
            {
                dshmSMConversionSettings.RescaleMinValue = (byte)this.RightRumbleConversionLowerRange;
                dshmSMConversionSettings.RescaleMaxValue = (byte)this.RightRumbleConversionUpperRange;
            }
            else
            {

            }

            

            // Right rumble (light) threshold
            dshmForcedSMSettings.SMThresholdEnabled = this.IsForcedRightMotorLightThresholdEnabled;
            dshmForcedSMSettings.SMThresholdValue = (byte)this.ForcedRightMotorLightThreshold;

            // Left rumble (Heavy) threshold
            dshmForcedSMSettings.BMThresholdEnabled = this.IsForcedRightMotorHeavyThreasholdEnabled;
            dshmForcedSMSettings.BMThresholdValue = (byte)this.ForcedRightMotorHeavyThreshold;
        }
    }
}
