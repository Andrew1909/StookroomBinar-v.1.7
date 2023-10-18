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
    /// Логика взаимодействия для PlasticDitalesPage.xaml
    /// </summary>
    public partial class PlasticDitalesPage : Page
    {
        public PlasticDitalesPage()
        {
            InitializeComponent();

            var coutEmpty = Connect.bd.PlasticProducts.Where(p => p.IDInside != 0).Count()+1;
            for(int j = 1; j < coutEmpty; j++)
            {

                var objA = Connect.bd.PlasticProducts.First(p => p.IDInside == j);
                if (objA.CountOnStoock == 0)
                {
                    var objL = Connect.bd.ProductsForEngraving.Where(p => p.IDInside == objA.ID && p.TypeDitalesID == 1).Count();
                    if (objL == 0)
                    {
                        Connect.bd.PlasticProducts.Remove(objA);
                        Connect.bd.SaveChanges();
                    }
                }
            }
            ChangedIDIsnideDeliver();


            PlastitDitelisView.ItemsSource = Connect.bd.PlasticProducts.ToList();
        }

        void ChangedIDIsnideDeliver()
        {
            var c = Connect.bd.PlasticProducts.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.PlasticProducts.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.PlasticProducts.Select(q => q.ID).Max();
                int minID = Connect.bd.PlasticProducts.Select(q => q.ID).Min();


                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.PlasticProducts.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.PlasticProducts.First(p => p.ID == n);
                        NullElement.IDInside = st;
                        Connect.bd.SaveChanges();
                        n = NullElement.ID;
                        n++;
                        st++;
                    }
                    if (t == 0)
                    {
                        n++;
                    }
                }
            }
        }


        private void AddDitalis_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddPlasticDitalesPage());
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PlastitDitelisView.ItemsSource = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID.StartsWith(SearchColor.Text)).ToList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var a = PlastitDitelisView.SelectedItem as PlasticProducts;
            if (a != null)
            {
                MyFrame.Navigate(new EditPlasticProductPage(a));
            }
        }
    }
}
