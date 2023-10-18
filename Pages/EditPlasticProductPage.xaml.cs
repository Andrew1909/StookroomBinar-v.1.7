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
    /// Логика взаимодействия для EditPlasticProductPage.xaml
    /// </summary>
    public partial class EditPlasticProductPage : Page
    {
        public PlasticProducts plasticProducts = new PlasticProducts();
        public IDPlasticProducts iDPlasticProduct = new IDPlasticProducts();
        public ProductsForEngraving forEngraving = new ProductsForEngraving();

        int maxIndex;
        string NameDitaliesID = "";
        string ColorNamePlast = "";
        string Manufactr = "";
        string TypePlast = "";
        decimal SupportsWight1 = 0;
        int IDPlast;
        public EditPlasticProductPage(PlasticProducts item)
        {
            InitializeComponent();
            var a = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count();
            maxIndex = a+1;
            AddColordNamePlastic.Items.Add("Выберите цвет платика");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.PlasticStor.Where(p => p.IDInsaid == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.PlasticStor.First(p => p.IDInsaid == j);
                    AddColordNamePlastic.Items.Add(a1.ColorName.ToString() + " Тип: " + a1.PlasticType.ToString() + " Производитель: " + a1.Manufacturer.ToString());
                }
            }

            var objA = Connect.bd.PlasticStor.Where(p => p.ColorName == item.ColorName && p.PlasticType == item.TypePlasticPrint && p.Manufacturer == item.ManufacturerPlasticPrint).Count();
            if (objA != 0)
            {
                var objB = Connect.bd.PlasticStor.First(p => p.ColorName == item.ColorName&&p.PlasticType==item.TypePlasticPrint&&p.Manufacturer==item.ManufacturerPlasticPrint);
                AddColordNamePlastic.SelectedIndex = int.Parse(objB.IDInsaid.ToString());

            }
            else
            {
                AddColordNamePlastic.Items.Add(item.ColorName.ToString() + " Тип: " + item.TypePlasticPrint.ToString() + " Производитель: " + item.ManufacturerPlasticPrint.ToString()+" (закончился)");
            }
            AddNameDitalies.Text= item.ProductTypeID;
            AddColordNamePlastic.SelectedIndex = int.Parse((a + 1).ToString());
            AddWidthDitales.Text = item.ProductWeight.ToString();
            AddTimeDitalis.Text = item.TimePrint.ToString();
            if (item.SupportsWeight != 0)
            {
                SupportsWight.Text = item.SupportsWeight.ToString();
                Plus.Visibility = Visibility.Visible;
                NextTextSupports.Visibility = Visibility.Visible;
            }
            else
            {
                Plus.Visibility = Visibility.Hidden;
                NextTextSupports.Visibility = Visibility.Hidden;
                plasticProducts = item;
            }

            
        }

        private void Suports_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Suports_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int index1 = AddColordNamePlastic.SelectedIndex;
            if (AddColordNamePlastic.SelectedIndex == index1)
            {
                if (index1 > 0&& index1!= maxIndex)
                {
                    var a1 = Connect.bd.PlasticStor.First(p => p.IDInsaid == index1);
                    plasticProducts.ColorName = a1.ColorName;
                    plasticProducts.ManufacturerPlasticPrint = a1.Manufacturer;
                    plasticProducts.TypePlasticPrint = a1.PlasticType;
                }
            }
            plasticProducts.ProductWeight = decimal.Parse(AddWidthDitales.Text);
            plasticProducts.TimePrint = AddTimeDitalis.Text;

            var objA = Connect.bd.IDPlasticProducts.Where(p => p.NameProduct == AddNameDitalies.Text).Count();
            if(objA != 0) 
            {
                plasticProducts.ProductTypeID = AddNameDitalies.Text;
                Connect.bd.SaveChanges();
                MessageBox.Show("Деталь добавлена!");
                MyFrame.Navigate(new PlasticDitalesPage());
            }
            else
            {
                string str;
                var objB=Connect.bd.IDPlasticProducts.First(p => p.NameProduct == plasticProducts.ProductTypeID);
                str = objB.NameProduct;
                iDPlasticProduct = objB;
                iDPlasticProduct.NameProduct = AddNameDitalies.Text;
                Connect.bd.SaveChanges();
                var objC=Connect.bd.PlasticProducts.Where(p => p.ProductTypeID== str).Count();
                if(objC != 0)
                {
                    for(int j = 0; j < objC; j++)
                    {
                        var objD = Connect.bd.PlasticProducts.First(p => p.ProductTypeID ==str);
                        plasticProducts=objD;
                        plasticProducts.ProductTypeID = AddNameDitalies.Text;
                        Connect.bd.SaveChanges();
                    }
                }

                objC = Connect.bd.ProductsForEngraving.Where(p => p.ProductTypeID == str).Count();
                if (objC != 0)
                {
                    for (int j = 0; j < objC; j++)
                    {
                        var objD = Connect.bd.ProductsForEngraving.First(p => p.ProductTypeID == str);
                        forEngraving = objD;
                        forEngraving.ProductTypeID = AddNameDitalies.Text;
                        Connect.bd.SaveChanges();
                    }
                }
                MessageBox.Show("Деталь добавлена!");
                MyFrame.Navigate(new PlasticDitalesPage());

            }
        }
    }
}
