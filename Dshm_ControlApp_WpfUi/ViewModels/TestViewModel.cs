using Dshm_ControlApp_WpfUi.ViewModels.Pages;
using Dshm_ControlApp_WpfUi;
using FontAwesome5;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.UserData;
using Nefarius.Utilities.Bluetooth;
using Nefarius.Utilities.DeviceManagement.PnP;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Linq;
using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public partial class TestViewModel : ObservableObject
    {
        // ------------------------------------------------------ FIELDS

        internal static ControllersUserData UserDataManager = new ControllersUserData();
        internal static ProfileEditorViewModel vm = App.GetService<ProfileEditorViewModel>();
        private readonly PnPDevice _device;
        private DeviceSpecificData deviceUserData;
        public readonly List<SettingsModes> settingsModesList = new List<SettingsModes>
        {
            SettingsModes.Global,
            SettingsModes.Profile,
            SettingsModes.Custom,
        };

        // ------------------------------------------------------ PROPERTIES

        [ObservableProperty] private VMGroupsContainer _deviceCustomsVM;
        [ObservableProperty] private VMGroupsContainer _selectedGroupsVM;

        //internal string DisplayName { get; set; }
        [ObservableProperty] private bool _isEditorEnabled;
        [ObservableProperty] private bool _isProfileSelectorVisible;
        public List<SettingsModes> SettingsModesList => settingsModesList;

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

        [ObservableProperty] private bool _autoPairDeviceWhenCabled = true;

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
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BatteryStatus"));
        }



        // ------------------------------------------------------ CONSTRUCTOR

        internal TestViewModel(PnPDevice device)
        {
            _device = device;
            // Loads correspondent controller data based on controller's MAC address 
            deviceUserData = UserDataManager.GetDeviceData(DeviceAddress);

            AutoPairDeviceWhenCabled = deviceUserData.AutoPairWhenCabled;

            //DisplayName = DeviceAddress;

            // Loads device' specific custom settings from its BackingDataContainer into the Settings Groups VM
            DeviceCustomsVM = new(deviceUserData.DatasContainter);

            // Checks if the Profile GUID the controller is set to use actually exists in the list of disk profiles and loads it if so
            if (UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse) == null)
            {
                deviceUserData.GuidOfProfileToUse = ProfileData.DefaultGuid;
            }
            SelectedProfile = UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse);

            CurrentDeviceSettingsMode = deviceUserData.SettingsMode;

            this.WhenAnyValue(x => x.CurrentDeviceSettingsMode, x => x.SelectedProfile)
                .Subscribe(x => UpdateEditor());
        }


        // ------------------------------------------------------ METHODS

        public void ChangeProfileForDevice(ProfileData profile)
        {
            deviceUserData.GuidOfProfileToUse = profile.ProfileGuid;
            //ProfileCustomsVM = profile.GetProfileVMGroupsContainer();
        }

        public void UpdateEditor()
        {
            switch (CurrentDeviceSettingsMode)
            {
                case SettingsModes.Profile:
                    SelectedGroupsVM = SelectedProfile.GetProfileVMGroupsContainer();
                    break;
                case SettingsModes.Custom:
                    SelectedGroupsVM = DeviceCustomsVM;
                    break;
                case SettingsModes.Global:
                default:
                    SelectedGroupsVM = UserDataManager.GlobalProfile.GetProfileVMGroupsContainer();
                    break;
            }
            SelectedGroupsVM.AllowEditing = CurrentDeviceSettingsMode == SettingsModes.Custom;
            IsProfileSelectorVisible = CurrentDeviceSettingsMode == SettingsModes.Profile;
        }

        [ObservableProperty] private ProfileData? _selectedProfile;

        public List<ProfileData> ListOfProfiles => UserDataManager.Profiles;

        // ---------------------------------------- Commands

        [RelayCommand]
        private void ApplyChanges()
        {
            deviceUserData.SettingsMode = CurrentDeviceSettingsMode;
            deviceUserData.AutoPairWhenCabled = AutoPairDeviceWhenCabled;

            if(CurrentDeviceSettingsMode != SettingsModes.Global)
            {
                SelectedGroupsVM.SaveAllChangesToBackingData(deviceUserData.DatasContainter);
            }

            if (CurrentDeviceSettingsMode == SettingsModes.Profile)
            {
                deviceUserData.GuidOfProfileToUse = SelectedProfile.ProfileGuid;
            }

            UserDataManager.SaveDeviceSpecificDataToDisk(deviceUserData);
            UserDataManager.UpdateDsHidMiniSettings();
        }

        [RelayCommand]
        private void RevertChanges()
        {
            DeviceCustomsVM.LoadDatasToAllGroups(deviceUserData.DatasContainter);
            SelectedProfile = UserDataManager.GetProfile(deviceUserData.GuidOfProfileToUse);
            CurrentDeviceSettingsMode = deviceUserData.SettingsMode;
            AutoPairDeviceWhenCabled = deviceUserData.AutoPairWhenCabled;
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