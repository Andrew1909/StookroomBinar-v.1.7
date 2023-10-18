using StockroomBinar.BD;
using StockroomBinar.Class;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для ManufacturerInfoPage.xaml
    /// </summary>
    public partial class ManufacturerInfoPage : Page
    {
        public ManufacturerInfoPage()
        {
            InitializeComponent();
            ManufacturerView.ItemsSource = Connect.bd.IDManufacturer.ToList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var a = ManufacturerView.SelectedItem as IDManufacturer;
            if (a != null)
            {
                MyFrame.Navigate(new EditManufacturerInfoPage(a));
            }
        }

        private void ManufactSite_Click(object sender, RoutedEventArgs e)
        {

            var a = ManufacturerView.SelectedItem as IDManufacturer;
            if (a != null)
            {
                if (a.Site != null ||a.Site!="")
                {
                    System.Diagnostics.Process.Start($"{a.Site}");
                }                
            }
        }

        private void AddNewManufacturer_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddNewManufacturerPage());
        }

        private void Serch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ManufacturerView.ItemsSource = Connect.bd.IDManufacturer.Where(p => p.NameManufacturer.StartsWith(Serch.Text)).ToList();
        }
    }
}
