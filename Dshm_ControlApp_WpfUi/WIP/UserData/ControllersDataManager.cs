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
    public class ProfileData
    {
        private VMGroupsContainer vmGroupsContainer;

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public static Guid DefaultGuid = new Guid("00000000000000000000000000000000");
        public string ProfileName { get; set; }
        public Guid ProfileGuid { get; set; } = Guid.NewGuid();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]

        public BackingDataContainer DataContainer { get; set; } = new();

        public ProfileData()
        {
            /*
            ProfileGuid = Guid.NewGuid();
            DataContainer = BackingDataContainer.GetDefaultDatas();
            */
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

        public VMGroupsContainer GetProfileVMGroupsContainer()
        {
            if (vmGroupsContainer == null)
                vmGroupsContainer = new VMGroupsContainer(DataContainer);
            return vmGroupsContainer;
        }
    }
    public class DeviceSpecificData
    {
        public string DeviceMac { get; set; } = "0000000000";
        public string DeviceCustomName { get; set; } = "DualShock 3";
        public Guid GuidOfProfileToUse { get; set; } = new Guid();
        public bool AutoPairWhenCabled { get; set; } = true;
        public string? PairingAddress { get; set; } = null;
        public SettingsModes SettingsMode { get; set; } = SettingsModes.Global;

        public BackingDataContainer DatasContainter { get; set; } = new();

        public DeviceSpecificData(string deviceMac)
        {
            DeviceMac = deviceMac;
        }
    }
    internal class ControllersUserData
    {
        // ----------------------------------------------------------- FIELDS

        public static JsonSerializerOptions ControlAppJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            IncludeFields = true,

            Converters =
            {
                new JsonStringEnumConverter(),
                new DshmCustomJsonConverter(),
                //new DshmCustomContextNameJsonConverter(),

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

        private static readonly ControlApp_UserData controlAppSettings = ControlApp_UserData.Instance;


        // ----------------------------------------------------------- PROPERTIES

        public ProfileData GlobalProfile
        {
            get
            {
                ProfileData gp = GetProfile(controlAppSettings.GlobalProfileGuid);
                if(gp == null)
                {
                    controlAppSettings.GlobalProfileGuid = ProfileData.DefaultGuid;
                    gp = ProfileData.DefaultProfile;
                }
                return gp;
            }
            set
            {
                controlAppSettings.GlobalProfileGuid = value.ProfileGuid;
            }
        }

        public List<ProfileData> Profiles
        {
            get
            {
                var userProfilesPlusDefault = new List<ProfileData>(controlAppSettings.Profiles);
                if(!userProfilesPlusDefault.Contains(ProfileData.DefaultProfile))
                    userProfilesPlusDefault.Insert(0,ProfileData.DefaultProfile);
                return userProfilesPlusDefault;
            }
            set
            {
                var userProfilesMinusDefault = new List<ProfileData>(value);
                if (userProfilesMinusDefault.Contains(ProfileData.DefaultProfile))
                    userProfilesMinusDefault.Remove(ProfileData.DefaultProfile);
                controlAppSettings.Profiles = userProfilesMinusDefault;                
            }
        }


        // ----------------------------------------------------------- CONSTRUCTOR

        public ControllersUserData()
        {
            FixDevicesWithBlankProfiles();
        }

        private class ControlApp_UserData
        {
            /// <summary>
            ///     Implicitly loads configuration from file.
            /// </summary>
            private static readonly Lazy<ControlApp_UserData> AppConfigLazy =
                new Lazy<ControlApp_UserData>(() => JsonApplicationConfiguration
                    .Load<ControlApp_UserData>(
                        GlobalUserCustomsFileName,
                        true,
                        true));

            /// <summary>
            ///     Singleton instance of app configuration.
            /// </summary>
            public static ControlApp_UserData Instance => AppConfigLazy.Value;

            [JsonIgnore]
            public static string GlobalUserCustomsFileName => "ControlApp_UserData";
            public Guid GlobalProfileGuid { get; set; } = ProfileData.DefaultGuid;
            public List<ProfileData> Profiles { get; set; } = new();
            public List<DeviceSpecificData> Devices { get; set; } = new();


            /// <summary>
            ///     Write changes to file.
            /// </summary>
            public void Save()
            {
                //
                // Store (modified) configuration to disk
                // 
                JsonApplicationConfiguration.Save(
                    GlobalUserCustomsFileName,
                    this,
                    true);
            }

        }

        // ----------------------------------------------------------- METHODS

        private void FixDevicesWithBlankProfiles()
        {
            foreach(DeviceSpecificData device in controlAppSettings.Devices)
            {
                if(GetProfile(device.GuidOfProfileToUse) == null)
                {
                    device.GuidOfProfileToUse = ProfileData.DefaultGuid;
                    device.SettingsMode = SettingsModes.Global;
                }
            }
        }

        //private List<DeviceSpecificData> LoadDevicesFromDisk()
        //{

        //    var devicesOnDisk = new List<DeviceSpecificData>();

        //    if(Directory.Exists(DevicesFolderFullPath))
        //    {
        //        string[] devicesPaths = Directory.GetFiles($@"{DevicesFolderFullPath}", "*.json");
        //        foreach (string devPath in devicesPaths)
        //        {
        //            var dirName = new DirectoryInfo(devPath).Name;
        //            var jsonText = System.IO.File.ReadAllText(devPath);

        //            var data = JsonSerializer.Deserialize<DeviceSpecificData>(jsonText, ControlAppJsonSerializerOptions);
        //            //data.DiskFileName = dirName;
        //            devicesOnDisk.Add(data);
        //        }
        //    }
        //    return devicesOnDisk;
        //}

        //public List<ProfileData> LoadProfilesFromDisk()
        //{
        //    var profilesOnDisk = new List<ProfileData>();
        //    profilesOnDisk.Add(ProfileData.DefaultProfile); // Include Default Profile on profile list
        //    if(Directory.Exists(ProfilesFolderFullPath))
        //    {
        //        string[] profilesPaths = Directory.GetFiles($@"{ProfilesFolderFullPath}", "*.json");
        //        foreach (string profilePath in profilesPaths)
        //        {
        //            var dirName = new DirectoryInfo(profilePath).Name;
        //            var jsonText = System.IO.File.ReadAllText(profilePath);

        //            JsonSerializerOptions ControlAppJsonSerializerOptions = new JsonSerializerOptions
        //            {
        //                WriteIndented = true,
        //                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        //                IncludeFields = true,

        //                Converters =
        //            {
        //                new JsonStringEnumConverter()
        //            }
        //            };

        //            ProfileData data = JsonSerializer.Deserialize<ProfileData>(jsonText, ControlAppJsonSerializerOptions);
        //            data.DiskFileName = dirName;
        //            profilesOnDisk.Add(data);
        //        }
        //    }

        //    return profilesOnDisk;
        //}

        public ProfileData GetProfile(Guid profileGuid)
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

        //public void SaveDeviceSpecificDataToDisk(DeviceSpecificData device)
        //{
        //    // Save profile to disk
        //    string profileJson = JsonSerializer.Serialize(device, ControlAppJsonSerializerOptions);

        //    System.IO.Directory.CreateDirectory(DevicesFolderFullPath);
        //    System.IO.File.WriteAllText($@"{DevicesFolderFullPath}{device.DeviceMac}.json", profileJson);

        //    UpdateDsHidMiniConfigFile();
        //}


        public void SaveChangesAndUpdateDsHidMiniConfigFile()
        {
            controlAppSettings.Save();
            UpdateDsHidMiniConfigFile();
        }

        public void UpdateDsHidMiniConfigFile()
        {
            var dshmSettings = new DshmSettings();
            
            GlobalProfile.DataContainer.ConvertAllToDSHM(dshmSettings.Global);
           
            foreach(DeviceSpecificData dev in controlAppSettings.Devices)
            {
                var temp = new DshmDeviceSettings();
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
            
            string profileJson = JsonSerializer.Serialize(dshmSettings, ControlAppJsonSerializerOptions);

            System.IO.Directory.CreateDirectory(DshmFolderFullPath);
            System.IO.File.WriteAllText($@"{DshmFolderFullPath}DsHidMini.json", profileJson);
        }

        public void CreateNewProfile(string profileName)
        {
            ProfileData newProfile = new();
            newProfile.ProfileName = profileName;
            //newProfile.DiskFileName = profileName + ".json";
            controlAppSettings.Profiles.Add(newProfile);
        }

        public void DeleteProfile(ProfileData profile)
        {
            if (profile == ProfileData.DefaultProfile) // Must not save Default Profile to disk
            {
                return;
            }
            controlAppSettings.Profiles.Remove(profile);
            FixDevicesWithBlankProfiles();
        }

        public DeviceSpecificData GetDeviceData(string deviceMac)
        {
            foreach (DeviceSpecificData dev in controlAppSettings.Devices)
            {
                if (dev.DeviceMac == deviceMac)
                {
                    return dev;
                }
            }
            var newDevice = new DeviceSpecificData(deviceMac);
            newDevice.DeviceMac = deviceMac;
            controlAppSettings.Devices.Add(newDevice);
            return newDevice;
        }

    }
}