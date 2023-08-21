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
            { typeof(GroupModeUniqueVM), "Template_Unique_All"},
            { typeof(GroupLEDsCustomsVM), "Template_LEDsSettings"},
            { typeof(GroupWirelessSettingsVM), "Template_WirelessSettings"},
            { typeof(GroupSticksVM), "Template_SticksDeadZone"},
            { typeof(GroupRumbleGeneralVM), "Template_RumbleBasicFunctions"},
            { typeof(GroupOutRepControlVM), "Template_OutputRateControl"},
            { typeof(GroupRumbleLeftRescaleVM), "Template_RumbleHeavyStrRescale"},
            { typeof(GroupRumbleRightConversionAdjustsVM), "Template_RumbleVariableLightEmuTuning"},

        };
    }
}
