﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Dshm_ControlApp_WpfUi.ViewModels.Pages;
using Dshm_ControlApp_WpfUi.ViewModels.Windows;
using Nefarius.DsHidMini.ControlApp.Drivers;
using Nefarius.DsHidMini.ControlApp.MVVM;
using Nefarius.Utilities.DeviceManagement.PnP;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace Dshm_ControlApp_WpfUi.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        private readonly DeviceNotificationListener _listener = new DeviceNotificationListener();

        public DevicesViewModel DevicesVM = App.GetService<DevicesViewModel>();
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel, //
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            ISnackbarService snackbarService,
            IContentDialogService contentDialogService
        )
        {
 

            ViewModel = viewModel;
            DataContext = this;

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);
            snackbarService.SetSnackbarPresenter(SnackbarPresenter);
            contentDialogService.SetContentPresenter(RootContentDialog);

            


        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            InitializeComponent();
            RefreshDevicesList();


            _listener.DeviceArrived += ListenerOnDeviceArrived;
            _listener.DeviceRemoved += ListenerOnDeviceRemoved;

            _listener.StartListen(DsHidMiniDriver.DeviceInterfaceGuid);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _listener.StopListen();
            _listener.Dispose();
        }

        /// <summary>
        ///     DsHidMini device disconnected.
        /// </summary>
        /// <param name="obj">The device path.</param>
        private void ListenerOnDeviceRemoved(DeviceEventArgs e)
        {
            RefreshDevicesList();
        }

        /// <summary>
        ///     DsHidMini device connected.
        /// </summary>
        /// <param name="obj">The device path.</param>
        private void ListenerOnDeviceArrived(DeviceEventArgs e)
        {
            RefreshDevicesList();
        }

        private void RefreshDevicesList()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DevicesVM.Devices.Clear();
                var instance = 0;
                while (Devcon.FindByInterfaceGuid(DsHidMiniDriver.DeviceInterfaceGuid, out var path, out var instanceId, instance++))
                {
                    DevicesVM.Devices.Add(new TestViewModel(PnPDevice.GetDeviceByInstanceId(instanceId)));
                }
            }));
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) =>
            RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
        #endregion INavigationWindow methods
    }
}