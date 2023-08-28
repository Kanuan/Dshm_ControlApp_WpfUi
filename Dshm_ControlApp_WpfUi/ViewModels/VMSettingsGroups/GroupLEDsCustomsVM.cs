﻿using Nefarius.DsHidMini.ControlApp.UserData;
using static Nefarius.DsHidMini.ControlApp.UserData.BackingData_LEDs.All4LEDsCustoms;

namespace Nefarius.DsHidMini.ControlApp.MVVM
{
    public partial class GroupLEDsCustomsVM : GroupSettingsVM
    {
        private BackingData_LEDs _tempBackingData = new();

        public override SettingsModeGroups Group { get; } = SettingsModeGroups.LEDsControl;
        public int LEDMode
        {
            get => (int)_tempBackingData.LEDMode;
            set
            {
                _tempBackingData.LEDMode = (ControlApp_LEDsModes)value;
                this.OnPropertyChanged(nameof(LEDMode));
            }
        }

        protected override IBackingData _myInterface => _tempBackingData;

        public bool AllowLedsOverride
        {
            get => _tempBackingData.AllowExternalLedsControl;
            set
            {
                _tempBackingData.AllowExternalLedsControl = value;
                this.OnPropertyChanged(nameof(AllowLedsOverride));
            }
        }

        [ObservableProperty] int _currentLEDCustomsIndex = 0;

        [ObservableProperty] LED_VM? _selectedLED_VM = null;

        [ObservableProperty] private LED_VM[]? _leds_VM = new LED_VM[] { new (1), new (2), new (3), new (4), };

        partial void OnCurrentLEDCustomsIndexChanged(int value)
        {
            SelectedLED_VM = Leds_VM[value];
        }

        public GroupLEDsCustomsVM()
        {
            Leds_VM[0].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[0];
            Leds_VM[1].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[1];
            Leds_VM[2].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[2];
            Leds_VM[3].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[3];
            SelectedLED_VM = Leds_VM[0];
        }

        public override void NotifyAllPropertiesHaveChanged()
        {
            base.NotifyAllPropertiesHaveChanged();
            foreach (LED_VM ledVM in Leds_VM)
            {
                ledVM.RaisePropertyChangedForAll();
            }
        }

        //public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_LEDs.CopySettings(dataContainerSource.ledsData, _tempBackingData);
        //}

        //public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        //{
        //    BackingData_LEDs.CopySettings(_tempBackingData, dataContainerSource.ledsData);
        //    NotifyAllPropertiesHaveChanged();
        //}


        public partial class LED_VM : ObservableObject
        {
            public singleLEDCustoms singleLEDCustoms;

            [ObservableProperty] int _ledOrder = 0;
            public bool IsEnabled
            {
                get => singleLEDCustoms.IsLedEnabled;
                set
                {
                    singleLEDCustoms.IsLedEnabled = value;
                    this.OnPropertyChanged(nameof(IsEnabled));
                }
            }
            public bool UseEffects
            {
                get => singleLEDCustoms.UseLEDEffects;
                set
                {
                    singleLEDCustoms.UseLEDEffects = value;
                    this.OnPropertyChanged(nameof(UseEffects));
                }
            }
            public byte Duration
            {
                get => singleLEDCustoms.Duration;
                set
                {
                    singleLEDCustoms.Duration = value;
                    this.OnPropertyChanged(nameof(Duration));
                }
            }
            public int IntervalDuration
            {
                get => singleLEDCustoms.CycleDuration;
                set
                {
                    singleLEDCustoms.CycleDuration = value;
                    
                    this.OnPropertyChanged(nameof(IntervalDuration));
                }
            }
            public int IntervalPortionON
            {
                get => singleLEDCustoms.OnPeriodCycles;
                set
                {
                    singleLEDCustoms.OnPeriodCycles = (byte)value;
                    this.OnPropertyChanged(nameof(IntervalPortionON));
                }
            }
            public byte IntervalPortionOFF
            {
                get => singleLEDCustoms.OffPeriodCycles;
                set
                {
                    singleLEDCustoms.OffPeriodCycles = value;
                    this.OnPropertyChanged(nameof(IntervalPortionOFF));
                }
            }

            public LED_VM(int ledOrder)
            {
                _ledOrder = ledOrder;
            }

            public void RaisePropertyChangedForAll()
            {
                this.OnPropertyChanged(string.Empty);
            }
        }

    }


}
