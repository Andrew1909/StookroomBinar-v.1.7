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
    /// Логика взаимодействия для EditManufacturerInfoPage.xaml
    /// </summary>
    public partial class EditManufacturerInfoPage : Page
    {
        public IDManufacturer idManufacturer = new IDManufacturer();
        int NDSYesNo = 0;
        public EditManufacturerInfoPage(IDManufacturer item)
        {
            InitializeComponent();
             AddNmeManufact.Text=item.NameManufacturer;
            if (item.NDS == "Да") NDSManufact.IsChecked= true;
            if (item.NDS=="Нет") NDSManufact.IsChecked = false;
            CountryManufact.Text= item.Country;
            if (item.Quality == "Отличное") Quality.SelectedIndex=1;
            if (item.Quality == "Хорошее") Quality.SelectedIndex = 2;
            if (item.Quality == "Нормальное") Quality.SelectedIndex = 3;
            if (item.Quality == "Плохое") Quality.SelectedIndex = 4;
            if (item.SpeedDeliver == "Быстро")SpeedDeliver.SelectedIndex = 1;
            if (item.SpeedDeliver == "Нормально") SpeedDeliver.SelectedIndex = 2;
            if (item.SpeedDeliver == "Долго") SpeedDeliver.SelectedIndex = 3;
            Site.Text= item.Site;
            Notes.Text=item.Notes;
            idManufacturer = item;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (AddNmeManufact.Text == null || AddNmeManufact.Text == "" || CountryManufact.Text == null || CountryManufact.Text == "" || Quality.SelectedIndex == 0 || SpeedDeliver.SelectedIndex == 0)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                    int maxID = int.Parse((Connect.bd.IDManufacturer.Select(q => q.IDInside).Max()).ToString());
                    idManufacturer.NameManufacturer = AddNmeManufact.Text;
                    if (NDSYesNo == 0) idManufacturer.NDS = "Нет";
                    if (NDSYesNo == 1) idManufacturer.NDS = "Да";
                    idManufacturer.Country = CountryManufact.Text;
                    if (Quality.SelectedIndex == 1) idManufacturer.Quality = "Отличное";
                    if (Quality.SelectedIndex == 2) idManufacturer.Quality = "Хорошее";
                    if (Quality.SelectedIndex == 3) idManufacturer.Quality = "Нормальное";
                    if (Quality.SelectedIndex == 4) idManufacturer.Quality = "Плохое";
                    if (SpeedDeliver.SelectedIndex == 1) idManufacturer.SpeedDeliver = "Быстро";
                    if (SpeedDeliver.SelectedIndex == 2) idManufacturer.SpeedDeliver = "Нормально";
                    if (SpeedDeliver.SelectedIndex == 3) idManufacturer.SpeedDeliver = "Долго";
                    if (Site.Text != null || Site.Text != "") idManufacturer.Site = Site.Text;
                    idManufacturer.Notes = Notes.Text;
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Изменения сохранены!");
                    MyFrame.Navigate(new ManufacturerInfoPage());
            }
        }

        private void NDSManufact_Checked(object sender, RoutedEventArgs e)
        {
            NDSYesNo = 1;
        }

        private void NDSManufact_Unchecked(object sender, RoutedEventArgs e)
        {
            NDSYesNo = 0;
        }
    }
}
