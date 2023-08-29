using Nefarius.DsHidMini.ControlApp.DshmConfigManager;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{

    public partial class ProfilesViewModel : ObservableObject, INavigationAware
    {
        // ----------------------------------------------------------- FIELDS

        [ObservableProperty] public List<ProfileViewModel> _profilesViewModels;
        [ObservableProperty] private ProfileViewModel? _selectedProfileVM = null;
        [ObservableProperty] bool _isEditing = false;

        // ----------------------------------------------------------- PROPERTIES

        public List<ProfileData> ProfilesDatas => DeviceViewModel.UserDataManager.Profiles;

        private readonly ISnackbarService _snackbarService;

        // ----------------------------------------------------------- CONSTRUCTOR

        public ProfilesViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
            UpdateProfileList();
        }
     

        public void UpdateProfileList()
        {
            List<ProfileViewModel> newList = new();
            foreach(ProfileData prof in ProfilesDatas)
            {
                newList.Add(new(prof) { IsGlobal = (prof == DeviceViewModel.UserDataManager.GlobalProfile)});
            }
            ProfilesViewModels = newList;
        }


        // ---------------------------------------- Methods

        partial void OnIsEditingChanged(bool value)
        {
            if(SelectedProfileVM != null)
            {
                SelectedProfileVM.VmGroupsCont.AllowEditing = value;
            }
        }

        public void OnNavigatedFrom() 
        {
            if(IsEditing && SelectedProfileVM != null)
            {
                CancelChangesToProfile();
                ShowSnackbarMessage("Canceled profile changes.", "", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.ErrorCircle24), 2);
            }
        }

        public void OnNavigatedTo()
        {
        }

        private void ShowSnackbarMessage(string title, string message, ControlAppearance appearance, SymbolIcon symbol, int timeSpanInSeconds)
        {
            _snackbarService.Show(
                title,
                message,
                appearance,
                symbol,
                TimeSpan.FromSeconds(timeSpanInSeconds)
            );
        }

        [RelayCommand]
        private void EnableEditingOfSelectedProfile()
        {
            if (SelectedProfileVM == null) return;
            if (SelectedProfileVM._profileData == ProfileData.DefaultProfile)
            {
                ShowSnackbarMessage("ControlApp's default profile can't be modified.", "", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Info24), 2);
            }
            else
            {
                IsEditing = true;
            }
        }

        [RelayCommand]
        private void SaveChangesToProfile()
        {
            SelectedProfileVM.SaveChanges();
            ShowSnackbarMessage("Profile updated.", "", ControlAppearance.Info, new SymbolIcon(SymbolRegular.ErrorCircle24), 2);
            IsEditing = false;
            DeviceViewModel.UserDataManager.SaveChangesAndUpdateDsHidMiniConfigFile();
        }

        [RelayCommand]
        private void CancelChangesToProfile()
        {
            SelectedProfileVM.CancelChanges();
            IsEditing = false;
        }



        [RelayCommand]
        private void SetprofileAsGlobal(ProfileViewModel? obj)
        {
            if (obj != null)
            {
                DeviceViewModel.UserDataManager.GlobalProfile = obj._profileData;
                ShowSnackbarMessage("Global profile updated.", "", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Checkmark24), 2);
                DeviceViewModel.UserDataManager.SaveChangesAndUpdateDsHidMiniConfigFile();
            }
            UpdateProfileList();
        }

        [RelayCommand]
        private void CreateProfile()
        {
            DeviceViewModel.UserDataManager.CreateNewProfile("New profile");
            DeviceViewModel.UserDataManager.SaveChangesAndUpdateDsHidMiniConfigFile();
            UpdateProfileList();
        }

        [RelayCommand]
        private void DeleteProfile(ProfileViewModel? obj)
        {
            if (obj == null) return;
            DeviceViewModel.UserDataManager.DeleteProfile(obj._profileData);
            DeviceViewModel.UserDataManager.SaveChangesAndUpdateDsHidMiniConfigFile();
            UpdateProfileList();
        }
            
        }

        public partial class ProfileViewModel : ObservableObject
        {
        DshmConfigManager.DshmConfigManager _userDataManager = DeviceViewModel.UserDataManager;

            public readonly ProfileData _profileData;

            [ObservableProperty] private string _name;
            [ObservableProperty] private SettingsEditorViewModel _vmGroupsCont;
            [ObservableProperty] private bool _isGlobal = false;


            public ProfileViewModel(ProfileData data)
            {
                _profileData = data;
                _name = data.ProfileName;
                _vmGroupsCont = new(data.DataContainer);
            }

            [RelayCommand]
            public void SaveChanges()
            {
                if(_name == null)
                {
                    _name = "User Profile";
                }
                _profileData.ProfileName = _name;
                VmGroupsCont.SaveAllChangesToBackingData(_profileData.DataContainer);
            
            }

        [RelayCommand]
        public void CancelChanges()
            {
                VmGroupsCont.LoadDatasToAllGroups(_profileData.DataContainer);
                Name = _profileData.ProfileName;
            }
        }

}