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
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using StockroomBinar.BD;
using StockroomBinar.Class;
using StockroomBinar.DialogWindow;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public Notifications notifications = new Notifications();
        public NotesUser notesUser = new NotesUser();
        public HomePage()
        {
            InitializeComponent();
            startclock();

            var objV = Connect.bd.NotesUser.Where(p => p.Status == true).Count();
            if (objV != 0)
            {
                for (int h = 0; h < objV; h++)
                {
                    var objX = Connect.bd.NotesUser.Where(p => p.Status == true).First();
                    Connect.bd.NotesUser.Remove(objX);
                    Connect.bd.SaveChanges();
                }

            }



            var objA = Connect.bd.Deliveries.Where(p => p.ID != 0).Count(); //проверяем данные о поставках для отображения
            if(objA==0)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = "Данные о поставках отсутствуют";
                DeliversView.Items.Add(lv);
            }
            else DeliversView.ItemsSource = Connect.bd.Deliveries.OrderBy(p=>p.Date).ToList();

            NotoficationDeliver();

            objA = Connect.bd.Notifications.Where(p => p.ID != 0).Count(); //проверяем данные о уведомлениях для отображения
            if (objA == 0)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = "Уведомлений нет";
                NotificationsView.Items.Add(lv);
            }
            else NotificationsView.ItemsSource = Connect.bd.Notifications.ToList();

            var objE = Connect.bd.NotesUser.Where(p => p.ID != 0).Count();
            if (objE == 0)
            {
                ListViewItem lv = new ListViewItem();
                lv.Content = "Напоминаний нет";
                NotessView.Items.Add(lv);
            }
            else
            {
                NotessView.ItemsSource = Connect.bd.NotesUser.ToList();
            }
            //CountPlastOnStock.Text = (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()).ToString();

            //if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() == 1|| Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() == 21)
            //{
            //    ColorCount.Text = "цвет";
            //}
            //if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()> 1&& Connect.bd.PlasticStor.Where(p => p.ID != 0).Count()<5|| Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() > 21 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <=24)
            //{
            //    ColorCount.Text = "цвета";
            //}
            //if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >=5 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <=20)
            //{
            //    ColorCount.Text = "цветов";
            //}
            //if (Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >= 21 && Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() <= 20)
            //{
            //    ColorCount.Text = "цветов";
            //}
            //if(Connect.bd.PlasticStor.Where(p => p.ID != 0).Count() >= 25)
            //{
            //    ColorCount.Text = "цвета";
            //}

            //CountDitalsOnStock.Text = (Connect.bd.DitalesProduction.Where(p => p.ID != 0).Count()+ Connect.bd.PlasticProducts.Where(p => p.ID != 0).Count()).ToString();

            pieChart();
            DiagramPlast();
            DioagramDitals();
            
        }

        private void startclock()
        {
            DispatcherTimer timer= new DispatcherTimer();
            timer.Interval=TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            Date.Text = DateTime.Now.ToString();
            //Clock.Text= DateTime.Now.ToString(@"HH:mm:ss");
        }

        void NotoficationDeliver()
        {
            DateTime a = DateTime.Now;
            var objA = Connect.bd.Deliveries.Where(p => p.Date < a).Count();
            if (objA != 0)
            {
                for(int j = 0; j < objA; j++)
                {

                    var objB = Connect.bd.Deliveries.First(p => p.Date < a);
                    string DescriptionNatification = $"Поставка для {objB.СustomerТame} просрочена";
                    var objC = Connect.bd.Notifications.Where(p => p.Descriptiont == DescriptionNatification).Count();
                    if (objC == 0)
                    {
                        notifications.Descriptiont = $"Поставка для {objB.СustomerТame} просрочена";
                        Connect.bd.Notifications.Add(notifications);
                        Connect.bd.SaveChanges();
                    }
                    
                }
            }
        }


        public void DiagramPlast()
        {
            var SumCount=Connect.bd.PlasticStor.Where(p=>p.ID != 0).Count();
            if (SumCount != 0)
            {
                SumCount--;
                int[] arr = new int[SumCount];
                for (int j = 0; j < SumCount; j++)
                {
                    var objA = Connect.bd.PlasticStor.Where(p => p.IDInsaid == j).Count();

                    if (objA != 0)
                    {
                        var objB = Connect.bd.PlasticStor.First(p => p.IDInsaid == j);
                        arr[j] = int.Parse(objB.NumberСoils.ToString());
                    }
                }

                int temp;
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    for (int j = i + 1; j < arr.Length; j++)
                    {
                        if (arr[i] > arr[j])
                        {
                            temp = arr[i];
                            arr[i] = arr[j];
                            arr[j] = temp;
                        }
                    }
                }

                if (SumCount >3)
                {
                    int max = arr[SumCount-1];
                    int max1 = arr[SumCount-2];
                    int max2 = arr[SumCount-3];
                    var objMax1 = Connect.bd.PlasticStor.First(p=>p.NumberСoils==max);
                    var objMax2 = Connect.bd.PlasticStor.First(p => p.NumberСoils == max1);
                    var objMax3 = Connect.bd.PlasticStor.First(p => p.NumberСoils == max2);
                    string MaxName1 = objMax1.ColorName;
                    string MaxName2 = objMax2.ColorName;
                    string MaxName3 = objMax3.ColorName;
                    seriesCollection = new SeriesCollection
                    {

                        new PieSeries
                        {
                    
                            Title=MaxName1,
                            Values=new ChartValues<ObservableValue> {new ObservableValue(max)},
                            DataLabels=true
                         },
                        new PieSeries
                        {
                            Title=MaxName2,
                            Values=new ChartValues<ObservableValue> {new ObservableValue(max1)},
                            DataLabels=true
                        },
                        new PieSeries
                        {
                            Title=MaxName3,
                            Values=new ChartValues<ObservableValue> {new ObservableValue(max2)},
                            DataLabels=true
                        },
                        new PieSeries
                        {
                            Title="Другие...",
                             Values=new ChartValues<ObservableValue> {new ObservableValue(2)},
                            DataLabels=true
                        },
                    };
                    DataContext = this;
                }
                
            }
           
        }

        public void DioagramDitals()
        {

            var SumCount = Connect.bd.PlasticProducts.Where(p => p.ID != 0).Count();
            if (SumCount != 0)
            {
                SumCount--;
                int[] arr = new int[SumCount];
                for (int j = 0; j < SumCount; j++)
                {
                    var objA = Connect.bd.PlasticProducts.Where(p => p.IDInside == j).Count();

                    if (objA != 0)
                    {
                        var objB = Connect.bd.PlasticProducts.First(p => p.IDInside == j);
                        arr[j] = int.Parse(objB.CountOnStoock.ToString());
                    }
                }

                int temp;
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    for (int j = i + 1; j < arr.Length; j++)
                    {
                        if (arr[i] > arr[j])
                        {
                            temp = arr[i];
                            arr[i] = arr[j];
                            arr[j] = temp;
                        }
                    }
                }

                if (SumCount > 3)
                {

                    int max = arr[SumCount - 1];
                    int max1 = arr[SumCount - 2];
                    int max2 = arr[SumCount - 3];

                    var objMax1 = Connect.bd.PlasticProducts.First(p => p.CountOnStoock == max);
                    var objMax2 = Connect.bd.PlasticProducts.First(p => p.CountOnStoock == max1);
                    var objMax3 = Connect.bd.PlasticProducts.First(p => p.CountOnStoock == max2);
                    string MaxName1 = objMax1.ProductTypeID;
                    string MaxName2 = objMax2.ProductTypeID;
                    string MaxName3 = objMax3.ProductTypeID;



                    seriesCollection2 = new SeriesCollection
                    {

                         new PieSeries
                         {
                             Title=MaxName1,
                             Values=new ChartValues<ObservableValue> {new ObservableValue(max) },
                             DataLabels=true
                        },
                        new PieSeries
                        {
                             Title=MaxName2,
                             Values=new ChartValues<ObservableValue> {new ObservableValue(max1) },
                                DataLabels=true
                        },
                         new PieSeries
                        {
                            Title=MaxName3,
                            Values=new ChartValues<ObservableValue> {new ObservableValue(max2) },
                            DataLabels=true
                        },
                        new PieSeries
                        {
                            Title="Другие",
                            Values=new ChartValues<ObservableValue> {new ObservableValue(2) },
                            DataLabels=true
                        },
                        };
                    DataContext = this;

                }
            }
        }

        public void pieChart()
        {
            Pointlabel = ChartPoint => string.Format("{0}({1:P)", ChartPoint.Y, ChartPoint.Participation);
            DataContext = this;
        }

        public Func<ChartPoint, string> Pointlabel { get; set; }
        public SeriesCollection seriesCollection { get; set; }
        public SeriesCollection seriesCollection2 { get; set; }
        private void info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchInfoNat_Click(object sender, RoutedEventArgs e)
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

        private void DeleteNotifications_Click(object sender, RoutedEventArgs e)
        {
            var a = NotificationsView.SelectedItem as Notifications;
            if (a != null)
            {
                Connect.bd.Notifications.Remove(a);
                Connect.bd.SaveChanges();
                NotificationsView.ItemsSource = Connect.bd.Notifications.ToList();
            }
           //var objA = Connect.bd.Notifications.Where(p => p.ID != 0).Count(); //проверяем данные о уведомлениях для отображения
           // if (objA == 0)
           // {
           //     ListViewItem lv = new ListViewItem();
           //     lv.Content = "Уведомлений нет";
           //     NotificationsView.Items.Add(lv);
           // }
           // else NotificationsView.ItemsSource = Connect.bd.Notifications.ToList();
        }

        private void DoneNotes_Checked(object sender, RoutedEventArgs e)
        {
            var a = NotessView.SelectedItem as NotesUser;
            if (a != null)
            {
                notesUser = a;
                notesUser.Status = true;
                Connect.bd.SaveChanges();
                NotessView.ItemsSource = Connect.bd.NotesUser.ToList();
            }
        }

        private void DoneNotes_Unchecked(object sender, RoutedEventArgs e)
        {
            var a = NotessView.SelectedItem as NotesUser;
            if (a != null)
            {
                notesUser = a;
                notesUser.Status = false;
                Connect.bd.SaveChanges();
               
            }
            NotessView.ItemsSource = Connect.bd.NotesUser.ToList();
        }

        private void AddNotes_Click(object sender, RoutedEventArgs e)
        {
            AddNotesWindow TypeWindow = new AddNotesWindow();
            if (TypeWindow.ShowDialog() == true)
            {
                notesUser.Descriptions = TypeWindow.NewNotes.Text;
                notesUser.Status = false;
                Connect.bd.NotesUser.Add(notesUser);
                Connect.bd.SaveChanges();
            }
            MyFrame.Navigate(new HomePage());
        }
    }
}
