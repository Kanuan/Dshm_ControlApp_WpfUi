using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
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

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleRightConversion;

        public int RightRumbleConversionUpperRange
        {
            get => _tempBackingData.RightRumbleConversionUpperRange;
            set
            {
                _tempBackingData.RightRumbleConversionUpperRange = value;
                this.OnPropertyChanged(nameof(RightRumbleConversionUpperRange));

            }
        }
        public int RightRumbleConversionLowerRange
        {
            get => _tempBackingData.RightRumbleConversionLowerRange;
            set
            {
                _tempBackingData.RightRumbleConversionLowerRange = value;
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

        public GroupRumbleRightConversionAdjustsVM(BackingDataContainer backingDataContainer) : base(backingDataContainer) { }
        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.OnPropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.rightVariableEmulData);
        }
        public void SaveSettingsToBackingData(BackingData_VariablaRightRumbleEmulAdjusts dataSource)
        {
            BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.rightVariableEmulData);
        }

        public void LoadSettingsFromBackingData(BackingData_VariablaRightRumbleEmulAdjusts dataTarget)
        {
            BackingData_VariablaRightRumbleEmulAdjusts.CopySettings(_tempBackingData, dataTarget);
            this.OnPropertyChanged(string.Empty);
        }
    }


}
