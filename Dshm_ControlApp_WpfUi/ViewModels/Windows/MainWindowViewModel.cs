// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Nefarius.DsHidMini.ControlApp.MVVM;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace Dshm_ControlApp_WpfUi.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "DsHidMini ControlApp";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Devices",
                Icon = new SymbolIcon { Symbol = SymbolRegular.XboxController48 },
                TargetPageType = typeof(Views.Pages.DevicesPage)
            },
            new NavigationViewItem()
            {
                Content = "Profiles",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentOnePage20 },
                TargetPageType = typeof(Views.Pages.ProfileEditorPage)
            },
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };


    }
}
