using Dshm_ControlApp_WpfUi;
using Nefarius.DsHidMini.ControlApp.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nefarius.DsHidMini.ControlApp.Helpers.WPF
{
    internal class SettingsGroupsDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            if(item != null)
            {
                if (GroupSettingsTemplateIndex.TryGetValue(item.GetType(), out string templateKey))
                {
                    return Application.Current.TryFindResource(templateKey) as DataTemplate;
                }
            }
            return null;
        }

        public Dictionary<Type, string> GroupSettingsTemplateIndex = new Dictionary<Type, string>()
        {
            { typeof(HidModeSettingsViewModel), "Template_Unique_All"},
            { typeof(LedsSettingsViewModel), "Template_LEDsSettings"},
            { typeof(WirelessSettingsViewModel), "Template_WirelessSettings"},
            { typeof(SticksSettingsViewModel), "Template_SticksDeadZone"},
            { typeof(GeneralRumbleSettingsViewModel), "Template_RumbleBasicFunctions"},
            { typeof(OutputReportSettingsViewModel), "Template_OutputRateControl"},
            { typeof(LeftMotorRescalingSettingsViewModel), "Template_RumbleHeavyStrRescale"},
            { typeof(AltRumbleModeSettingsViewModel), "Template_RumbleVariableLightEmuTuning"},

        };
    }
}
