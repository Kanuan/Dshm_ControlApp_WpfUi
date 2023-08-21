﻿using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleGeneralVM : GroupSettingsVM
    {
        private BackingData_RumbleGeneral _tempBackingData = new();
       
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.RumbleGeneral;

        public bool IsVariableLightRumbleEmulationEnabled
        {
            get => _tempBackingData.IsVariableLightRumbleEmulationEnabled;
            set
            {
                _tempBackingData.IsVariableLightRumbleEmulationEnabled = value;
                this.OnPropertyChanged(nameof(IsVariableLightRumbleEmulationEnabled));
            }
        }

        public bool IsVariableRightEmulToggleComboEnabled
        {
            get => _tempBackingData.IsVariableRightEmulToggleComboEnabled;
            set
            {
                _tempBackingData.IsVariableRightEmulToggleComboEnabled = value;
                this.OnPropertyChanged(nameof(IsVariableRightEmulToggleComboEnabled));
            }
        }

        public ControlApp_ComboButtons VarRightEmul_ToggleComboButton1
        {
            get => _tempBackingData.VariableRightEmulToggleCombo.Button1;
            set
            {
                _tempBackingData.VariableRightEmulToggleCombo.Button1 = value;
                this.OnPropertyChanged(nameof(VarRightEmul_ToggleComboButton1));
            }
        }

        public ControlApp_ComboButtons VarRightEmul_ToggleComboButton2
        {
            get => _tempBackingData.VariableRightEmulToggleCombo.Button2;
            set
            {
                _tempBackingData.VariableRightEmulToggleCombo.Button2 = value;
                this.OnPropertyChanged(nameof(VarRightEmul_ToggleComboButton2));
            }
        }
        public ControlApp_ComboButtons VarRightEmul_ToggleComboButton3
        {
            get => _tempBackingData.VariableRightEmulToggleCombo.Button3;
            set
            {
                _tempBackingData.VariableRightEmulToggleCombo.Button3 = value;
                this.OnPropertyChanged(nameof(VarRightEmul_ToggleComboButton3));
            }
        }

        public bool IsLeftMotorDisabled
        {
            get => _tempBackingData.IsLeftMotorDisabled;

            set
            {
                _tempBackingData.IsLeftMotorDisabled = value;
                this.OnPropertyChanged(nameof(IsLeftMotorDisabled));
            }
        }
        public bool IsRightMotorDisabled
        {
            get => _tempBackingData.IsRightMotorDisabled;

            set
            {
                _tempBackingData.IsRightMotorDisabled = value;
                this.OnPropertyChanged(nameof(IsRightMotorDisabled));
            }
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.OnPropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.rumbleGeneralData);
        }

        public void SaveSettingsToBackingData(BackingData_RumbleGeneral dataSource)
        {
            BackingData_RumbleGeneral.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.rumbleGeneralData);
        }

        public void LoadSettingsFromBackingData(BackingData_RumbleGeneral dataTarget)
        {
            BackingData_RumbleGeneral.CopySettings(_tempBackingData, dataTarget);
            this.OnPropertyChanged(string.Empty);
        }

        public GroupRumbleGeneralVM(BackingDataContainer backingDataContainer) : base(backingDataContainer) { }
    }


}
