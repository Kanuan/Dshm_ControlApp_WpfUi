﻿using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
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
        [ObservableProperty] internal List<GroupSettingsVM> _groupSettingsList = new();
        [ObservableProperty] public bool _allowEditing = false;
        [ObservableProperty] public GroupModeUniqueVM _groupModeUnique;
        [ObservableProperty] public GroupLEDsCustomsVM _groupLEDsControl;
        [ObservableProperty] public GroupWirelessSettingsVM _groupWireless;
        [ObservableProperty] public GroupSticksVM _groupSticksDZ;
        [ObservableProperty] public GroupRumbleGeneralVM _groupRumbleGeneral;
        [ObservableProperty] public GroupOutRepControlVM _groupOutRepControl;
        [ObservableProperty] public GroupRumbleLeftRescaleVM _groupRumbleLeftRescale;
        [ObservableProperty] public GroupRumbleRightConversionAdjustsVM _groupRumbleRightConversion;

        public VMGroupsContainer(BackingDataContainer dataContainer)
        {
            GroupSettingsList.Add(GroupModeUnique = new(dataContainer));
            GroupSettingsList.Add(GroupLEDsControl = new(dataContainer));
            GroupSettingsList.Add(GroupWireless = new(dataContainer));
            GroupSettingsList.Add(GroupSticksDZ = new(dataContainer));
            GroupSettingsList.Add(GroupRumbleGeneral = new(dataContainer));
            GroupSettingsList.Add(GroupOutRepControl = new(dataContainer));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(dataContainer));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(dataContainer));

            this.WhenAnyValue(
                x => x.GroupModeUnique.Context,
                x => x.GroupModeUnique.IsDS4LightbarTranslationEnabled)
                .Subscribe(x => UpdateLockStateOfGroups());

            // Duct tape for RaisePropertyChange(string.empty)
            this.GroupModeUnique.PropertyChanged += UpdateLockStateOfGroupsIfEmptyStringOnPropertyChanged;

        }

        public void UpdateLockStateOfGroups()
        {
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.IsGroupLocked = false;
            }

            if (GroupModeUnique.Context == SettingsContext.DS4W)
            {
                GroupSticksDZ.IsGroupLocked = true;
                GroupLEDsControl.IsGroupLocked = GroupModeUnique.IsDS4LightbarTranslationEnabled;
            }
        }

        void UpdateLockStateOfGroupsIfEmptyStringOnPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            foreach (GroupSettingsVM group in GroupSettingsList)
            {
                group.SaveSettingsToBackingDataContainer(dataContainer);
            }

        }

        public void LoadDatasToAllGroups(BackingDataContainer dataContainer)
        {
            foreach (GroupSettingsVM group in GroupSettingsList)
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

        protected IBackingData _myInterface;

        [ObservableProperty] public bool _isGroupLocked = false;

        public abstract SettingsModeGroups Group { get; }

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

        public abstract void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource);

        public GroupSettingsVM(BackingDataContainer backingDataContainer)
        {
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            LoadSettingsFromBackingDataContainer(backingDataContainer);
        }
    }
}
