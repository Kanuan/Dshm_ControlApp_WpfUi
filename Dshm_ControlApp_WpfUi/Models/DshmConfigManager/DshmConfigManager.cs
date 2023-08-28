using Microsoft.Win32.SafeHandles;
using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.DsHidMini.ControlApp.Util.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nefarius.DsHidMini.ControlApp.UserData
{
    internal class DshmConfigManager
    {
        // ----------------------------------------------------------- FIELDS

        public static JsonSerializerOptions DshmConfigSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,

            Converters =
            {
                new JsonStringEnumConverter(),
                new DshmConfigCustomJsonConverter(),
            }
        };

        private const string DISK = @"C:\";
        private const string CONTROL_APP_FOLDER_PATH_IN_DISK = @"ProgramData\DsHidMini\ControlApp\";
        private const string CONTROL_APP_SETTINGS_FILE_NAME = @"ControlAppSettings.json";
        private const string DSHM_FOLDER_PATH_IN_DISK = @"ProgramData\DsHidMini\";
        private const string PROFILE_FOLDER_NAME = @"Profiles\";
        private const string DEVICES_FOLDER_NAME = @"Devices\";

        public string DshmFolderFullPath { get; } = $@"{DISK}{DSHM_FOLDER_PATH_IN_DISK}";
        public string ProfilesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{PROFILE_FOLDER_NAME}";
        public string DevicesFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{DEVICES_FOLDER_NAME}";
        public string ControlAppFolderFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}";
        public string ControlAppSettingsFileFullPath { get; } = $@"{DISK}{CONTROL_APP_FOLDER_PATH_IN_DISK}{CONTROL_APP_SETTINGS_FILE_NAME}";

        public static Guid guid = new();

        // ----------------------------------------------------------- AUTO-PROPERTIES

        private static readonly DshmConfigManagerUserData dshmManagerUserData = DshmConfigManagerUserData.Instance;


        // ----------------------------------------------------------- PROPERTIES

        public ProfileData GlobalProfile
        {
            get
            {
                ProfileData gp = GetProfile(dshmManagerUserData.GlobalProfileGuid);
                if (gp == null)
                {
                    dshmManagerUserData.GlobalProfileGuid = ProfileData.DefaultGuid;
                    gp = ProfileData.DefaultProfile;
                }
                return gp;
            }
            set => dshmManagerUserData.GlobalProfileGuid = value.ProfileGuid;
        }

        public List<ProfileData> Profiles
        {
            get
            {
                var userProfilesPlusDefault = new List<ProfileData>(dshmManagerUserData.Profiles);
                if(!userProfilesPlusDefault.Contains(ProfileData.DefaultProfile))
                    userProfilesPlusDefault.Insert(0,ProfileData.DefaultProfile);
                return userProfilesPlusDefault;
            }
            set
            {
                var userProfilesMinusDefault = new List<ProfileData>(value);
                if (userProfilesMinusDefault.Contains(ProfileData.DefaultProfile))
                    userProfilesMinusDefault.Remove(ProfileData.DefaultProfile);
                dshmManagerUserData.Profiles = userProfilesMinusDefault;                
            }
        }


        // ----------------------------------------------------------- CONSTRUCTOR

        public DshmConfigManager()
        {
            FixDevicesWithBlankProfiles();
        }

        private class DshmConfigManagerUserData
        {
            /// <summary>
            ///     Implicitly loads configuration from file.
            /// </summary>
            private static readonly Lazy<DshmConfigManagerUserData> AppConfigLazy =
                new Lazy<DshmConfigManagerUserData>(() => JsonApplicationConfiguration
                    .Load<DshmConfigManagerUserData>(
                        GlobalUserDataFileName,
                        true,
                        true));

            /// <summary>
            ///     Singleton instance of app configuration.
            /// </summary>
            public static DshmConfigManagerUserData Instance => AppConfigLazy.Value;

            [JsonIgnore]
            public static string GlobalUserDataFileName => "DshmUserData";
            public Guid GlobalProfileGuid { get; set; } = ProfileData.DefaultGuid;
            public List<ProfileData> Profiles { get; set; } = new();
            public List<DeviceData> Devices { get; set; } = new();


            /// <summary>
            ///     Write changes to file.
            /// </summary>
            public void Save()
            {
                //
                // Store (modified) configuration to disk
                // 
                JsonApplicationConfiguration.Save(
                    GlobalUserDataFileName,
                    this,
                    true);
            }

        }

        // ----------------------------------------------------------- METHODS

        private void FixDevicesWithBlankProfiles()
        {
            foreach(DeviceData device in dshmManagerUserData.Devices)
            {
                if(GetProfile(device.GuidOfProfileToUse) == null)
                {
                    device.GuidOfProfileToUse = ProfileData.DefaultGuid;
                    device.SettingsMode = SettingsModes.Global;
                }
            }
        }

        public ProfileData? GetProfile(Guid profileGuid)
        {
            ProfileData profile = null;

            foreach(ProfileData p in Profiles)
            {
                if(p.ProfileGuid == profileGuid)
                {
                    profile = p;
                    break;
                }
            }
            return profile;
        }

        public void SaveChangesAndUpdateDsHidMiniConfigFile()
        {
            dshmManagerUserData.Save();
            UpdateDsHidMiniConfigFile();
        }

        public void UpdateDsHidMiniConfigFile()
        {
            var dshmSettings = new DshmConfiguration();
            
            GlobalProfile.DataContainer.ConvertAllToDSHM(dshmSettings.Global);
           
            foreach(DeviceData dev in dshmManagerUserData.Devices)
            {
                var temp = new DshmDeviceData();
                temp.DeviceAddress = dev.DeviceMac;
                temp.CustomSettings.DisableAutoPairing = !dev.AutoPairWhenCabled;
                temp.CustomSettings.PairingAddress = dev.PairingAddress;
                switch (dev.SettingsMode)
                {
                    case SettingsModes.Custom:
                dev.DatasContainter.ConvertAllToDSHM(temp.CustomSettings);
                        break;
                    case SettingsModes.Profile:
                        ProfileData devprof = GetProfile(dev.GuidOfProfileToUse);
                        if(devprof == null)
                        {
                            break; ; // If profile set for the controller does not exist anymore then leave settings blank so controller loads Global Profile
                        }
                        else
                        {
                            devprof.DataContainer.ConvertAllToDSHM(temp.CustomSettings);
                        }
                        break;

                    case SettingsModes.Global:
                    default:
                        break; ;
                }
                dshmSettings.Devices.Add(temp);
            }
            
            string profileJson = JsonSerializer.Serialize(dshmSettings, DshmConfigSerializerOptions);

            System.IO.Directory.CreateDirectory(DshmFolderFullPath);
            System.IO.File.WriteAllText($@"{DshmFolderFullPath}DsHidMini.json", profileJson);
        }

        public void CreateNewProfile(string profileName)
        {
            ProfileData newProfile = new();
            newProfile.ProfileName = profileName;
            //newProfile.DiskFileName = profileName + ".json";
            dshmManagerUserData.Profiles.Add(newProfile);
        }

        public void DeleteProfile(ProfileData profile)
        {
            if (profile == ProfileData.DefaultProfile) // Must not save Default Profile to disk
            {
                return;
            }
            dshmManagerUserData.Profiles.Remove(profile);
            FixDevicesWithBlankProfiles();
        }

        public DeviceData GetDeviceData(string deviceMac)
        {
            foreach (DeviceData dev in dshmManagerUserData.Devices)
            {
                if (dev.DeviceMac == deviceMac)
                {
                    return dev;
                }
            }
            var newDevice = new DeviceData(deviceMac);
            newDevice.DeviceMac = deviceMac;
            dshmManagerUserData.Devices.Add(newDevice);
            return newDevice;
        }

    }
}