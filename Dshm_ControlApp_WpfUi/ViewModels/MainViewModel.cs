﻿using Nefarius.DsHidMini.ControlApp.Util.App;
using System.Collections.ObjectModel;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public partial class MainViewModel : ObservableObject
    {
        /// <summary>
        ///     List of detected devices.
        /// </summary>
        internal ObservableCollection<TestViewModel> Devices { get; set; }

        /// <summary>
        ///     Currently selected device, if any.
        /// </summary>
        [ObservableProperty] private TestViewModel _selectedDevice;

        public MainViewModel()
        {
            Devices = new ObservableCollection<TestViewModel>();

        }



        public ApplicationConfiguration AppConfig => ApplicationConfiguration.Instance;



        /// <summary>
        ///     Is a device currently selected.
        /// </summary>
        public bool HasDeviceSelected => SelectedDevice != null;

        /// <summary>
        ///     Are there devices connected.
        /// </summary>
        public bool HasNoDevices => Devices.Count == 0;

        /*
        /// <summary>
        ///     Helper to check if run with elevated privileges.
        /// </summary>
        public bool IsElevated => SecurityUtil.IsElevated;

        /// <summary>
        ///     True if run as regular, non-administrative user.
        /// </summary>
        public bool IsMissingPermissions => !IsElevated;

        /// <summary>
        ///     Is it possible to edit the selected device.
        /// </summary>
        public bool IsEditable => IsElevated && HasDeviceSelected && !IsRestarting;

        /// <summary>
        ///     Is the selected device in the process of getting restarted.
        /// </summary>
        public bool IsRestarting { get; set; } = false;

        /// <summary>
        ///     Version to display in window title.
        /// </summary>
        public string Version => $"Version: {Assembly.GetEntryAssembly().GetName().Version}";

        /// <summary>
        ///     True if GitHub version is newer than own version.
        /// </summary>
        public bool IsUpdateAvailable => Updater.IsUpdateAvailable;

        private static string ParametersKey =>
            "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WUDF\\Services\\dshidmini\\Parameters";

        /// <summary>
        ///     Indicates if verbose logging is on or off.
        /// </summary>
        public bool VerboseOn
        {
            get
            {
                using (var key = RegistryHelpers.GetRegistryKey(ParametersKey))
                {
                    if (key == null) return false;
                    var value = key.GetValue("VerboseOn");
                    return value != null && (int) value > 0;
                }
            }
            set
            {
                using (var key = RegistryHelpers.GetRegistryKey(ParametersKey, true))
                {
                    key?.SetValue("VerboseOn", value ? 1 : 0, RegistryValueKind.DWord);
                }
            }
        }

        public bool IsFilterAvailable => IsElevated && BthPS3FilterDriver.IsFilterAvailable;

        public bool IsFilterUnavailable => IsElevated && !BthPS3FilterDriver.IsFilterAvailable;

        public bool IsFilterEnabled
        {
            get => IsElevated && BthPS3FilterDriver.IsFilterEnabled;
            set => BthPS3FilterDriver.IsFilterEnabled = value;
        }

        public bool IsRawPDODisabled => !BthPS3ProfileDriver.RawPDO;

        public bool AreBthPS3SettingsCorrect =>
            IsElevated && !BthPS3ProfileDriver.RawPDO && BthPS3FilterDriver.IsFilterEnabled;

        public bool AreBthPS3SettingsIncorrect =>
            IsElevated && (BthPS3ProfileDriver.RawPDO || !BthPS3FilterDriver.IsFilterEnabled);

        public bool IsPressureMutingSupported => IsEditable && SelectedDevice.IsPressureMutingSupported;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshProperties()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFilterEnabled"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRawPDODisabled"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AreBthPS3SettingsIncorrect"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AreBthPS3SettingsCorrect"));
        }
        */
    }
}