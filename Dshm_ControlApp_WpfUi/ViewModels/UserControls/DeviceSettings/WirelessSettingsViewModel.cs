using Nefarius.DsHidMini.ControlApp.DshmConfigManager;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class WirelessSettingsViewModel : DeviceSettingsViewModel
    {
        private WirelessSettings _tempBackingData = new();
        protected override IBackingData _myInterface => _tempBackingData;

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.WirelessSettings;

        public bool IsWirelessIdleDisconnectEnabled
        {
            get => _tempBackingData.IsWirelessIdleDisconnectEnabled;
            set
            {
                _tempBackingData.IsWirelessIdleDisconnectEnabled = value;
                this.OnPropertyChanged(nameof(IsWirelessIdleDisconnectEnabled));
            }
        }
        public int WirelessIdleDisconnectTime
        {
            get => _tempBackingData.WirelessIdleDisconnectTime;
            set
            {
                _tempBackingData.WirelessIdleDisconnectTime = value;
                this.OnPropertyChanged(nameof(WirelessIdleDisconnectTime));
            }
        }
        public bool IsQuickDisconnectComboEnabled
        {
            get => _tempBackingData.QuickDisconnectCombo.IsEnabled;
            set
            {
                _tempBackingData.QuickDisconnectCombo.IsEnabled = value;
                this.OnPropertyChanged(nameof(IsQuickDisconnectComboEnabled));
            }
        }

        public int QuickDisconnectComboHoldTime
        {
            get => _tempBackingData.QuickDisconnectCombo.HoldTime;
            set
            {
                _tempBackingData.QuickDisconnectCombo.HoldTime = value;
                this.OnPropertyChanged(nameof(QuickDisconnectComboHoldTime));
            }
        }
        public Manager_Button QuickDisconnectComboButton1
        {
            get => _tempBackingData.QuickDisconnectCombo.Button1;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button1 = value;
                this.OnPropertyChanged(nameof(QuickDisconnectComboButton1));
            }
        }

        public Manager_Button QuickDisconnectComboButton2
        {
            get => _tempBackingData.QuickDisconnectCombo.Button2;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button2 = value;
                this.OnPropertyChanged(nameof(QuickDisconnectComboButton2));
            }
        }

        public Manager_Button QuickDisconnectComboButton3
        {
            get => _tempBackingData.QuickDisconnectCombo.Button3;
            set
            {
                _tempBackingData.QuickDisconnectCombo.Button3 = value;
                this.OnPropertyChanged(nameof(QuickDisconnectComboButton3));
            }
        }

        public WirelessSettingsViewModel() : base()
        {

        }
    }


}
