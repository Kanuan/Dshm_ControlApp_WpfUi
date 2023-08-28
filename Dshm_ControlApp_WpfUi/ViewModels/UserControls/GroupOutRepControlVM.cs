﻿using Nefarius.DsHidMini.ControlApp.UserData;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupOutRepControlVM : GroupSettingsVM
    {
        private OutputReportSettings _tempBackingData = new();
        protected override IBackingData _myInterface => _tempBackingData;
        public override SettingsModeGroups Group { get; } = SettingsModeGroups.OutputReportControl;
        public bool IsOutputReportRateControlEnabled
        {
            get => _tempBackingData.IsOutputReportRateControlEnabled;
            set
            {
                _tempBackingData.IsOutputReportRateControlEnabled = value;
                this.OnPropertyChanged(nameof(IsOutputReportRateControlEnabled));
            }
        }

        public int MaxOutputRate
        {
            get => _tempBackingData.MaxOutputRate;
            set
            {
                _tempBackingData.MaxOutputRate = value;
                this.OnPropertyChanged(nameof(MaxOutputRate));
            }
        }

        public bool IsOutputReportDeduplicatorEnabled
        {
            get => _tempBackingData.IsOutputReportDeduplicatorEnabled;
            set
            {
                _tempBackingData.IsOutputReportDeduplicatorEnabled = value;
                this.OnPropertyChanged(nameof(IsOutputReportDeduplicatorEnabled));
            }
        }

        public GroupOutRepControlVM() : base()
        {
        }

        //public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_OutRepControl.CopySettings(dataContainerSource.outRepData, _tempBackingData);
        //}

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_OutRepControl.CopySettings(_tempBackingData, dataContainerSource.outRepData);
        //    NotifyAllPropertiesHaveChanged();
        //}
    }


}