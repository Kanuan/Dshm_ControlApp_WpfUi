using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
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

        public bool IsAltModeEnabled
        {
            get => _tempBackingData.IsAltRumbleModeEnabled;
            set
            {
                _tempBackingData.IsAltRumbleModeEnabled = value;
                this.OnPropertyChanged(nameof(IsAltModeEnabled));
            }
        }

        public bool IsAltModeToggleButtonComboEnabled
        {
            get => _tempBackingData.IsAltModeToggleButtonComboEnabled;
            set
            {
                _tempBackingData.IsAltModeToggleButtonComboEnabled = value;
                this.OnPropertyChanged(nameof(IsAltModeToggleButtonComboEnabled));
            }
        }

        public bool AlwaysStartInNormal
        {
            get => _tempBackingData.IsAltModeToggleButtonComboEnabled;
            set
            {
                _tempBackingData.IsAltModeToggleButtonComboEnabled = value;
                this.OnPropertyChanged(nameof(IsAltModeToggleButtonComboEnabled));
            }
        }

        public ControlApp_ComboButtons AltModeToggleButttonCombo_Button1
        {
            get => _tempBackingData.AltModeToggleButtonCombo.Button1;
            set
            {
                _tempBackingData.AltModeToggleButtonCombo.Button1 = value;
                this.OnPropertyChanged(nameof(AltModeToggleButttonCombo_Button1));
            }
        }

        public ControlApp_ComboButtons AltModeToggleButttonCombo_Button2
        {
            get => _tempBackingData.AltModeToggleButtonCombo.Button2;
            set
            {
                _tempBackingData.AltModeToggleButtonCombo.Button2 = value;
                this.OnPropertyChanged(nameof(AltModeToggleButttonCombo_Button2));
            }
        }
        public ControlApp_ComboButtons AltModeToggleButttonCombo_Button3
        {
            get => _tempBackingData.AltModeToggleButtonCombo.Button3;
            set
            {
                _tempBackingData.AltModeToggleButtonCombo.Button3 = value;
                this.OnPropertyChanged(nameof(AltModeToggleButttonCombo_Button3));
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

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            BackingData_RumbleGeneral.CopySettings(dataContainerSource.rumbleGeneralData, _tempBackingData);
        }

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_RumbleGeneral.CopySettings(_tempBackingData, dataContainerSource.rumbleGeneralData);
        //    NotifyAllPropertiesHaveChanged();
        //}

        public GroupRumbleGeneralVM(BackingDataContainer backingDataContainer) : base(backingDataContainer)
        {
            _myInterface = _tempBackingData;
        }
    }


}
