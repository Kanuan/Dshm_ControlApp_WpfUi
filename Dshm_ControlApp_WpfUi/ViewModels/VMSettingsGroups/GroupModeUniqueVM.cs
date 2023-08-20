﻿using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{

    public class GroupModeUniqueVM : GroupSettingsVM
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

        private BackingData_ModesUnique _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.Unique_All;

        public SettingsContext Context 
        {
            get => _tempBackingData.SettingsContext;
            set
            {
                _tempBackingData.SettingsContext = value;
                this.RaisePropertyChanged(nameof(Context));
            }

        }
        public ControlApp_DsPressureMode PressureExposureMode
        {
            get => _tempBackingData.PressureExposureMode;
            set
            {
                _tempBackingData.PressureExposureMode = value;
                this.RaisePropertyChanged(nameof(PressureExposureMode));
            }
        }

        public ControlApp_DPADModes DPadExposureMode
        {
            get => _tempBackingData.DPadExposureMode;
            set
            {
                _tempBackingData.DPadExposureMode = value;
                this.RaisePropertyChanged(nameof(DPadExposureMode));
            }
        }

        // SXS 
        public bool PreventRemappingConflictsInSXSMode
        {
            get => _tempBackingData.PreventRemappingConflictsInSXSMode;
            set
            {
                _tempBackingData.PreventRemappingConflictsInSXSMode = value;
                this.RaisePropertyChanged(nameof(PreventRemappingConflictsInSXSMode));
            }
        }

        public bool AllowAppsToControlLEDsInSXSMode
        {
            get => _tempBackingData.AllowAppsToOverrideLEDsInSXSMode;
            set
            {
                _tempBackingData.AllowAppsToOverrideLEDsInSXSMode = value;
                this.RaisePropertyChanged(nameof(AllowAppsToControlLEDsInSXSMode));
            }
        }

        // XInput
        public bool IsLEDsAsXInputSlotEnabled
        {
            get => _tempBackingData.IsLEDsAsXInputSlotEnabled;
            set
            {
                _tempBackingData.IsLEDsAsXInputSlotEnabled = value;
                this.RaisePropertyChanged(nameof(IsLEDsAsXInputSlotEnabled));
            }
        }

        // DS4Windows
        public bool IsDS4LightbarTranslationEnabled
        {
            get => _tempBackingData.IsDS4LightbarTranslationEnabled;
            set
            {
                _tempBackingData.IsDS4LightbarTranslationEnabled = value;
                this.RaisePropertyChanged(nameof(IsDS4LightbarTranslationEnabled));
            }
        }

        public GroupModeUniqueVM(BackingDataContainer backingDataContainer, VMGroupsContainer vmGroupsContainter) : base(backingDataContainer)
        {
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.RaisePropertyChanged(string.Empty);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.modesUniqueData);
        }

        public void LoadSettingsFromBackingData(BackingData_ModesUnique dataTarget)
        {
            BackingData_ModesUnique.CopySettings(_tempBackingData, dataTarget);
            this.RaisePropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.modesUniqueData);
        }

        public void SaveSettingsToBackingData(BackingData_ModesUnique dataSource)
        {
            BackingData_ModesUnique.CopySettings(dataSource, _tempBackingData);
        }
    }
}
