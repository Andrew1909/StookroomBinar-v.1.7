using StockroomBinar.BD;
using StockroomBinar.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Timers;
namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для StatisticsOnOrdersPage.xaml
    /// </summary>
    public partial class StatisticsOnOrdersPage : Page
    {

        public PlasticProducts plasticProducts = new PlasticProducts();
        public DitalesProduction ditalesProduction = new DitalesProduction();
        public StatisticOrders statisticOrders = new StatisticOrders();


        public StatisticsOnOrdersPage(int FireID)
        {
           
            InitializeComponent();

            if (FireID == 1)
            {
                Fire.Visibility = Visibility.Visible;
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Tick += Timer_Tick;
                timer.Start();
            }


            int CountDiliverTodey = 0;
            string datenow = DateTime.Now.ToShortDateString();


            var objA2 = Connect.bd.Deliveries.Where(p => p.IDInside!=0).Count()+1;
            for (int i = 1; i < objA2; i++)
            {
                var objN = Connect.bd.Deliveries.Where(p => p.IDInside == i).Count();
                if (objN != 0)
                {
                    var objM = Connect.bd.Deliveries.Where(p => p.IDInside == i).First();
                    string a = (objM.Date.Value.Date.ToShortDateString()).ToString();
                    if (a == datenow) CountDiliverTodey++;
                }
                
            }
            countDiliverTodey.Text = CountDiliverTodey.ToString();



            string s = DateTime.Now.ToString("MM");
            DataToday.Text = DateTime.Now.ToString("dd MMMM");       
            int countDeliver = 0;
            var countDeliverMounth = Connect.bd.Deliveries.Where(p => p.IDInside !=0 ).Count()+1;
            for (int i = 1; i < countDeliverMounth; i++)
            {
                var objN = Connect.bd.Deliveries.Where(p => p.IDInside == i).Count();
                if (objN != 0)
                {
                    var objM = Connect.bd.Deliveries.Where(p => p.IDInside == i).First();
                    string a = objM.Date.Value.Date.Month.ToString();
                   
                    if (a == s) countDeliver++;
                }
            }
            CountDeliver.Text = countDeliver.ToString();

            var countDeliverAll = Connect.bd.Deliveries.Where(p => p.Date != null).Count();
            AllDeliverCount.Text = countDeliverAll.ToString();






            var objA = Connect.bd.PlasticProducts.Where(p => p.ID != 0).Count()+1;
            var objD = Connect.bd.DitalesProduction.Where(p => p.ID != 0).Count() + 1;
            var objC= Connect.bd.StatisticOrders.Where(p => p.NameDitales != null).Count();

            for (int i = 1; i < objA; i++)
            {
                var objB = Connect.bd.PlasticProducts.First(p => p.IDInside == i);
                string Name = objB.ProductTypeID;

                var objK = Connect.bd.StatisticOrders.Where(p => p.NameDitales == Name).Count();
                if (objK != 0)
                {
                    var objN = Connect.bd.StatisticOrders.First(p => p.NameDitales == Name);

                    statisticOrders = objN;
                    statisticOrders.ReadyCount = 0;
                    Connect.bd.SaveChanges();
                }

            }

            for (int i = 1; i < objD; i++)
            {
                var objB = Connect.bd.DitalesProduction.First(p => p.IDInside == i);
                string Name = objB.CodeDitales;

                var objK = Connect.bd.StatisticOrders.Where(p => p.NameDitales == Name).Count();
                if (objK != 0)
                {
                    var objN = Connect.bd.StatisticOrders.First(p => p.NameDitales == Name);

                    statisticOrders = objN;
                    statisticOrders.ReadyCount = 0;
                    Connect.bd.SaveChanges();
                }

            }

            for (int i = 1; i < objA; i++)
            {
                var objB = Connect.bd.PlasticProducts.First(p => p.IDInside == i);
                string Name = objB.ProductTypeID;
        
                    var objK = Connect.bd.StatisticOrders.Where(p => p.NameDitales == Name).Count();
                    if (objK != 0)
                    {
                        var objN = Connect.bd.StatisticOrders.First(p => p.NameDitales == Name);

                        statisticOrders = objN;
                        //if (statisticOrders.ReadyCount < objB.EngravingStatus)
                        //{
                            statisticOrders.ReadyCount = statisticOrders.ReadyCount + int.Parse(objB.EngravingStatus.ToString());
                            Connect.bd.SaveChanges();
                        //}
                    }
               
            }

            for (int i = 1; i < objD; i++)
            {
                var objB = Connect.bd.DitalesProduction.First(p => p.IDInside == i);
                string Name = objB.CodeDitales;

                var objK = Connect.bd.StatisticOrders.Where(p => p.NameDitales == Name).Count();
                if (objK != 0)
                {
                    var objN = Connect.bd.StatisticOrders.First(p => p.NameDitales == Name);

                    statisticOrders = objN;
                    statisticOrders.ReadyCount = statisticOrders.ReadyCount + int.Parse(objB.EngravingStatus.ToString());
                    Connect.bd.SaveChanges();
                }

            }

            var objV = Connect.bd.StatisticOrders.Where(p => p.NeseseryCount == 0).Count();
            if (objV != 0)
            {
                for(int h = 0; h < objV; h++)
                {
                    var objX = Connect.bd.StatisticOrders.Where(p => p.NeseseryCount == 0).First();
                    Connect.bd.StatisticOrders.Remove(objX);
                    Connect.bd.SaveChanges();
                }
               
            }

            StatisticOrdersView.ItemsSource = Connect.bd.StatisticOrders.ToList();


            var objA1 = Connect.bd.Deliveries.Where(p => p.ID != 0).Count();
            if (objA1 == 0)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = "Данные о поставках отсутствуют.";
                DeliversView.Items.Add(lv);
            }
            else DeliversView.ItemsSource = Connect.bd.Deliveries.ToList();

        }



        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LockInfoNatif_Click(object sender, RoutedEventArgs e)
        {
            var a = DeliversView.SelectedItem as Deliveries;
            if (a != null)
            {
                MyFrame.Navigate(new DeliveresInfoPage(a));
            }
        }

        private void AddDeliver_Click(object sender, RoutedEventArgs e)
        {
          
            MyFrame.Navigate(new AddDeliveriesPage());


        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            Fire.Visibility = Visibility.Hidden;

        }


        private void SearchCustomerName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var objA=Connect.bd.Deliveries.Count();
            if(objA!=0) DeliversView.ItemsSource = Connect.bd.Deliveries.Where(p => p.СustomerТame.StartsWith(SearchCustomerName.Text)).ToList();
        }
    }
}
