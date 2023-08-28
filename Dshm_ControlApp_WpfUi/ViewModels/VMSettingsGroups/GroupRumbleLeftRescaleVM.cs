using Nefarius.DsHidMini.ControlApp.UserData;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupRumbleLeftRescaleVM : GroupSettingsVM
    {
        private BackingData_LeftRumbleRescale _tempBackingData = new();
        protected override IBackingData _myInterface => _tempBackingData;

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

        public GroupRumbleLeftRescaleVM() : base()
        {
        }

        //public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_LeftRumbleRescale.CopySettings(dataContainerSource.leftRumbleRescaleData, _tempBackingData);
        //}

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_LeftRumbleRescale.CopySettings(_tempBackingData, dataContainerSource.leftRumbleRescaleData);
        //    NotifyAllPropertiesHaveChanged();
        //}
    }


}
