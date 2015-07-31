using System.Configuration;

namespace TimeNazi.Properties
{


    // This class allows you to handle specific events on the settings class:
    //  The SettingChanging event is raised before a setting's value is changed.
    //  The PropertyChanged event is raised after a setting's value is changed.
    //  The SettingsLoaded event is raised after the setting values are loaded.
    //  The SettingsSaving event is raised before the setting values are saved.
    internal sealed partial class Settings
    {

        public Settings()
        {
            // // To add event handlers for saving and changing settings, uncomment the lines below:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //

            // http://stackoverflow.com/a/13807361
            // add the custom settings provider to all of the properties
            SettingsProvider provider = new CustomSettingsProvider();

            // Try to re-use an existing provider, since we cannot have multiple providers
            // with same name.
            if (Providers[provider.Name] == null)
            {
                Providers.Add(provider);
            }
            else
            {
                provider = Providers[provider.Name];
            }

            // Change default provider.
            foreach (SettingsProperty property in Properties)
            {
                if (property.PropertyType.GetCustomAttributes(typeof(SettingsProviderAttribute), false).Length == 0)
                {
                    property.Provider = provider;
                }
            }
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // Add code to handle the SettingChangingEvent event here.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Add code to handle the SettingsSaving event here.
        }
    }
}
