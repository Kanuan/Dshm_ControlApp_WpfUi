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
    public class VMGroupsContainer : ObservableObject
    {
        [PropertyChanged] internal List<GroupSettingsVM> GroupSettingsList { get; set; } = new();
        [ObservableProperty] public bool _allowEditing = false;
        [ObservableProperty] public GroupModeUniqueVM GroupModeUnique { get; set; }
        [ObservableProperty] public GroupLEDsCustomsVM GroupLEDsControl { get; set; }
        [ObservableProperty] public GroupWirelessSettingsVM GroupWireless { get; set; }
        [ObservableProperty] public GroupSticksVM GroupSticksDZ { get; set; }
        [ObservableProperty] public GroupRumbleGeneralVM GroupRumbleGeneral { get; set; }
        [ObservableProperty] public GroupOutRepControlVM GroupOutRepControl { get; set; }
        [ObservableProperty] public GroupRumbleLeftRescaleVM GroupRumbleLeftRescale { get; set; }
        [ObservableProperty] public GroupRumbleRightConversionAdjustsVM GroupRumbleRightConversion { get; set; }

        public VMGroupsContainer(BackingDataContainer dataContainer)
        {
            GroupSettingsList.Add(GroupModeUnique = new(dataContainer, this));
            GroupSettingsList.Add(GroupLEDsControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupWireless = new(dataContainer, this));
            GroupSettingsList.Add(GroupSticksDZ = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleGeneral = new(dataContainer, this));
            GroupSettingsList.Add(GroupOutRepControl = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleLeftRescale = new(dataContainer, this));
            GroupSettingsList.Add(GroupRumbleRightConversion = new(dataContainer, this));

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

    public abstract class GroupSettingsVM : ObservableObject
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

        [ObservableProperty] public virtual bool IsGroupLocked { get; set; } = false;

        [ObservableProperty] public virtual bool IsOverrideCheckboxVisible { get; set; } = false;

        public abstract SettingsModeGroups Group { get; }

        public string Header { get; }

        public abstract void ResetGroupToOriginalDefaults();

        public abstract void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource);
        public abstract void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource);

        public GroupSettingsVM(BackingDataContainer backingDataContainer)
        {
            if (DictGroupHeader.TryGetValue(Group, out string groupHeader)) Header = groupHeader;
            LoadSettingsFromBackingDataContainer(backingDataContainer);
            ResetGroupToDefaultsCommand = ReactiveCommand.Create(ResetGroupToOriginalDefaults);
        }

        public ReactiveCommand<Unit, Unit> ResetGroupToDefaultsCommand { get; }

    }

    public class TemplateSelector
    {
        //public Control Build(object param)
        //{
        //    string templateName = SettingsGroupToTemplateDict[(SettingsModeGroups)param];

        //    Avalonia.Application.Current.Resources.TryGetResource(templateName, null,out var tempResource);
        //    var resultingCtrl = ((IDataTemplate)tempResource).Build(0);

        //    return resultingCtrl;
        //}

        public bool Match(object data)
        {
            // Check if we can accept the provided data
            return data is SettingsModeGroups;
        }

        private static Dictionary<SettingsModeGroups, string> SettingsGroupToTemplateDict = new()
        {
            { SettingsModeGroups.LEDsControl, "Template_LEDsSettings" },
            { SettingsModeGroups.WirelessSettings, "Template_WirelessSettings" },
            { SettingsModeGroups.SticksDeadzone, "Template_SticksDeadZone" },
            { SettingsModeGroups.RumbleGeneral, "Template_RumbleBasicFunctions" },
            { SettingsModeGroups.OutputReportControl, "Template_OutputRateControl" },
            { SettingsModeGroups.RumbleLeftStrRescale, "Template_RumbleHeavyStrRescale" },
            { SettingsModeGroups.RumbleRightConversion, "Template_RumbleVariableLightEmuTuning" },
            { SettingsModeGroups.Unique_All, "Template_Unique_All" },
            { SettingsModeGroups.Unique_Global, "Template_ToDo" },
            { SettingsModeGroups.Unique_General, "Template_ToDo" },
            { SettingsModeGroups.Unique_SDF, "Template_SDF_GPJ_PressureButtons" },
            { SettingsModeGroups.Unique_GPJ, "Template_SDF_GPJ_PressureButtons" },
            { SettingsModeGroups.Unique_SXS, "Template_ToDo" },
            { SettingsModeGroups.Unique_DS4W, "Template_ToDo" },
            { SettingsModeGroups.Unique_XInput, "Template_ToDo" },
        };
    }
}
