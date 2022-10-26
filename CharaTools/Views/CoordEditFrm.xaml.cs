using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace CharaTools.Views
{
    /// <summary>
    /// Interaction logic for CoordEditFrm.xaml
    /// </summary>
    public partial class CoordEditFrm : Window
    {
        public CoordEditor Editor { get; set; }

        public CoordEditFrm()
        {
            InitializeComponent();
            Loaded += CoordEditFrm_Loaded;
        }

        private void CoordEditFrm_Loaded(object sender, RoutedEventArgs e)
        {
            if (Editor == null) return;

            var srcClothes = Editor.clothes.Select(x => Models.CoordPartListData.From(x.Key, x.Value.PartsInfo, x.Value.UARInfo != null, JsonConvert.SerializeObject(x.Value))).ToList();
            listClothes.ItemsSource = srcClothes;

            var srcAccessories = Editor.accessories.Select((x, i) => Models.CoordPartListData.From(i, x.PartsInfo, x.UARInfo != null, JsonConvert.SerializeObject(x))).ToList();
            listAccessories.ItemsSource = srcAccessories;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
