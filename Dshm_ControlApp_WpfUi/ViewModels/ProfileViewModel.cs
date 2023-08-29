using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.ViewModels.UserControls;

namespace Nefarius.DsHidMini.ControlApp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    public readonly ProfileData _profileData;

    [ObservableProperty] private string _name;
    [ObservableProperty] private SettingsEditorViewModel _vmGroupsCont;
    [ObservableProperty] private bool _isGlobal = false;


    public ProfileViewModel(ProfileData data)
    {
        _profileData = data;
        _name = data.ProfileName;
        _vmGroupsCont = new(data.DeviceSettings);
    }

    [RelayCommand]
    public void SaveChanges()
    {
        if(_name == null)
        {
            _name = "User Profile";
        }
        _profileData.ProfileName = _name;
        VmGroupsCont.SaveAllChangesToBackingData(_profileData.DeviceSettings);
            
    }

    [RelayCommand]
    public void CancelChanges()
    {
        VmGroupsCont.LoadDatasToAllGroups(_profileData.DeviceSettings);
        Name = _profileData.ProfileName;
    }
}