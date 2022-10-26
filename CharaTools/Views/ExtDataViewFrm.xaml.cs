using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CharaTools.Views
{
    /// <summary>
    /// Interaction logic for ExtDataViewFrm.xaml
    /// </summary>
    public partial class ExtDataViewFrm : Window
    {
        #region Variables
        private AIChara.PluginData pluginData = null;

        private SaveFileDialog saveFileDialog = null;
        #endregion

        #region Constructor
        public ExtDataViewFrm()
        {
            InitializeComponent();

            saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON file|*.json"
            };

            Loaded += ExtDataViewFrm_Loaded;
        }
        #endregion

        #region Properties
        public AIChara.ChaFileControl.KKExData KKExData { get; set; }

        public string ExtKey { get; set; }

        public string FileName { get; set; } = string.Empty;
        #endregion

        #region Methods
        private void ExtDataViewFrm_Loaded(object sender, RoutedEventArgs e)
        {
            Title = $"Extended Data View - {ExtKey}";

            if (KKExData != null && KKExData.Data != null)
            {
                if (KKExData.Data.TryGetValue(ExtKey, out pluginData))
                {
                    if (pluginData != null)
                    {
                        jsonTree.Source = pluginData;
                        //var jsonString = JsonConvert.SerializeObject(pluginData);

                        //var dataSource = new Models.JsonTreeItem();
                        //dataSource.ShowJson(jsonString);
                        //jsonTree.ItemsSource = Models.JsonTreeItem.Root;
                    }
                }
            }
        }
        #endregion

        #region Controls Events
        private void btnExportJson_Click(object sender, RoutedEventArgs e)
        {
            if (KKExData != null && pluginData != null)
            {
                string fileName = FileName;
                if (!string.IsNullOrEmpty(fileName))
                    fileName += "_";

                fileName += ExtKey + ".json";

                saveFileDialog.FileName = fileName;
                if (saveFileDialog.ShowDialog(this) == true)
                {
                    try
                    {
                        var settings = new JsonSerializerSettings();

                        if (!Models.ConfigModel.Instance.MinifyJSON)
                            settings.Formatting = Formatting.Indented;

                        var jsonString = JsonConvert.SerializeObject(pluginData, settings);
                        File.WriteAllText(saveFileDialog.FileName, jsonString);
                    }
                    catch (Exception)
                    { }
                }
            }
        }
        #endregion
    }
}
