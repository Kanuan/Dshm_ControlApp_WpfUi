using Nefarius.DsHidMini.ControlApp.Models.Drivers;
using Nefarius.Utilities.Bluetooth;
using Nefarius.Utilities.DeviceManagement.PnP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.Windows.Networking.XboxLive;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.Models.Util;

namespace Nefarius.DsHidMini.ControlApp.Models
{
    internal class MyApp
    {
        public DshmConfigManager.DshmConfigManager DshmUserData { get; } = new();

        /// <summary>
        ///     Helper to check if run with elevated privileges.
        /// </summary>
        public bool IsElevated => SecurityUtil.IsElevated;

        /// <summary>
        ///     True if run as regular, non-administrative user.
        /// </summary>
        public bool IsMissingPermissions => !IsElevated;
    }


    internal class ConnectedDevicesManager
    {
        private DeviceNotificationListener _deviceNotificationListener;
        private readonly HostRadio _hostRadio = new();
        private readonly DshmConfigManager.DshmConfigManager _dshmConfigManager = new();

        public bool IsElevated { get; }


        public ConnectedDevicesManager(DeviceNotificationListener deviceNotificationListener, HostRadio hostRadio)
        {
            _deviceNotificationListener = deviceNotificationListener;
            _hostRadio = hostRadio;

        }


        public void StartDeviceListener()
        {
            _deviceNotificationListener = new();
            _deviceNotificationListener.StartListen(DsHidMiniDriver.DeviceInterfaceGuid);
        }

        public void StopDeviceListenerAndDispose()
        {
            _deviceNotificationListener.StopListen();
            _deviceNotificationListener.Dispose();
        }

        public DshmV3Device? something(PnPDevice pnpDevice)
        {
            var devaddress = pnpDevice.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();
            if (devaddress == null) return null;
            var devData = _dshmConfigManager.GetDeviceData(devaddress);
            return new DshmV3Device(devData, pnpDevice);
        }

        public void DisconnectFromBluetooth(string macAddress)
        {
            _hostRadio.DisconnectRemoteDevice(macAddress);
        }

        public void PowerCycleUsbDevice(PnPDevice device)
        {
            ((UsbPnPDevice)device).CyclePort();
            
        }
    }

    internal class DshmV3Device
    {

        private readonly PnPDevice? _device;

        private readonly DeviceData _deviceData;

        /// <summary>
        ///     Current battery status.
        /// </summary>
        public DsBatteryStatus? BatteryStatus { get; set; }
        
        public bool Ntstatus { get; set; }

        public string DisplayName { get; set; }

        public string DeviceAddress { get; set; }

        public bool IsWireless { get; set; }

        public DateTimeOffset LastConnected { get; set; }

        public DeviceData DeviceUserData => _deviceData;

        public PnPDevice? Device => _device;

        public string DriverVersion { get; set; }

        public DshmV3Device(DeviceData deviceData, PnPDevice? device = null)
        {
            _deviceData = deviceData;
            _device = device;
        }

        public void RefreshProperties()
        {
            DeviceAddress = _device.GetProperty<string>(DsHidMiniDriver.DeviceAddressProperty).ToUpper();
            BatteryStatus = (DsBatteryStatus)_device.GetProperty<byte>(DsHidMiniDriver.BatteryStatusProperty);
            Ntstatus = (_device.GetProperty<int>(DsHidMiniDriver.LastPairingStatusProperty) == 0);
            var name = _device.GetProperty<string>(DevicePropertyKey.Device_FriendlyName);
            DisplayName = string.IsNullOrEmpty(name) ? "DS3 Compatible HID Device" : name;
            DriverVersion = _device.GetProperty<string>(DevicePropertyKey.Device_DriverVersion);

            var enumerator = _device.GetProperty<string>(DevicePropertyKey.Device_EnumeratorName);
            IsWireless = !enumerator.Equals("USB", StringComparison.InvariantCultureIgnoreCase);
            LastConnected = _device.GetProperty<DateTimeOffset>(DsHidMiniDriver.BluetoothLastConnectedTimeProperty);
        }

        public void RefreshDynamicProperties()
        {
            //
        }
    }
}
