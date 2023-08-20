﻿using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Threading;
using FontAwesome5;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.Util;
using Nefarius.DsHidMini.ControlApp.Util.Web;
using Nefarius.Utilities.DeviceManagement.PnP;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        private readonly Timer _batteryQuery;
        private readonly PnPDevice _device;

        public DeviceViewModel(PnPDevice device)
        {
            _device = device;

            _batteryQuery = new Timer(UpdateBatteryStatus, null, 10000, 10000);
        }

        /*
        public bool MuteDigitalPressureButtons
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.MuteDigitalPressureButtonsProperty) > 0;
            set
            {
                using (var evt = EventWaitHandle.OpenExisting(
                           $"Global\\DsHidMiniConfigHotReloadEvent{DeviceAddress}"
                       ))
                {
                    _device.SetProperty(DsHidMiniDriver.MuteDigitalPressureButtonsProperty, (byte)(value ? 1 : 0));

                    evt.Set();
                }
            }
        }
        */

        public bool IsHidModeChangeable =>
            SecurityUtil.IsElevated /*&& HidEmulationMode != DsHidDeviceMode.XInputHIDCompatible*/;

        /// <summary>
        ///     Current HID device emulation mode.
        /// </summary>
        public DsHidDeviceMode HidEmulationMode
        {
            get =>
                (DsHidDeviceMode)_device.GetProperty<byte>(
                    DsHidMiniDriver.HidDeviceModeProperty);
            set
            {
                _device.SetProperty(DsHidMiniDriver.HidDeviceModeProperty, (byte)value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPressureMutingSupported)));
            }
        }

        /*
        public bool IsOutputRateControlEnabled
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.IsOutputRateControlEnabledProperty) > 0;
            set => _device.SetProperty(DsHidMiniDriver.IsOutputRateControlEnabledProperty, (byte)(value ? 1 : 0));
        }

        public byte OutputRateControlPeriodMs
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.OutputRateControlPeriodMsProperty);
            set => _device.SetProperty(DsHidMiniDriver.OutputRateControlPeriodMsProperty, value);
        }

        public bool IsOutputDeduplicatorEnabled
        {
            get => _device.GetProperty<byte>(DsHidMiniDriver.IsOutputDeduplicatorEnabledProperty) > 0;
            set => _device.SetProperty(DsHidMiniDriver.IsOutputDeduplicatorEnabledProperty, (byte)(value ? 1 : 0));
        }

        public uint WirelessIdleTimeoutPeriodMs
        {
            get => _device.GetProperty<uint>(DsHidMiniDriver.WirelessIdleTimeoutPeriodMsProperty) / 60000;
            set => _device.SetProperty(DsHidMiniDriver.WirelessIdleTimeoutPeriodMsProperty, value * 60000);
        }
        */

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

        /// <summary>
        ///     Current battery status.
        /// </summary>
        public DsBatteryStatus BatteryStatus =>
            (DsBatteryStatus)_device.GetProperty<byte>(DsHidMiniDriver.BatteryStatusProperty);

        /// <summary>
        ///     Return a battery icon depending on the charge.
        /// </summary>
        public EFontAwesomeIcon BatteryIcon
        {
            get
            {
                switch (BatteryStatus)
                {
                    case DsBatteryStatus.Charged:
                    case DsBatteryStatus.Charging:
                    case DsBatteryStatus.Full:
                        return EFontAwesomeIcon.Solid_BatteryFull;
                    case DsBatteryStatus.High:
                        return EFontAwesomeIcon.Solid_BatteryThreeQuarters;
                    case DsBatteryStatus.Medium:
                        return EFontAwesomeIcon.Solid_BatteryHalf;
                    case DsBatteryStatus.Low:
                        return EFontAwesomeIcon.Solid_BatteryQuarter;
                    case DsBatteryStatus.Dying:
                        return EFontAwesomeIcon.Solid_BatteryEmpty;
                    default:
                        return EFontAwesomeIcon.Solid_BatteryEmpty;
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

        public EFontAwesomeIcon GenuineIcon
        {
            get
            {
                if (Validator.IsGenuineAddress(PhysicalAddress.Parse(DeviceAddress)))
                    return EFontAwesomeIcon.Regular_CheckCircle;
                return EFontAwesomeIcon.Solid_ExclamationTriangle;
            }
        }

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
        ///     Last time this device has been seen connected (applies to Bluetooth connected devices only).
        /// </summary>
        public DateTimeOffset LastConnected =>
            _device.GetProperty<DateTimeOffset>(DsHidMiniDriver.BluetoothLastConnectedTimeProperty);

        public bool IsPressureMutingSupported =>
            HidEmulationMode is DsHidDeviceMode.Single or DsHidDeviceMode.Multi;

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateBatteryStatus(object state)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BatteryStatus"));
        }

        /// <summary>
        ///     Apply changes by requesting device restart.
        /// </summary>
        public void ApplyChanges()
        {
            _device.Restart();
        }
    }
}