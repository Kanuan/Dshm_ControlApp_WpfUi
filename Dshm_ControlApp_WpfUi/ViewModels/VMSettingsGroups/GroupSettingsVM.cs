using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Windows;

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

        public VMGroupsContainer(BackingDataContainer dataContainer)
        {
            groupSettingsList.Add(GroupModeUnique);
            groupSettingsList.Add(GroupLEDsControl);
            groupSettingsList.Add(GroupWireless);
            groupSettingsList.Add(GroupSticksDZ);
            groupSettingsList.Add(GroupRumbleGeneral);
            groupSettingsList.Add(GroupOutRepControl);
            groupSettingsList.Add(GroupRumbleLeftRescale);
            groupSettingsList.Add(GroupRumbleRightConversion);

            LoadDatasToAllGroups(dataContainer);

            this.WhenAnyValue(
                x => x.GroupModeUnique.Context,
                x => x.GroupModeUnique.IsDS4LightbarTranslationEnabled)
                .Subscribe(x => UpdateLockStateOfGroups());

            // Duct tape for RaisePropertyChange(string.empty)
            this.GroupModeUnique.PropertyChanged += UpdateLockStateOfGroupsIfEmptyStringOnPropertyChanged;

        }

        private void UpdateLockStateOfGroups()
        {
            foreach (GroupSettingsVM group in groupSettingsList)
            {
                group.IsGroupLocked = false;
            }

            if (GroupModeUnique.Context == SettingsContext.DS4W)
            {
                GroupSticksDZ.IsGroupLocked = true;
                GroupLEDsControl.IsGroupLocked = GroupModeUnique.IsDS4LightbarTranslationEnabled;
            }
        }

        private void UpdateLockStateOfGroupsIfEmptyStringOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "":
                    UpdateLockStateOfGroups();
                    break;
            }
        }

        public void SaveAllChangesToBackingData(BackingDataContainer dataContainer)
        {
            foreach (GroupSettingsVM group in groupSettingsList)
            {
                group.SaveSettingsToBackingDataContainer(dataContainer);
            }

        }

        public void LoadDatasToAllGroups(BackingDataContainer dataContainer)
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
            { SettingsModeGroups.SticksDeadzone, "Sticks DeadZone (DZ)" },
            { SettingsModeGroups.RumbleGeneral, "Rumble settings" },
            { SettingsModeGroups.OutputReportControl, "Output report control" },
            { SettingsModeGroups.RumbleLeftStrRescale, "Left motor (heavy) rescale" },
            { SettingsModeGroups.RumbleRightConversion, "Variable light rumble emulation adjuster" },
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

        public void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            _myInterface.CopySettingsFromContainer(dataContainerSource);
        }

        public void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
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
