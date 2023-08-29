using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager.Enums;

namespace Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager
{
    public class DeviceData
    {
        public string DeviceMac { get; set; } = "0000000000";
        public string DeviceCustomName { get; set; } = "DualShock 3";
        public Guid GuidOfProfileToUse { get; set; } = new Guid();
        public bool AutoPairWhenCabled { get; set; } = true;
        public string? PairingAddress { get; set; } = null;
        public SettingsModes SettingsMode { get; set; } = SettingsModes.Global;

        public DeviceSettings DatasContainter { get; set; } = new();

        public DeviceData(string deviceMac)
        {
            DeviceMac = deviceMac;
        }
    }
}