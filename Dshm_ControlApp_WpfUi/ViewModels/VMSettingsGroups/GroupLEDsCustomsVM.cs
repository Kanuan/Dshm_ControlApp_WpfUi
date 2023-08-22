using Nefarius.DsHidMini.ControlApp.DSHM_Settings;
using Nefarius.DsHidMini.ControlApp.UserData;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Text.Json.Serialization;
using System.Windows;
using static Nefarius.DsHidMini.ControlApp.MVVM.GroupLEDsCustomsVM;
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

        [ObservableProperty] int _currentLEDCustomsIndex = 0;

        [ObservableProperty] LED_VM? _selectedLED_VM = null;

        [ObservableProperty] private LED_VM[]? _leds_VM = new LED_VM[] { new (1), new (2), new (3), new (4), };

        partial void OnCurrentLEDCustomsIndexChanged(int value)
        {
            SelectedLED_VM = Leds_VM[value];
        }

        public GroupLEDsCustomsVM(BackingDataContainer backingDataContainer) : base(backingDataContainer)
        {
            Leds_VM[0].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[0];
            Leds_VM[1].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[1];
            Leds_VM[2].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[2];
            Leds_VM[3].singleLEDCustoms = _tempBackingData.LEDsCustoms.LED_x_Customs[3];
            SelectedLED_VM = Leds_VM[0];
        }

        public override void ResetGroupToOriginalDefaults()
        {
            _tempBackingData.ResetToDefault();

            this.OnPropertyChanged(string.Empty);
            foreach(LED_VM ledVM in Leds_VM)
            {
                ledVM.RaisePropertyChangedForAll();
            } 
        }

        public override void SaveSettingsToBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            SaveSettingsToBackingData(dataContainerSource.ledsData);
        }

        public void SaveSettingsToBackingData(BackingData_LEDs dataSource)
        {
            BackingData_LEDs.CopySettings(dataSource, _tempBackingData);
        }

        public override void LoadSettingsFromBackingDataContainer(BackingDataContainer dataContainerSource)
        {
            LoadSettingsFromBackingData(dataContainerSource.ledsData);
        }

        public void LoadSettingsFromBackingData(BackingData_LEDs dataTarget)
        {
            BackingData_LEDs.CopySettings(_tempBackingData, dataTarget);

            this.OnPropertyChanged(string.Empty);
            foreach (LED_VM ledVM in Leds_VM)
            {
                ledVM.RaisePropertyChangedForAll();
            }
        }

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
            public byte IntervalDuration
            {
                get => singleLEDCustoms.IntervalDuration;
                set
                {
                    singleLEDCustoms.IntervalDuration = value;
                    this.OnPropertyChanged(nameof(IntervalDuration));
                }
            }
            public string IntervalPortionON
            {
                get => singleLEDCustoms.IntervalPortionON.ToString();
                set
                {
                    byte.TryParse(value, out byte valueInNumber);
                    singleLEDCustoms.IntervalPortionON = valueInNumber;
                    this.OnPropertyChanged(nameof(IntervalPortionON));
                    this.OnPropertyChanged(nameof(IntervalPortionOFF));
                }
            }
            public byte IntervalPortionOFF
            {
                get => singleLEDCustoms.IntervalPortionOFF;
                set
                {
                    //singleLEDCustoms.IntervalPortionOFF = value;
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
