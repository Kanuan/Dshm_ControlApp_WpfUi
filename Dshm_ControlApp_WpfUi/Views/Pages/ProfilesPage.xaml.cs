// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Dshm_ControlApp_WpfUi.ViewModels.Pages;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Wpf.Ui.Controls;

namespace Dshm_ControlApp_WpfUi.Views.Pages
{
    public partial class ProfilesPage : INavigableView<ProfileEditorViewModel>
    {
        public ProfileEditorViewModel ViewModel { get; }

        public ProfilesPage(ProfileEditorViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
