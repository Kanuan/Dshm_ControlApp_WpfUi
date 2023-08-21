﻿using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        private BackingData_LeftRumbleRescale _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleLeftStrRescale;

        public bool IsLeftMotorStrRescalingEnabled
        {
            get => _tempBackingData.IsLeftMotorStrRescalingEnabled;
            set
            {
                _tempBackingData.IsLeftMotorStrRescalingEnabled = value;
                this.OnPropertyChanged(nameof(IsLeftMotorStrRescalingEnabled));
            }
        }
        public int LeftMotorStrRescalingUpperRange
        {
            get => _tempBackingData.LeftMotorStrRescalingUpperRange;
            set
            {
                _tempBackingData.LeftMotorStrRescalingUpperRange = value;
                this.OnPropertyChanged(nameof(LeftMotorStrRescalingUpperRange));
            }
        }
        public int LeftMotorStrRescalingLowerRange
        {
            get => _tempBackingData.LeftMotorStrRescalingLowerRange;
            set
            {
                _tempBackingData.LeftMotorStrRescalingLowerRange = value;
                this.OnPropertyChanged(nameof(LeftMotorStrRescalingLowerRange));
            }
        }

        public GroupRumbleLeftRescaleVM(BackingDataContainer backingDataContainer) : base(backingDataContainer) { }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.OnPropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.leftRumbleRescaleData);
        }

        public void SaveSettingsToBackingData(BackingData_LeftRumbleRescale dataSource)
        {
            BackingData_LeftRumbleRescale.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.leftRumbleRescaleData);
        }

        public void LoadSettingsFromBackingData(BackingData_LeftRumbleRescale dataTarget)
        {
            BackingData_LeftRumbleRescale.CopySettings(_tempBackingData, dataTarget);
            this.OnPropertyChanged(string.Empty);
        }
    }


}