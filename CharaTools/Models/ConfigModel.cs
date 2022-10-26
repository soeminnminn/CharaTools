using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using CharaTools.Observable;

namespace CharaTools.Models
{
    public class ConfigModel : ObservableObject, ICloneable
    {
        #region Static
        private static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string appDataConfigDir = string.Empty;

        public static string AppDataConfigDir
        {
            get
            {
                if (!string.IsNullOrEmpty(appDataConfigDir)) return appDataConfigDir;

                var assm = System.Reflection.Assembly.GetEntryAssembly();
                if (assm != null)
                {
                    var versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assm.Location);
                    var appName = versionInfo.ProductName ?? assm.GetName()?.Name ?? "";
                    var companyName = versionInfo.CompanyName ?? "";

                    var configDir = Path.Combine(appDataPath, companyName, appName);
                    if (!Directory.Exists(configDir))
                    {
                        try
                        {
                            var dirInfo = Directory.CreateDirectory(configDir);
                            if (dirInfo.Exists)
                            {
                                appDataConfigDir = dirInfo.FullName;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        appDataConfigDir = configDir;
                    }

                }
                return appDataConfigDir;
            }
        }

        private static ConfigModel instance = null;
        public static ConfigModel Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ConfigModel();
                    instance.Load();
                }
                return instance;
            }
        }
        #endregion

        #region Config Properties
        private bool excludeExtData = false;
        public bool ExcludeExtData 
        {
            get => excludeExtData;
            set { SetProperty(ref excludeExtData, value); }
        }

        private bool minifyJSON = false;
        public bool MinifyJSON
        {
            get => minifyJSON;
            set { SetProperty(ref minifyJSON, value); }
        }

        private int imageSizeType = 0;
        public int ImageSizeType
        {
            get => imageSizeType;
            set { SetProperty(ref imageSizeType, value); }
        }

        private bool drawBkgImage = true;
        public bool DrawBkgImage
        {
            get => drawBkgImage;
            set { SetProperty(ref drawBkgImage, value); }
        }

        private bool drawFrame = false;
        public bool DrawFrame
        {
            get => drawFrame;
            set { SetProperty(ref drawFrame, value); }
        }

        private Color bkgColor = Color.FromRgb(126, 116, 146);
        public Color BkgColor
        {
            get => bkgColor;
            set { SetProperty(ref bkgColor, value); }
        }

        private bool clearEmptyExtData = false;
        public bool ClearEmptyExtData
        {
            get => clearEmptyExtData;
            set { SetProperty(ref clearEmptyExtData, value); }
        }
        #endregion

        #region Constructor
        public ConfigModel()
        {
        }
        #endregion

        #region Methods
        public void Load(string fileName = "config.json")
        {
            var filePath = Path.Combine(AppDataConfigDir, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    if (!string.IsNullOrEmpty(json))
                    {
                        var config = JsonConvert.DeserializeObject<ConfigModel>(json);
                        if (config != null)
                        {
                            ExcludeExtData = config.ExcludeExtData;
                            MinifyJSON = config.MinifyJSON;
                            ImageSizeType = config.ImageSizeType;
                            DrawBkgImage = config.DrawBkgImage;
                            DrawFrame = config.DrawFrame;
                            BkgColor = config.BkgColor;
                            ClearEmptyExtData = config.ClearEmptyExtData;
                        }
                    }
                }
                catch (Exception)
                { }
            }
        }

        public bool Save(string fileName = "config.json")
        {
            try
            {
                var filePath = Path.Combine(AppDataConfigDir, fileName);
                var json = JsonConvert.SerializeObject(this);
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
        #endregion
    }
}
