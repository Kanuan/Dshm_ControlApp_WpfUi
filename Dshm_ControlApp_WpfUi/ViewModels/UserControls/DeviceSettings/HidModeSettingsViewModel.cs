﻿using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager.Enums;

namespace Nefarius.DsHidMini.ControlApp.ViewModels.UserControls.DeviceSettings
{

    public class HidModeSettingsViewModel : DeviceSettingsViewModel
    {
        public readonly List<SettingsContext> hidDeviceModesList = new List<SettingsContext>
        {
            SettingsContext.SDF,
            SettingsContext.GPJ,
            SettingsContext.SXS,
            SettingsContext.DS4W,
            SettingsContext.XInput,
        };
        public static readonly List<ControlApp_DsPressureMode> listOfPressureModes = new()
        {
            ControlApp_DsPressureMode.Digital,
            ControlApp_DsPressureMode.Analogue,
            ControlApp_DsPressureMode.Default,
        };

        public static readonly List<ControlApp_DPADModes> listOfDPadModes = new()
        {
            ControlApp_DPADModes.HAT,
            ControlApp_DPADModes.Buttons,
        };
        public List<SettingsContext> HIDDeviceModesList => hidDeviceModesList;
        public static List<ControlApp_DsPressureMode> ListOfPressureModes { get => listOfPressureModes; }
        public static List<ControlApp_DPADModes> ListOfDPadModes { get => listOfDPadModes; }

        private HidModeSettings _tempBackingData = new();
        protected override IDeviceSettings _myInterface => _tempBackingData;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        public SettingsContext Context 
        {
            get => _tempBackingData.SettingsContext;
            set
            {
                _tempBackingData.SettingsContext = value;
                this.OnPropertyChanged(nameof(Context));
            }

        }
        public ControlApp_DsPressureMode PressureExposureMode
        {
            get => _tempBackingData.PressureExposureMode;
            set
            {
                _tempBackingData.PressureExposureMode = value;
                this.OnPropertyChanged(nameof(PressureExposureMode));
            }
        }

        public ControlApp_DPADModes DPadExposureMode
        {
            get => _tempBackingData.DPadExposureMode;
            set
            {
                _tempBackingData.DPadExposureMode = value;
                this.OnPropertyChanged(nameof(DPadExposureMode));
            }
        }

        // SXS 
        public bool PreventRemappingConflictsInSXSMode
        {
            get => _tempBackingData.PreventRemappingConflictsInSXSMode;
            set
            {
                _tempBackingData.PreventRemappingConflictsInSXSMode = value;
                this.OnPropertyChanged(nameof(PreventRemappingConflictsInSXSMode));
            }
        }

        public bool AllowAppsToControlLEDsInSXSMode
        {
            get => _tempBackingData.AllowAppsToOverrideLEDsInSXSMode;
            set
            {
                _tempBackingData.AllowAppsToOverrideLEDsInSXSMode = value;
                this.OnPropertyChanged(nameof(AllowAppsToControlLEDsInSXSMode));
            }
        }

        // XInput
        public bool IsLEDsAsXInputSlotEnabled
        {
            get => _tempBackingData.IsLEDsAsXInputSlotEnabled;
            set
            {
                _tempBackingData.IsLEDsAsXInputSlotEnabled = value;
                this.OnPropertyChanged(nameof(IsLEDsAsXInputSlotEnabled));
            }
        }

        // DS4Windows
        public bool PreventRemappingConflictsInDS4WMode
        {
            get => _tempBackingData.PreventRemappingConflictsInDS4WMode;
            set
            {
                _tempBackingData.PreventRemappingConflictsInDS4WMode = value;
                this.OnPropertyChanged(nameof(PreventRemappingConflictsInDS4WMode));
            }
        }

        public HidModeSettingsViewModel() : base()
        {
        }

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_ModesUnique.CopySettings(_tempBackingData, dataContainerSource.modesUniqueData);
        //    NotifyAllPropertiesHaveChanged();
        //}

        //public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_ModesUnique.CopySettings(dataContainerSource.modesUniqueData, _tempBackingData);
        //}
    }
}
