using System.Diagnostics.Metrics;
using Nefarius.DsHidMini.ControlApp.Models.DshmConfigManager;
using Nefarius.DsHidMini.ControlApp.Services;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.ViewModels.Pages
{

    public partial class ProfilesViewModel : ObservableObject, INavigationAware
    {
        // ----------------------------------------------------------- FIELDS

        private readonly DshmConfigManager _dshmConfigManager;

        [ObservableProperty] public List<ProfileViewModel> _profilesViewModels;
        [ObservableProperty] private ProfileViewModel? _selectedProfileVM = null;
        [ObservableProperty] bool _isEditing = false;

        // ----------------------------------------------------------- PROPERTIES

        public List<ProfileData> ProfilesDatas => _dshmConfigManager.Profiles;

        private readonly AppSnackbarMessagesService _appSnackbarMessagesService;

        // ----------------------------------------------------------- CONSTRUCTOR

        public ProfilesViewModel(AppSnackbarMessagesService appSnackbarMessagesService, DshmConfigManager dshmConfigManager)
        {
            _dshmConfigManager = dshmConfigManager;
            _appSnackbarMessagesService = appSnackbarMessagesService;
            UpdateProfileList();
        }
     

        public void UpdateProfileList()
        {
            List<ProfileViewModel> newList = new();
            foreach(ProfileData prof in ProfilesDatas)
            {
                newList.Add(new(prof) { IsGlobal = (prof == _dshmConfigManager.GlobalProfile)});
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
                _appSnackbarMessagesService.ShowProfileChangedCanceledMessage();
            }
        }

        public void OnNavigatedTo()
        {
        }



        [RelayCommand]
        private void EnableEditingOfSelectedProfile()
        {
            if (SelectedProfileVM == null) return;
            if (SelectedProfileVM._profileData == ProfileData.DefaultProfile)
            {
                _appSnackbarMessagesService.ShowDefaultProfileEditingBlockedMessage();
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
            _appSnackbarMessagesService.ShowProfileUpdateMessage();
            IsEditing = false;
            _dshmConfigManager.SaveChangesAndUpdateDsHidMiniConfigFile();
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
                _dshmConfigManager.GlobalProfile = obj._profileData;
                _appSnackbarMessagesService.ShowGlobalProfileUpdatedMessage();
                _dshmConfigManager.SaveChangesAndUpdateDsHidMiniConfigFile();
            }
            UpdateProfileList();
        }

        [RelayCommand]
        private void CreateProfile()
        {
            _dshmConfigManager.CreateNewProfile("New profile");
            _dshmConfigManager.SaveChanges();
            UpdateProfileList();
        }

        [RelayCommand]
        private void DeleteProfile(ProfileViewModel? obj)
        {
            if (obj == null) return;
            if (obj._profileData == ProfileData.DefaultProfile)
            {
                _appSnackbarMessagesService.ShowDefaultProfileEditingBlockedMessage();
                return;
            }
            _dshmConfigManager.DeleteProfile(obj._profileData);
            _dshmConfigManager.SaveChangesAndUpdateDsHidMiniConfigFile();
            _appSnackbarMessagesService.ShowProfileDeletedMessage();
            UpdateProfileList();
        }
            
        }
}