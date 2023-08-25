using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class GroupOutRepControlVM : GroupSettingsVM
    {
        private BackingData_OutRepControl _tempBackingData = new();
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
            base._myInterface = _tempBackingData;
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
