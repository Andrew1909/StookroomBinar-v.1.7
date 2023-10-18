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
    /// Логика взаимодействия для AddNewManufacturerPage.xaml
    /// </summary>
    public partial class AddNewManufacturerPage : Page
    {
        public IDManufacturer idManufacturer = new IDManufacturer();

        int NDSYesNo=0;
        public AddNewManufacturerPage()
        {
            InitializeComponent();
            Quality.SelectedIndex = 0;
            SpeedDeliver.SelectedIndex = 0;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void NDSManufact_Checked(object sender, RoutedEventArgs e)
        {
            NDSYesNo = 1;
        }

        private void NDSManufact_Unchecked(object sender, RoutedEventArgs e)
        {
            NDSYesNo = 0;
        }

        private void AddManufact_Click(object sender, RoutedEventArgs e)
        {
            if(AddNmeManufact.Text==null|| AddNmeManufact.Text == "" || CountryManufact.Text == null || CountryManufact.Text == "" || Quality.SelectedIndex==0 || SpeedDeliver.SelectedIndex == 0)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                var objA = Connect.bd.IDManufacturer.Where(p => p.NameManufacturer == AddNmeManufact.Text).Count();
                if (objA != 0) MessageBox.Show("Такой производитель уже есть!");
                else
                {
                    int maxID = int.Parse((Connect.bd.IDManufacturer.Select(q => q.IDInside).Max()).ToString());
                    idManufacturer.NameManufacturer = AddNmeManufact.Text;
                    idManufacturer.IDInside = maxID + 1;
                    if (NDSYesNo == 0) idManufacturer.NDS = "Нет";
                    if (NDSYesNo == 1) idManufacturer.NDS = "Да";
                    idManufacturer.Country = CountryManufact.Text;
                    if (Quality.SelectedIndex == 1) idManufacturer.Quality = "Отличное";
                    if (Quality.SelectedIndex == 2) idManufacturer.Quality = "Хорошее";
                    if (Quality.SelectedIndex == 3) idManufacturer.Quality = "Нормальное";
                    if (Quality.SelectedIndex == 4) idManufacturer.Quality = "Плохое";
                    if (SpeedDeliver.SelectedIndex == 1) idManufacturer.SpeedDeliver = "Быстро";
                    if (SpeedDeliver.SelectedIndex == 1) idManufacturer.SpeedDeliver = "Нормально";
                    if (SpeedDeliver.SelectedIndex == 1) idManufacturer.SpeedDeliver = "Долго";
                    if (Site.Text != null || Site.Text != "") idManufacturer.Site = Site.Text;
                    idManufacturer.Notes = Notes.Text;
                    Connect.bd.IDManufacturer.Add(idManufacturer);
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Производитель добавлен!");
                    MyFrame.Navigate(new ManufacturerInfoPage());
                }
            }
        }
    }
}
