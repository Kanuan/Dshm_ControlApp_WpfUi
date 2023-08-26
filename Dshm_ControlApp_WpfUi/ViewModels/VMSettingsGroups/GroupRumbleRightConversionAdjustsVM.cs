﻿using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleRightConversionAdjustsVM : GroupSettingsVM
    {
        // -------------------------------------------- RIGHT MOTOR CONVERSION GROUP

        public BackingData_VariablaRightRumbleEmulAdjusts _tempBackingData = new();
        protected override IBackingData _myInterface => _tempBackingData;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        public int RightRumbleConversionUpperRange
        {
            get => _tempBackingData.RightRumbleConversionUpperRange;
            set
            {
                int tempInt = (value < _tempBackingData.RightRumbleConversionLowerRange) ? _tempBackingData.RightRumbleConversionLowerRange + 1 : value;
                _tempBackingData.RightRumbleConversionUpperRange = tempInt;
                this.OnPropertyChanged(nameof(RightRumbleConversionUpperRange));
            }
        }
        public int RightRumbleConversionLowerRange
        {
            get => _tempBackingData.RightRumbleConversionLowerRange;
            set
            {
                int tempInt = (value > _tempBackingData.RightRumbleConversionUpperRange) ? (byte)(_tempBackingData.RightRumbleConversionUpperRange - 1) : value;
                _tempBackingData.RightRumbleConversionLowerRange = tempInt;
                this.OnPropertyChanged(nameof(RightRumbleConversionLowerRange));
            }
        }
        public bool IsForcedRightMotorLightThresholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorLightThresholdEnabled;
            set
            {
                _tempBackingData.IsForcedRightMotorLightThresholdEnabled = value;
                this.OnPropertyChanged(nameof(IsForcedRightMotorLightThresholdEnabled));
            }
        }
        public bool IsForcedRightMotorHeavyThreasholdEnabled
        {
            get => _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled;
            set
            {
                _tempBackingData.IsForcedRightMotorHeavyThreasholdEnabled = value;
                this.OnPropertyChanged(nameof(IsForcedRightMotorHeavyThreasholdEnabled));
            }
        }
        public int ForcedRightMotorLightThreshold
        {
            get => _tempBackingData.ForcedRightMotorLightThreshold;
            set
            {
                _tempBackingData.ForcedRightMotorLightThreshold = value;
                this.OnPropertyChanged(nameof(ForcedRightMotorLightThreshold));
            }
        }
        public int ForcedRightMotorHeavyThreshold
        {
            get => _tempBackingData.ForcedRightMotorHeavyThreshold;
            set
            {
                _tempBackingData.ForcedRightMotorHeavyThreshold = value;
                this.OnPropertyChanged(nameof(ForcedRightMotorHeavyThreshold));
            }
        }

        public GroupRumbleRightConversionAdjustsVM() : base()
        {
        }

        //public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(dataContainerSource.rightVariableEmulData, _tempBackingData);
        //}

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(_tempBackingData, dataContainerSource.rightVariableEmulData);
        //    NotifyAllPropertiesHaveChanged();
        //}
    }


}
