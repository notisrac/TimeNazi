using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace TimeNazi
{
    public class CustomSettingsProvider : SettingsProvider
    {
        public override string ApplicationName
        {
            get { return Application.ProductName; }
            set { }
        }

        private string _sFilePath = string.Empty;
        public string FilePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sFilePath))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                    var companyName = versionInfo.CompanyName;
                    var appName = versionInfo.ProductName;
                    _sFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appName, "user.config");
                    //_sFilePath = Path.Combine(Application.LocalUserAppDataPath.Replace(Application.ProductVersion, ""), "user.config");
                }
                return _sFilePath;
            }
        }

        public override string Name
        {
            get
            {
                return "CustomSettingsProvider";
            }
        }

        private Dictionary<string, object> _config;

        public Dictionary<string, object> Config
        {
            get
            {
                if(null == _config)
                {
                    _config = new Dictionary<string, object>();
                }
                return _config;
            }
            set { _config = value; }
        }


        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(this.ApplicationName, config);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection spvcRet = new SettingsPropertyValueCollection();
            if (Config.Count == 0)
            {
                _loadConfig(context["GroupName"] as string);
            }

            foreach (SettingsProperty settingsProperty in collection)
            {
                SettingsPropertyValue spvValue = new SettingsPropertyValue(settingsProperty);
                spvValue.IsDirty = false;
                if (Config.Count != 0 && Config.ContainsKey(settingsProperty.Name))
                {
                    spvValue.SerializedValue = Config[settingsProperty.Name];
                }
                spvcRet.Add(spvValue);
            }

            return spvcRet;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue spvValue in collection)
            {
                if (Config.ContainsKey(spvValue.Name))
                {
                    Config[spvValue.Name] = spvValue.PropertyValue ?? "";
                }
                else
                {
                    Config.Add(spvValue.Name, spvValue.PropertyValue ?? "");
                }
            }

            _saveConfig(context["GroupName"] as string);
        }

        private bool IsUserScoped(SettingsProperty property)
        {
            return property.Attributes.ContainsKey(typeof(UserScopedSettingAttribute));
        }

        private void _loadConfig(string sectionName)
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    using (XmlTextReader xtrReader = new XmlTextReader(FilePath))
                    {
                        xtrReader.ReadToFollowing(sectionName);

                        while (xtrReader.ReadToFollowing("setting"))
                        {
                            string sAttributeName = xtrReader.GetAttribute("name");
                            xtrReader.ReadToDescendant("value");
                            string sValue = xtrReader.ReadElementContentAsString();
                            Config.Add(sAttributeName, sValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void _saveConfig(string sectionName)
        {
            try
            {
                string sDirName = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(sDirName))
                {
                    Directory.CreateDirectory(sDirName);
                }

                using (XmlTextWriter xwWriter = new XmlTextWriter(FilePath, Encoding.UTF8))
                {
                    xwWriter.Formatting = Formatting.Indented;
                    xwWriter.WriteStartDocument();
                    xwWriter.WriteStartElement("configuration");
                    xwWriter.WriteStartElement("userSettings");
                    xwWriter.WriteStartElement(sectionName);
                    foreach (var entry in Config)
                    {
                        xwWriter.WriteStartElement("setting");
                        xwWriter.WriteAttributeString("name", entry.Key);
                        xwWriter.WriteAttributeString("serializeAs", entry.Key.GetType().Name);
                        xwWriter.WriteStartElement("value");
                        xwWriter.WriteValue(entry.Value.ToConfigString());
                        xwWriter.WriteEndElement();
                        xwWriter.WriteEndElement();
                    }
                    xwWriter.WriteEndElement();
                    xwWriter.WriteEndElement();
                    xwWriter.WriteEndElement();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public static partial class Extensions
    {
        public static object ToConfigString(this object input)
        {
            if (null == input)
            {
                return string.Empty;
            }

            if (input.GetType() == typeof(System.Drawing.Point))
            {
                return string.Format("{0}, {1}", ((System.Drawing.Point)input).X, ((System.Drawing.Point)input).Y);
            }

            return input.ToString();
        }
    }
}
