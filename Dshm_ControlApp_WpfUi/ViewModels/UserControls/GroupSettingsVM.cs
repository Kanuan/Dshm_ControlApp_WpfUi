using Nefarius.DsHidMini.ControlApp.UserData;
using System.ComponentModel;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public partial class VMGroupsContainer : ObservableObject
    {   
        private readonly List<GroupSettingsVM> groupSettingsList = new();
        [ObservableProperty] public bool _allowEditing = false;
        [ObservableProperty] private GroupModeUniqueVM _groupModeUnique = new();
        [ObservableProperty] private GroupLEDsCustomsVM _groupLEDsControl = new();
        [ObservableProperty] private GroupWirelessSettingsVM _groupWireless = new();
        [ObservableProperty] private GroupSticksVM _groupSticksDZ = new();
        [ObservableProperty] private GroupRumbleGeneralVM _groupRumbleGeneral = new();
        [ObservableProperty] private GroupOutRepControlVM _groupOutRepControl = new();
        [ObservableProperty] private GroupRumbleLeftRescaleVM _groupRumbleLeftRescale = new();
        [ObservableProperty] private GroupRumbleRightConversionAdjustsVM _groupRumbleRightConversion = new();

        public VMGroupsContainer() : this(null)
        {
        }

        public VMGroupsContainer(DeviceSettings? dataContainer = null)
        {
            groupSettingsList.Add(GroupModeUnique);
            groupSettingsList.Add(GroupLEDsControl);
            groupSettingsList.Add(GroupWireless);
            groupSettingsList.Add(GroupSticksDZ);
            groupSettingsList.Add(GroupRumbleGeneral);
            groupSettingsList.Add(GroupOutRepControl);
            groupSettingsList.Add(GroupRumbleLeftRescale);
            groupSettingsList.Add(GroupRumbleRightConversion);

            this.GroupModeUnique.PropertyChanged += ModeSettingsChanged;

            if (dataContainer != null)
                LoadDatasToAllGroups(dataContainer);
        }

        private void UpdateLockStateOfGroups()
        {
            foreach (GroupSettingsVM group in groupSettingsList)
            {
                group.IsGroupLocked = false;
            }

            if (GroupModeUnique.Context == SettingsContext.DS4W)
            {
                GroupSticksDZ.IsGroupLocked = GroupModeUnique.PreventRemappingConflictsInDS4WMode;
            }

            if (GroupModeUnique.Context == SettingsContext.SXS)
            {
                GroupSticksDZ.IsGroupLocked = GroupModeUnique.PreventRemappingConflictsInSXSMode;
            }
        }

        private void ModeSettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(GroupModeUnique.Context):
                case nameof(GroupModeUnique.PreventRemappingConflictsInSXSMode):
                case nameof(GroupModeUnique.PreventRemappingConflictsInDS4WMode):
                    UpdateLockStateOfGroups();
                    break;
            }
        }

        public void SaveAllChangesToBackingData(DeviceSettings dataContainer)
        {
            foreach (GroupSettingsVM group in groupSettingsList)
            {
                group.SaveSettingsToBackingDataContainer(dataContainer);
            }

        }

        public void LoadDatasToAllGroups(DeviceSettings dataContainer)
        {
            foreach (GroupSettingsVM group in groupSettingsList)
            {
                group.LoadSettingsFromBackingDataContainer(dataContainer);
            }

        }
    }

    public abstract partial class GroupSettingsVM : ObservableObject
    {
        /// Replace with LexLoc
        private static Dictionary<SettingsModeGroups, string> DictGroupHeader = new()
        {
            { SettingsModeGroups.LEDsControl, "LEDs control" },
            { SettingsModeGroups.WirelessSettings, "Wireless settings" },
            { SettingsModeGroups.SticksDeadzone, "Sticks DeadZone" },
            { SettingsModeGroups.RumbleGeneral, "Rumble settings" },
            { SettingsModeGroups.OutputReportControl, "Output report control" },
            { SettingsModeGroups.RumbleLeftStrRescale, "Left motor (heavy) rescale" },
            { SettingsModeGroups.RumbleRightConversion, "Alternative rumble mode adjuster" },
            { SettingsModeGroups.Unique_All, "Mode specific settings" },
            { SettingsModeGroups.Unique_Global, "Default settings" },
            { SettingsModeGroups.Unique_General, "General settings" },
            { SettingsModeGroups.Unique_SDF, "SDF mode specific settings" },
            { SettingsModeGroups.Unique_GPJ, "GPJ mode specific settings" },
            { SettingsModeGroups.Unique_SXS, "SXS mode specific settings" },
            { SettingsModeGroups.Unique_DS4W, "DS4W mode specific settings" },
            { SettingsModeGroups.Unique_XInput, "GPJ mode specific settings" },
        };

        [ObservableProperty] private List<string> _controlAppProfiles = new List<string>() { "profile 1", "profile 2", "profile 3" };

        public abstract SettingsModeGroups Group { get; }

        protected abstract IBackingData _myInterface { get; }

        [ObservableProperty] private bool _isGroupLocked = false;

        public string Header { get; }

        [RelayCommand]
        public void ResetGroupToOriginalDefaults()
        {
            _myInterface.ResetToDefault();
            NotifyAllPropertiesHaveChanged();
        }

        public virtual void NotifyAllPropertiesHaveChanged()
        {
            this.OnPropertyChanged(string.Empty);
        }

        public void LoadSettingsFromBackingDataContainer(DeviceSettings dataContainerSource)
        {
            _myInterface.CopySettingsFromContainer(dataContainerSource);
            NotifyAllPropertiesHaveChanged();
        }

        public void SaveSettingsToBackingDataContainer(DeviceSettings dataContainerSource)
        {
            _myInterface.CopySettingsToContainer(dataContainerSource);
        }

        public GroupSettingsVM()
        {
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            //LoadSettingsFromBackingDataContainer(backingDataContainer);
        }
    }
}
