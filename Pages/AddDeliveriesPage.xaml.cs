using LiveCharts.Wpf;
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
using static System.Net.Mime.MediaTypeNames;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddDeliveriesPage.xaml
    /// </summary>
    public partial class AddDeliveriesPage : Page
    {

        string[,] DetalesNeme = new string[99, 2];
        public DeliveriesProducts deliveriesProducts = new DeliveriesProducts();
        public Deliveries deliveries = new Deliveries();
        public StatisticOrders statisticOrders = new StatisticOrders();


        string NameDitaliesID = "";
        int CountPlastProduct = 0;
        int CountProductProduct = 0;
        int sumProduct;

        public AddDeliveriesPage()
        {
            InitializeComponent();
            var a = Connect.bd.IDPlasticProducts.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            CountPlastProduct = a;
            AddNameDitalies.Items.Add("Выберите изделие");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDPlasticProducts.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDPlasticProducts.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProduct.ToString());
                }
            }
            a = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();  //заносим данные ID деталей из пластика из БД в combobox 
            CountProductProduct = a;
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.IDProductsProduction.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.IDProductsProduction.First(p => p.IDInside == j);
                    AddNameDitalies.Items.Add(a1.NameProducts.ToString());
                }
            }
            AddNameDitalies.SelectedIndex = 0;


            string date = DateTime.Now.ToString("yyyy");
            AddDate.Text = $"__.__.{date}";
        }

        private void AddDelivereies_Click(object sender, RoutedEventArgs e)
        {
            


            string date = DateTime.Now.ToString("yyyy");
            if (AddCustomer == null || AddDate.Text == null || DetalesNeme[0, 0] == null || AddDate.Text== $"__.__.{date}" || AddDate.Text=="") MessageBox.Show("Не все поля заполнены!");
            else
            {

                int IDCorrectDate = 0;
                string s1 = AddDate.Text;
                int indexOfChar = s1.IndexOf(".");
                if (indexOfChar != 2) MessageBox.Show("Введен неверный формат даты!");

                else
                {
                    string[] words = s1.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] days = { "01","02","03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
                    string[] month = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"};

                    for (int j = 0; j < 31; j++)
                    {
                        if (words[0].ToString() == days[j])
                        {
                            IDCorrectDate++;
                        }

                    }

                    for (int j = 0; j < 12; j++)
                    {
                        if (words[1] == month[j])
                        {
                            IDCorrectDate++;
                        }

                    }
                    
                }
                if (IDCorrectDate != 2) MessageBox.Show("Введен неверный формат даты!");
                else
                {
                    deliveries.СustomerТame = AddCustomer.Text;
                    deliveries.Date = DateTime.ParseExact(AddDate.Text, "dd.MM.yyyy", null);
                    deliveries.Status = 0;
                    if (Connect.bd.Deliveries.Select(q => q.IDInside).Max() == null) deliveries.IDInside = 1;
                    else deliveries.IDInside = Connect.bd.Deliveries.Select(q => q.IDInside).Max() + 1;
                    Connect.bd.Deliveries.Add(deliveries);
                    Connect.bd.SaveChanges();
                    int position = 1;
                    int maxID = (Connect.bd.Deliveries.Select(q => q.ID).Max());
                    for (int c = 0; c < 99; c++)
                    {
                        if (DetalesNeme[c, 0] != null)
                        {
                            deliveriesProducts.IDInside = maxID;
                            deliveriesProducts.CodeDitals = DetalesNeme[c, 0];
                            deliveriesProducts.ReadyDitals = 0;
                            deliveriesProducts.NecessaryCountDitals= int.Parse(DetalesNeme[c, 1]);
                            deliveriesProducts.Status = 0;
                            deliveriesProducts.NumberPosition = position;
                            Connect.bd.DeliveriesProducts.Add(deliveriesProducts);
                            Connect.bd.SaveChanges();
                            string NameDitales= DetalesNeme[c, 0];  
                            var objL = Connect.bd.StatisticOrders.Where(p=>p.NameDitales== NameDitales).Count();
                            if (objL != 0)
                            {
                                var objK = Connect.bd.StatisticOrders.First(p => p.NameDitales == NameDitales);
                                statisticOrders = objK;
                                statisticOrders.NeseseryCount = statisticOrders.NeseseryCount + int.Parse(DetalesNeme[c, 1]);
                                Connect.bd.SaveChanges();
                            }
                            else
                            {
                                statisticOrders.NameDitales= DetalesNeme[c, 0];
                                statisticOrders.NeseseryCount = int.Parse(DetalesNeme[c, 1]);
                                statisticOrders.ReadyCount = 0;
                                Connect.bd.StatisticOrders.Add(statisticOrders);
                                Connect.bd.SaveChanges();
                            }
                            position++;
                        }
                    }
                    MessageBox.Show("Поставка добавлена!");
                    var objA = Connect.bd.Deliveries.First(p => p.ID == maxID);
                    MyFrame.Navigate(new DeliveresInfoPage(objA));
                }

                
            }
        }
        public class AddtData
        {
            public string CodeDitals { get; set; }
            public string NeseseryDitales { get; set; }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (AddNececeryCount != null && AddNececeryCount.Text != "")
            {
                for (int j = 0; j < 99; j++)
                {
                    if (DetalesNeme[j, 0] == null)
                    {
                        DetalesNeme[j, 0] = NameDitaliesID;
                        DetalesNeme[j, 1] = AddNececeryCount.Text;
                        DeliversProductView.Items.Add(new AddtData { CodeDitals = DetalesNeme[j, 0], NeseseryDitales = DetalesNeme[j, 1] });
                        break;
                    }
                }
                AddNececeryCount.Text = string.Empty;
                AddNameDitalies.SelectedIndex = 0;
            }

            

        }

        private void AddCodeDitales_SelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }



        private void AddNameDitalies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sumProduct = CountProductProduct + CountPlastProduct+1;
            string[,] DetalesProduction = new string[sumProduct, 2];
            int countet = 1;
            int index2;
            var objA = Connect.bd.IDPlasticProducts.Where(p => p.ID != 0).Count();
            var objB = Connect.bd.IDProductsProduction.Where(p => p.ID != 0).Count();


            if (objA != 0 && objB != 0)
            { 
                for(int j=0;j<=objA; j++)
                {
                    if (DetalesProduction[j, 0] == null)
                    {
                        DetalesProduction[j, 0] = j.ToString();
                        DetalesProduction[j, 1] = countet.ToString();
                        countet++;
                    }
                }
                countet = 1;

                objA++;
                for (int j = objA; j < sumProduct; j++)
                {
                    if (DetalesProduction[j, 0] == null)
                    {
                        DetalesProduction[j, 0] = j.ToString();
                        DetalesProduction[j, 1] = countet.ToString();
                        countet++;

                    }
                }
                int index = AddNameDitalies.SelectedIndex;
                if (AddNameDitalies.SelectedIndex == index)
                {
                    if (index > 0)
                    {
                        
                        if (index <= CountPlastProduct)//данные с этими индексами взяты из таблицы с платиковыми деталями
                        {
                            var objC = Connect.bd.IDPlasticProducts.First(p => p.IDInside == index);
                            NameDitaliesID = objC.NameProduct;
                        }
                        if (index > CountPlastProduct)// данные с этими индексами взяты из таблицы с деталями с производства
                        {
                            

                            for (int j = 0; j < sumProduct; j++)
                            {
                                if (DetalesProduction[j, 0] == index.ToString())
                                {

                                    index2 = int.Parse(DetalesProduction[j, 1].ToString());
                                    var objC = Connect.bd.IDProductsProduction.First(p => p.IDInside == index2);
                                    NameDitaliesID = objC.NameProducts;

                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val) && e.Text != ".")
            {
                e.Handled = true; // отклоняем ввод
            }
        }

        private void AddNececeryCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }
    }
}
