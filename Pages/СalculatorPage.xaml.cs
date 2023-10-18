using StockroomBinar.BD;
using StockroomBinar.Class;
using StockroomBinar.DialogWindow;
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
    /// Логика взаимодействия для СalculatorPage.xaml
    /// </summary>
    public partial class СalculatorPage : Page
    {
        double Weight=0;
        double WeightSupport=0;
        string DetalesName;
        public СalculatorPage()
        {
            InitializeComponent();
            var a = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            AddNameDitalies.Items.Add("Выберите изделие");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDProductsProduction.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProduct.ToString());
                }
            }
            AddNameDitalies.SelectedIndex = 0;
        }

        private void Сalculate_Click(object sender, RoutedEventArgs e)
        {
            if (AddCountDitalis.Text == null || AddCountDitalis.Text == "") MessageBox.Show("Не все поля заполнены!");
            else
            {
                int index1 = AddNameDitalies.SelectedIndex;
                if (AddNameDitalies.SelectedIndex == index1)
                {
                    if (index1 > 0)
                    {
                        var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index1);
                        DetalesName = a1.NameProduct;
                        var objA = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == DetalesName);
                        Weight = double.Parse(objA.ProductWeight.ToString());
                        if (double.Parse(objA.SupportsWeight.ToString()) != 0) WeightSupport = double.Parse(objA.SupportsWeight.ToString());
                    }
                }

                SummWeght.Text = ((Weight + WeightSupport) * int.Parse(Count.Text)).ToString() + "кг.";
                if (((Weight + WeightSupport) * int.Parse(Count.Text))  < 1) CoilsCount.Text = "> 1 катушки";
                else
                {
                    //if (((int)((Weight + WeightSupport) * int.Parse(Count.Text))/1000) % 1000==1) CoilsCount.Text = (((int)((Weight + WeightSupport) * int.Parse(Count.Text)))).ToString() + " катушки";
                    CoilsCount.Text = ((int)((Weight + WeightSupport) * int.Parse(Count.Text)) + 1).ToString() + " катушки";   
                }
            }
                
        }

        private void AddNameDitalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index1 = AddNameDitalies.SelectedIndex;
            if (AddNameDitalies.SelectedIndex == index1)
            {
                if (index1 > 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index1);
                    var objA = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == a1.NameProduct).Count();
                    if (objA != 0)
                    {
                        var objB = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == a1.NameProduct);
                        WeightOneDitales.Text = objB.ProductWeight.ToString();
                        WeightSupports.Text = objB.SupportsWeight.ToString();
                    }
                    else MessageBox.Show($"Детлаь {a1.NameProduct} отсутствует на складе. Введите данные в ручную.");
                }
            }
        }

        private void EnterData_Click(object sender, RoutedEventArgs e)
        {
            AddDataForCalculatorWindow DataWindow = new AddDataForCalculatorWindow();
            if (DataWindow.ShowDialog() == true)
            {
                AddNameDitalies.SelectedIndex = 0;
                string a = DataWindow.WeightDitales.ToString();
                string[] words = a.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Weight = double.Parse(words[0].ToString());
                WeightSupport = double.Parse(words[1].ToString());
                WeightOneDitales.Text = Weight.ToString();
                WeightSupports.Text = WeightSupport.ToString();
            }
        }
        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm".ToCharArray();
        private void AddCountDitalis_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Count.Text = AddCountDitalis.Text;
        }

        private void AddCountDitalis_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
