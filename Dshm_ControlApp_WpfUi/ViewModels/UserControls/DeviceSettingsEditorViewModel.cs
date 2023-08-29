using Nefarius.DsHidMini.ControlApp.DshmConfigManager;
using System.ComponentModel;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public partial class SettingsEditorViewModel : ObservableObject
    {   
        private readonly List<DeviceSettingsViewModel> groupSettingsList = new();
        [ObservableProperty] public bool _allowEditing = false;
        [ObservableProperty] private HidModeSettingsViewModel _hidModeVM = new();
        [ObservableProperty] private LedsSettingsViewModel _ledsSettingsVM = new();
        [ObservableProperty] private WirelessSettingsViewModel _wirelessSettingsVM = new();
        [ObservableProperty] private SticksSettingsViewModel _sticksSettingsVM = new();
        [ObservableProperty] private GeneralRumbleSettingsViewModel _generalRumbleSettingsVM = new();
        [ObservableProperty] private OutputReportSettingsViewModel _outRepSettingsVM = new();
        [ObservableProperty] private LeftMotorRescalingSettingsViewModel _leftMotorRescaleSettingsVM = new();
        [ObservableProperty] private AltRumbleModeSettingsViewModel _altRumbleSettingsVM = new();

        public SettingsEditorViewModel() : this(null)
        {
        }

        public SettingsEditorViewModel(DeviceSettings? dataContainer = null)
        {
            groupSettingsList.Add(HidModeVM);
            groupSettingsList.Add(LedsSettingsVM);
            groupSettingsList.Add(WirelessSettingsVM);
            groupSettingsList.Add(SticksSettingsVM);
            groupSettingsList.Add(GeneralRumbleSettingsVM);
            groupSettingsList.Add(OutRepSettingsVM);
            groupSettingsList.Add(LeftMotorRescaleSettingsVM);
            groupSettingsList.Add(AltRumbleSettingsVM);

            this.HidModeVM.PropertyChanged += ModeSettingsChanged;

            if (dataContainer != null)
                LoadDatasToAllGroups(dataContainer);
        }

        private void UpdateLockStateOfGroups()
        {
            foreach (DeviceSettingsViewModel group in groupSettingsList)
            {
                group.IsGroupLocked = false;
            }

            if (HidModeVM.Context == SettingsContext.DS4W)
            {
                SticksSettingsVM.IsGroupLocked = HidModeVM.PreventRemappingConflictsInDS4WMode;
            }

            if (HidModeVM.Context == SettingsContext.SXS)
            {
                SticksSettingsVM.IsGroupLocked = HidModeVM.PreventRemappingConflictsInSXSMode;
            }
        }

        private void ModeSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(HidModeVM.Context):
                case nameof(HidModeVM.PreventRemappingConflictsInSXSMode):
                case nameof(HidModeVM.PreventRemappingConflictsInDS4WMode):
                    UpdateLockStateOfGroups();
                    break;
            }
        }

        public void SaveAllChangesToBackingData(DeviceSettings dataContainer)
        {
            foreach (DeviceSettingsViewModel group in groupSettingsList)
            {
                group.SaveSettingsToBackingDataContainer(dataContainer);
            }

        }

        public void LoadDatasToAllGroups(DeviceSettings dataContainer)
        {
            foreach (DeviceSettingsViewModel group in groupSettingsList)
            {
                group.LoadSettingsFromBackingDataContainer(dataContainer);
            }

        }
    }
}
