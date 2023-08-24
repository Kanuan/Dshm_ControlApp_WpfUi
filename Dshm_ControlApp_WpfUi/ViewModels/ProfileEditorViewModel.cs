using DynamicData.Binding;
using Nefarius.DsHidMini.ControlApp.UserData;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{

    public partial class ProfileEditorViewModel : ObservableObject, INavigationAware
    {
        // ----------------------------------------------------------- FIELDS

        [ObservableProperty] public List<ProfileViewModel> _profilesViewModels;
        [ObservableProperty] private ProfileViewModel? _selectedProfileVM = null;
        [ObservableProperty] bool _isEditing = false;

        // ----------------------------------------------------------- PROPERTIES

        public List<ProfileData> ProfilesDatas => TestViewModel.UserDataManager.Profiles;

        private readonly ISnackbarService _snackbarService;

        // ----------------------------------------------------------- CONSTRUCTOR

        public ProfileEditorViewModel(ISnackbarService snackbarService)
        {
            _snackbarService = snackbarService;
            UpdateProfileList();
        }
     

        public void UpdateProfileList()
        {
            List<ProfileViewModel> newList = new();
            foreach(ProfileData prof in ProfilesDatas)
            {
                newList.Add(new(prof) { IsGlobal = (prof == TestViewModel.UserDataManager.GlobalProfile)});
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
                TestViewModel.UserDataManager.GlobalProfile = obj._profileData;
                ShowSnackbarMessage("Global profile updated.", "", ControlAppearance.Info, new SymbolIcon(SymbolRegular.Checkmark24), 2);
            }
            TestViewModel.UserDataManager.SaveControlAppSettingsToDisk();
            UpdateProfileList();
        }

        [RelayCommand]
        private void CreateProfile()
        {
            TestViewModel.UserDataManager.CreateNewProfile("New profile");
            UpdateProfileList();
        }

        [RelayCommand]
        private void DeleteProfile(ProfileViewModel? obj)
        {
            if (obj == null) return;
            TestViewModel.UserDataManager.DeleteProfile(obj._profileData);
            UpdateProfileList();
        }
            
        }

        public partial class ProfileViewModel : ObservableObject
        {
            ControllersUserData _userDataManager = TestViewModel.UserDataManager;

            public readonly ProfileData _profileData;

            [ObservableProperty] private string _name;
            [ObservableProperty] private VMGroupsContainer _vmGroupsCont;
            [ObservableProperty] private bool _isGlobal = false;


            public ProfileViewModel(ProfileData data)
            {
                _profileData = data;
                _name = data.ProfileName;
                _vmGroupsCont = data.GetProfileVMGroupsContainer();
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
                TestViewModel.UserDataManager.SaveProfileToDisk(_profileData);
            }

        [RelayCommand]
        public void CancelChanges()
            {
                VmGroupsCont.LoadDatasToAllGroups(_profileData.DataContainer);
                Name = _profileData.ProfileName;
            }
        }

}