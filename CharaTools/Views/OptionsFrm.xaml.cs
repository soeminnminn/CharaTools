using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CharaTools.Views
{
    /// <summary>
    /// Interaction logic for OptionsFrm.xaml
    /// </summary>
    public partial class OptionsFrm : Window
    {
        private Models.ConfigModel config = null;

        public OptionsFrm()
        {
            InitializeComponent();
            config = (Models.ConfigModel)Models.ConfigModel.Instance.Clone();
            DataContext = config;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (config.Save())
                Models.ConfigModel.Instance.Load();

            Close();
        }
    }
}
