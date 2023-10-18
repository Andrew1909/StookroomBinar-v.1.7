using StockroomBinar.Pages;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace StockroomBinar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MyFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            MyFrame.Navigate(new HomePage());
            DirectoryInfo dirInfo = new DirectoryInfo("C:\\BinarStokroom");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
         dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Profiles");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Profiles\\Profiles3Dprint");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Profiles\\ProfilesEgraving");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Reports");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Blueprints");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Blueprints\\Construction Blueprints");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo = new DirectoryInfo("C:\\BinarStokroom\\Blueprints\\ESK Blueprints");
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlasticOnStock_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new PlasticStorage());
        }

        private void Recycling_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new WasteRecyclingPage());
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Defective_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new DefectiveCoilsPage());

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new HomePage());
        }

        private void SettingWindow_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new SettingPage());
        }

        private void Calculator_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new СalculatorPage());
        }

        private void Delivering_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new StatisticsOnOrdersPage(0)) ;
        }

        private void PlasticDitals_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new PlasticDitalesPage());
        }

        private void DitalsFromProduction_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new DeitalesProductionPage());
        }

        private void Engraving_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new EngravingPage());
        }

        private void EngravingProfils_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new EngravingProfilPage());
        }

        private void RecyclingProjils_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void PrintProfils_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new Print3DProfilsPage());
        }

        private void Printers3D_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new For3DPrintingPage());
        }

        private void ESKBlueprints_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new ESKBlueprintsPage());
        }

        private void KonstruktionsBlueprints_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new ConstructionBlueprintsPage());
        }
    }
}
