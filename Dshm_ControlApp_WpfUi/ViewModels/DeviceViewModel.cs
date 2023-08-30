using FontAwesome5;
using Nefarius.DsHidMini.ControlApp.Models.Drivers;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager.Enums;
using Nefarius.DsHidMini.ControlApp.Services;
using Nefarius.DsHidMini.ControlApp.ViewModels.Pages;
using Nefarius.DsHidMini.ControlApp.ViewModels.UserControls;
using Nefarius.Utilities.Bluetooth;
using Nefarius.Utilities.DeviceManagement.PnP;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.ViewModels
{
    public partial class DeviceViewModel : ObservableObject
    {
        // ------------------------------------------------------ FIELDS

        private readonly DshmConfigManager _dshmConfigManager;
        private readonly AppSnackbarMessagesService _appSnackbarMessagesService;


        private readonly PnPDevice _device;
        private DeviceData deviceUserData;
        private readonly Timer _batteryQuery;

        // ------------------------------------------------------ PROPERTIES

        [ObservableProperty] private SettingsEditorViewModel _deviceCustomsVM = new() { AllowEditing = true };
        [ObservableProperty] private SettingsEditorViewModel _profileCustomsVM = new();
        [ObservableProperty] private SettingsEditorViewModel _globalCustomsVM = new();

        [ObservableProperty] private SettingsEditorViewModel _selectedGroupsVM = new();

        //internal string DisplayName { get; set; }
        [ObservableProperty] private bool _isEditorEnabled;
        [ObservableProperty] private bool _isProfileSelectorVisible;

        [ObservableProperty] private SettingsModes _currentDeviceSettingsMode;

        /// <summary>
        ///     Current HID device emulation mode.
        /// </summary>
        public int HidEmulationMode => _device.GetProperty<byte>(DsHidMiniDriver.HidDeviceModeProperty);


        /// <summary>
        ///     The device Instance ID.
        /// </summary>
        public string InstanceId => _device.InstanceId;

        /// <summary>
        ///     The Bluetooth MAC address of this device.
        /// </summary>
        public string DeviceAddress => _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();

        /// <summary>
        ///     The Bluetooth MAC address of this device.
        /// </summary>
        public string DeviceAddressFriendly
        {
            get
            {
                var friendlyAddress = DeviceAddress;

                var insertedCount = 0;
                for (var i = 2; i < DeviceAddress.Length; i = i + 2)
                    friendlyAddress = friendlyAddress.Insert(i + insertedCount++, ":");

                return friendlyAddress;
            }
        }


        /// <summary>
        ///     The Bluetooth MAC address of the host radio this device is currently paired to.
        /// </summary>
        public string HostAddress
        {
            get
            {
                var hostAddress = _device.GetProperty<ulong>(DsHidMiniDriver.HostAddressProperty).ToString("X12")
                    .ToUpper();

                var friendlyAddress = hostAddress;

                var insertedCount = 0;
                for (var i = 2; i < hostAddress.Length; i = i + 2)
                    friendlyAddress = friendlyAddress.Insert(i + insertedCount++, ":");

                return friendlyAddress;
            }
        }
        
        [ObservableProperty] private string? _pairingMacAddress;

        private BluetoothPairingMode? _pairingMode;

        public int PairingMode
        {
            get => (int)_pairingMode;
            set
            {
                _pairingMode = (BluetoothPairingMode)value;
                this.OnPropertyChanged(nameof(PairingMode));
            }

        }

        /// <summary>
        ///     Current battery status.
        /// </summary>
        public DsBatteryStatus BatteryStatus =>
            (DsBatteryStatus)_device.GetProperty<byte>(DsHidMiniDriver.BatteryStatusProperty);

        /// <summary>
        ///     Current battery status.
        /// </summary>
        public string BatteryStatusInText =>
            ((DsBatteryStatus)_device.GetProperty<byte>(DsHidMiniDriver.BatteryStatusProperty)).ToString();

        /// <summary>
        ///     Return a battery icon depending on the charge.
        /// </summary>
        public SymbolRegular BatteryIcon
        {
            get
            {
                switch (BatteryStatus)
                {
                    case DsBatteryStatus.Charged:
                        return SymbolRegular.Battery1024;
                    case DsBatteryStatus.Charging:
                        return SymbolRegular.BatteryCharge24;
                    case DsBatteryStatus.Full:
                        return SymbolRegular.Battery1024;
                    case DsBatteryStatus.High:
                        return SymbolRegular.Battery724;
                    case DsBatteryStatus.Medium:
                        return SymbolRegular.Battery524;
                    case DsBatteryStatus.Low:
                        return SymbolRegular.Battery224;
                    case DsBatteryStatus.Dying:
                        return SymbolRegular.Battery024;
                    default:
                        return SymbolRegular.BatteryWarning24;
                }
            }
        }

        public EFontAwesomeIcon LastPairingStatusIcon
        {
            get
            {
                var ntstatus = _device.GetProperty<int>(DsHidMiniDriver.LastPairingStatusProperty);

                return ntstatus == 0
                    ? EFontAwesomeIcon.Regular_CheckCircle
                    : EFontAwesomeIcon.Solid_ExclamationTriangle;
            }
        }

        //public EFontAwesomeIcon GenuineIcon
        //{
        //    get
        //    {
        //        if (Validator.IsGenuineAddress(PhysicalAddress.Parse(DeviceAddress)))
        //            return EFontAwesomeIcon.Regular_CheckCircle;
        //        return EFontAwesomeIcon.Solid_ExclamationTriangle;
        //    }
        //}

        /// <summary>
        ///     The friendly (product) name of this device.
        /// </summary>
        public string DisplayName
        {
            get
            {
                var name = _device.GetProperty<string>(DevicePropertyKey.Device_FriendlyName);

                return string.IsNullOrEmpty(name) ? "DS3 Compatible HID Device" : name;
            }
        }

        public bool IsWireless
        {
            get
            {
                var enumerator = _device.GetProperty<string>(DevicePropertyKey.Device_EnumeratorName);

                return !enumerator.Equals("USB", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        ///     The connection protocol used by this device.
        /// </summary>
        public EFontAwesomeIcon ConnectionType =>
            !IsWireless
                ? EFontAwesomeIcon.Brands_Usb
                : EFontAwesomeIcon.Brands_Bluetooth;

        /// <summary>
        ///     Icon for connection protocol
        /// </summary>
        public SymbolRegular ConnectionTypeIcon =>
            !IsWireless
                ? SymbolRegular.UsbPlug24
                : SymbolRegular.Bluetooth24;

        /// <summary>
        ///     Last time this device has been seen connected (applies to Bluetooth connected devices only).
        /// </summary>
        public DateTimeOffset LastConnected =>
            _device.GetProperty<DateTimeOffset>(DsHidMiniDriver.BluetoothLastConnectedTimeProperty);

        private void UpdateBatteryStatus(object state)
        {
            OnPropertyChanged(nameof(BatteryStatus));
        }



        // ------------------------------------------------------ CONSTRUCTOR

        internal DeviceViewModel(PnPDevice device, DshmConfigManager dshmConfigManager, AppSnackbarMessagesService appSnackbarMessagesService)
        {
            _device = device;
            _dshmConfigManager = dshmConfigManager;
            _appSnackbarMessagesService = appSnackbarMessagesService;
            _batteryQuery = new Timer(UpdateBatteryStatus, null, 10000, 10000);
            deviceUserData = _dshmConfigManager.GetDeviceData(DeviceAddress);
            // Loads correspondent controller data based on controller's MAC address 



            //DisplayName = DeviceAddress;
            RefreshDeviceSettings();
        }


        // ------------------------------------------------------ METHODS

        [ObservableProperty] private ProfileData? _selectedProfile;

        [ObservableProperty] public List<ProfileData> _listOfProfiles;

        partial void OnCurrentDeviceSettingsModeChanged(SettingsModes value)
        {
            switch (CurrentDeviceSettingsMode)
            {
                case SettingsModes.Custom:
                    SelectedGroupsVM = DeviceCustomsVM;
                    break;
                case SettingsModes.Profile:
                    SelectedGroupsVM = ProfileCustomsVM;
                    break;
                case SettingsModes.Global:
                default:
                    SelectedGroupsVM = GlobalCustomsVM;
                    break;
            }
            IsProfileSelectorVisible = CurrentDeviceSettingsMode == SettingsModes.Profile;
        }

        partial void OnSelectedProfileChanged(ProfileData? value)
        {
            ProfileCustomsVM.LoadDatasToAllGroups(SelectedProfile.DeviceSettings);
        }

        [RelayCommand]
        public void RefreshDeviceSettings()
        {
            PairingMode = (int)deviceUserData.BluetoothPairingMode;
            DeviceCustomsVM.LoadDatasToAllGroups(deviceUserData.Settings);
            ListOfProfiles = _dshmConfigManager.Profiles;
            SelectedProfile = _dshmConfigManager.GetProfile(deviceUserData.GuidOfProfileToUse);
            GlobalCustomsVM.LoadDatasToAllGroups(_dshmConfigManager.GlobalProfile.DeviceSettings);
            CurrentDeviceSettingsMode = deviceUserData.SettingsMode;
        }

        //public void UpdateEditor()
        //{

        //}

        [RelayCommand]
        private void ApplyChanges()
        {
            deviceUserData.SettingsMode = CurrentDeviceSettingsMode;
            deviceUserData.BluetoothPairingMode = (BluetoothPairingMode)PairingMode;

            if (CurrentDeviceSettingsMode != SettingsModes.Global)
            {
                SelectedGroupsVM.SaveAllChangesToBackingData(deviceUserData.Settings);
            }

            if (CurrentDeviceSettingsMode == SettingsModes.Profile)
            {
                deviceUserData.GuidOfProfileToUse = SelectedProfile.ProfileGuid;
            }
            _appSnackbarMessagesService.ShowDsHidMiniConfigurationUpdateSuccessMessage();
            _dshmConfigManager.SaveChangesAndUpdateDsHidMiniConfigFile();
        }

        [RelayCommand]
        private void RestartDevice()
        {
            if(IsWireless)
            {
                using (var radio = new HostRadio())
                {
                    radio.DisconnectRemoteDevice(DeviceAddress);
                };
            }
            else
            {
                try
                {
                    (_device).RemoveAndSetup();
                }
                catch
                {

                }
            }
        }

    }

}