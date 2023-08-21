using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupSticksVM : GroupSettingsVM
    {
        // -------------------------------------------- STICKS DEADZONE GROUP
        private BackingData_Sticks _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.SticksDeadzone;

        public ObservableAsPropertyHelper<bool> isGroupLockedToPreventDS4WConflicts;
        
        public bool ApplyLeftStickDeadZone
        {
            get => _tempBackingData.LeftStickData.IsDeadZoneEnabled;
            set
            {
                _tempBackingData.LeftStickData.IsDeadZoneEnabled = value;
                this.OnPropertyChanged(nameof(ApplyLeftStickDeadZone));
            }
        }

        public bool ApplyRightStickDeadZone
        {
            get => _tempBackingData.RightStickData.IsDeadZoneEnabled;
            set
            {
                _tempBackingData.RightStickData.IsDeadZoneEnabled = value;
                this.OnPropertyChanged(nameof(ApplyRightStickDeadZone));
            }
        }

        public int LeftStickDeadZone
        {
            get => _tempBackingData.LeftStickData.DeadZone;
            set
            {
                _tempBackingData.LeftStickData.DeadZone = value;
                this.OnPropertyChanged(nameof(LeftStickDeadZone));
            }
        }

        public int RightStickDeadZone
        {
            get => _tempBackingData.RightStickData.DeadZone;
            set
            {
                _tempBackingData.RightStickData.DeadZone = value;
                this.OnPropertyChanged(nameof(RightStickDeadZone));
            }
        }

        public bool InvertLSX
        {
            get => _tempBackingData.LeftStickData.InvertXAxis;
            set
            {
                _tempBackingData.LeftStickData.InvertXAxis = value;
                this.OnPropertyChanged(nameof(InvertLSX));
            }
        }

        public bool InvertLSY
        {
            get => _tempBackingData.LeftStickData.InvertYAxis;
            set
            {
                _tempBackingData.LeftStickData.InvertYAxis = value;
                this.OnPropertyChanged(nameof(InvertLSY));
            }
        }

        public bool InvertRSX
        {
            get => _tempBackingData.RightStickData.InvertXAxis;
            set
            {
                _tempBackingData.RightStickData.InvertXAxis = value;
                this.OnPropertyChanged(nameof(InvertRSX));
            }
        }

        public bool InvertRSY
        {
            get => _tempBackingData.RightStickData.InvertYAxis;
            set
            {
                _tempBackingData.RightStickData.InvertYAxis = value;
                this.OnPropertyChanged(nameof(InvertRSY));
            }
        }


        public GroupSticksVM(BackingDataContainer backingDataContainer) : base(backingDataContainer)
        {


        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();
            this.OnPropertyChanged(string.Empty);
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.sticksData);
        }
        public void SaveSettingsToBackingData(BackingData_Sticks dataSource)
        {
            BackingData_Sticks.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.sticksData);
        }

        public void LoadSettingsFromBackingData(BackingData_Sticks dataTarget)
        {
            BackingData_Sticks.CopySettings(_tempBackingData, dataTarget);
            this.OnPropertyChanged(string.Empty);
        }
    }
}
