﻿namespace Nefarius.DsHidMini.ControlApp.UserData
{
    public class ProfileData
    {
        public static readonly Guid DefaultGuid = new Guid("00000000000000000000000000000000");
        public string ProfileName { get; set; }
        public Guid ProfileGuid { get; set; } = Guid.NewGuid();

        public DeviceSettings DataContainer { get; set; } = new();

        public ProfileData()
        {
        }

        public static readonly ProfileData DefaultProfile = new()
        {
            ProfileName = "XInput (Default)",
            ProfileGuid = DefaultGuid,
            DataContainer = new(),
        };

        public override string ToString()
        {
            return ProfileName;
        }
    }
}