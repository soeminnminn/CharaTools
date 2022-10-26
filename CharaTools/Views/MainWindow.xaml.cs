using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;

namespace CharaTools.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        private BackgroundWorker backgroundWorkerLoad = null;
        private OpenFileDialog openFileDialog = null;
        private OpenFileDialog imageFileDialog = null;
        private SaveFileDialog saveFileDialog = null;

        private CharacterEditor chaFileEdit = null;
        private ImageReplacer imageReplacer = null;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }
        #endregion

        #region Methods
        private void Initialize()
        {
            cboBithDay.ItemsSource = Enumerable.Range(1, 31);
            cboBithMonth.ItemsSource = Enumerable.Range(0, 12).Select(x => Constants.sMonth[x]);

            cboPersonality.ItemsSource = Constants.ssPersonality;
            cboTrait.ItemsSource = Constants.ssTrait;
            cboMentality.ItemsSource = Constants.ssMentality;
            cboSexTrait.ItemsSource = Constants.ssSexTrait;

            txtVoicePitch.Minimum = -100;
            txtVoicePitch.Maximum = 200;

            SetUpDownMinMax(txtFavoritePlace);
            SetUpDownMinMax(txtLifeStyle);
            SetUpDownMinMax(txtMorality);
            SetUpDownMinMax(txtMotivation);
            SetUpDownMinMax(txtImmoral);

            txtHCount.Minimum = 0;
            txtHCount.Maximum = int.MaxValue;

            SetUpDownMinMax(txtAlertness);
            SetUpDownMinMax(txtUsedItem);

            SetUpDownMinMax(txtResistH);
            SetUpDownMinMax(txtResistPain);
            SetUpDownMinMax(txtResistAnal);

            backgroundWorkerLoad = new BackgroundWorker();
            backgroundWorkerLoad.DoWork += BackgroundWorkerLoad_DoWork;
            backgroundWorkerLoad.RunWorkerCompleted += BackgroundWorkerLoad_RunWorkerCompleted;

            openFileDialog = new OpenFileDialog() 
            {
                Filter = "AI/HS2 Chara File|*.png"
            };

            imageFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp"
            };

            saveFileDialog = new SaveFileDialog()
            {
                Filter = "AI/HS2 Chara File|*.png"
            };

            Plugin.Manager.SendOnPluginLoaded();

            chaFileEdit = new CharacterEditor();
            DataContext = chaFileEdit;

            EnabledControls(true, true);
        }

        private void SetUpDownMinMax(Controls.NumericTextBox ctrl)
        {
            ctrl.Minimum = -100;
            ctrl.Maximum = 100;
        }

        private void EnabledControls(bool enabled, bool isEmpty)
        {
            //
        }

        #region Commands
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == true)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = chaFileEdit != null && chaFileEdit.HasChanged;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            chaFileEdit?.Save(null, imageReplacer?.PngData,
                Models.ConfigModel.Instance.ClearEmptyExtData);
        }

        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = chaFileEdit != null && chaFileEdit.HasChanged;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (chaFileEdit != null)
            {
                saveFileDialog.FileName = chaFileEdit.GenerateFileName();

                if (saveFileDialog.ShowDialog(this) == true)
                {
                    chaFileEdit.Save(saveFileDialog.FileName, imageReplacer?.PngData,
                        Models.ConfigModel.Instance.ClearEmptyExtData);
                }
            }
        }

        private void ExportJson_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = chaFileEdit != null && !chaFileEdit.IsEmpty;
        }

        private void ExportJsonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (chaFileEdit == null || chaFileEdit.IsEmpty) return;

            saveFileDialog.Filter = "JSON File|*.json";
            saveFileDialog.FileName = chaFileEdit.CharaFileName + ".json";

            if (saveFileDialog.ShowDialog(this) == true)
            {
                try
                {
                    var jsonString = chaFileEdit.SerializeJson(
                        Models.ConfigModel.Instance.ExcludeExtData, 
                        !Models.ConfigModel.Instance.MinifyJSON);

                    if (!string.IsNullOrEmpty(jsonString))
                        File.WriteAllText(saveFileDialog.FileName, jsonString);
                }
                catch (Exception)
                { }
            }
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void OptionsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var form = new OptionsFrm()
            {
                Owner = this
            };
            form.ShowDialog();
        }

        private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string aboutString = $"Develop by Sulfur {Environment.NewLine}Copyright © 2022 {Environment.NewLine}The Open Source Project";
            string aboutTitle = $"About - {this.Title}";

            MessageBox.Show(this, aboutString, aboutTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Widow Events
        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0)
            {
                var filePath = files.Where(f => f.ToLowerInvariant().EndsWith(".png")).ToArray().FirstOrDefault();
                LoadFile(filePath);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.chaFileEdit?.HasChanged == true)
            {
                var result = MessageBox.Show(this, "Do you want to save changes", this.Title,
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        this.chaFileEdit?.Save();
                        break;
                    default:
                        break;
                }
            }
            base.OnClosing(e);
        }
        #endregion

        #region Worker Events
        private void BackgroundWorkerLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            var filePath = e.Argument as string;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                e.Result = chaFileEdit.Load(filePath);
            }
        }

        private void BackgroundWorkerLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!chaFileEdit.IsEmpty)
            {
                Plugin.Manager.SendOnChaFileLoaded(chaFileEdit.Card);

                if (chaFileEdit.PngData != null && chaFileEdit.PngData.Length > 0)
                {
                    var image = ToImage(chaFileEdit.PngData);
                    imageReplacer = new ImageReplacer(image.PixelWidth, image.PixelHeight);
                    pictureBox.Source = image;
                }

                LoadCoordData();
                LoadExtendedData();

                statusFileName.Content = chaFileEdit.CharaFileName;
            }

            EnabledControls(true, chaFileEdit.IsEmpty);
            statusProgress.IsIndeterminate = false;
        }
        #endregion

        #region Controls Events
        private void listViewExtra_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var itemData = listViewExtra.SelectedItem as Models.ExtendedListData;
            if (itemData != null && !chaFileEdit.IsEmpty)
            {
                string key = itemData.GUID;
                if (chaFileEdit.ExtendedData[key] != null)
                {
                    ExtDataViewFrm viewForm = new ExtDataViewFrm()
                    {
                        FileName = chaFileEdit.CharaFileName,
                        ExtKey = key,
                        KKExData = chaFileEdit.KKEx,
                        Owner = this
                    };

                    viewForm.ShowDialog();
                }
            }
        }

        private void btnCoordEdit_Click(object sender, RoutedEventArgs e)
        {
            if (chaFileEdit.Coordinate == null || chaFileEdit.CoordinateEditor == null) return;

            var form = new CoordEditFrm()
            {
                Owner = this,
                Editor = chaFileEdit.CoordinateEditor
            };
            form.ShowDialog();
        }

        private void btnCoordReplace_Click(object sender, RoutedEventArgs e)
        {
            if (chaFileEdit.Coordinate == null || chaFileEdit.CoordinateEditor == null) return;

            var form = new CoordReplaceFrm()
            {
                Owner = this,
                Editor = chaFileEdit.CoordinateEditor
            };
            form.ShowDialog();
        }

        private void btnReplaceImage_Click(object sender, RoutedEventArgs e)
        {
            if (imageReplacer != null && imageFileDialog.ShowDialog(this) == true)
            {
                var config = Models.ConfigModel.Instance;
                
                imageReplacer.Resolution = (Constants.CardResolutions)config.ImageSizeType;
                imageReplacer.DrawBkgImage = config.DrawBkgImage;
                imageReplacer.DrawFrame = config.DrawFrame;
                imageReplacer.SetBackgroundColor(config.BkgColor.A, config.BkgColor.R, config.BkgColor.G, config.BkgColor.B);

                if (imageReplacer.Load(imageFileDialog.FileName) && imageReplacer.PngData != null)
                {
                    var image = ToImage(imageReplacer.PngData);
                    pictureBox.Source = image;
                }
            }
        }
        #endregion

        private void LoadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;

            EnabledControls(false, true);
            statusProgress.IsIndeterminate = true;
            backgroundWorkerLoad.RunWorkerAsync(filePath);
        }

        private void LoadCoordData()
        {
            listViewCoord.ItemsSource = null;
            if (chaFileEdit.Coordinate == null || chaFileEdit.CoordinateEditor == null) return;

            var listSource = new List<Models.CoordPartListData>();

            var coord = chaFileEdit.CoordinateEditor;
            var clothes = coord.clothes.Select(x => Models.CoordPartListData.From(x.Key, x.Value.PartsInfo, x.Value.UARInfo != null));
            listSource.AddRange(clothes);

            var accs = coord.accessories.Select((x, i) => Models.CoordPartListData.From(i, x.PartsInfo, x.UARInfo != null));
            listSource.AddRange(accs);

            listViewCoord.ItemsSource = listSource;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listViewCoord.ItemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription("PartType"));
        }

        private void LoadExtendedData()
        {
            listViewExtra.ItemsSource = null;

            if (chaFileEdit.ExtendedData != null && chaFileEdit.ExtendedData.Count > 0)
            {
                var list = new List<Models.ExtendedListData>();

                int i = 1;
                foreach (var key in chaFileEdit.ExtendedData.Keys)
                {
                    var item = new Models.ExtendedListData()
                    {
                        Index = i,
                        GUID = key,
                    };

                    if (chaFileEdit.ExtendedData[key] != null)
                        item.Count = chaFileEdit.ExtendedData[key].data.Count.ToString();
                    else
                        item.Count = "[Empty]";

                    list.Add(item);
                    i++;
                }

                listViewExtra.ItemsSource = list;
            }
        }

        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        #endregion
    }
}
