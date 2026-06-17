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
using System.Text.RegularExpressions;

namespace OfficeCreator
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class OCWindow : Window
    {
        public OCWindow()
        {   
            InitializeComponent();
            //adding data contex, pulling info from where
            DataContext = new OfficeCreator.ViewModel.ViewModel();
        }
        
        //mozliwosc przyjmowania tylko cyfr
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
