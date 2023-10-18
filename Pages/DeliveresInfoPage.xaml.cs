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
    /// Логика взаимодействия для DeliveresInfoPage.xaml
    /// </summary>
    public partial class DeliveresInfoPage : Page
    {
        public DeliveriesProducts deliveriesProducts = new DeliveriesProducts();
        public Deliveries deliveries = new Deliveries();
        public PlasticProducts plasticProducts = new PlasticProducts();
        public DitalesProduction ditalesProduction = new DitalesProduction();
        public StatisticOrders statisticOrders = new StatisticOrders();

        int DeliverID = 0;

        public DeliveresInfoPage(Deliveries item)
        {
            InitializeComponent();
            CostomerText.Text = item.СustomerТame;
            DataText.Text = item.Date.Value.Date.ToShortDateString();
            ProcentText.Text = item.Status.ToString();
            DeliverID = item.ID;
            //заполняем заказ из бд

            //var CountPosition = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).Count();
            //int SumReadyDitales = 0;
            //int SumNeseseryDitales = 0;
            //int position = 0; //индикатор, что нужная деталь закончилась
            //int IDPosition = 0;
            //int CountSum = 0;
            //CountPosition++;
            //for (int j=1; j<CountPosition; j++)
            //{
            //    var objA = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID && p.NumberPosition == j).Count();//проверям таблицу на пустоту
            //    if (objA != 0)
            //    {
            //        var objB = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID && p.NumberPosition == j);
            //        var objC = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0).Count();                                                                      //проверяем, в какой таблице находится деатль(платик или с произдовдства)
            //        var objD = Connect.bd.DitalesProduction.Where(p => p.CodeDitales == objB.CodeDitals && p.EngravingStatus > 0).Count();
            //        if (objC != 0)
            //        {
            //            position = objC-1;                                                                                                                                                                                //из таблици с пластиковыми изделяим
            //            var objE = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == objB.CodeDitals &&p.EngravingStatus>0);
            //            deliveriesProducts = objB;
            //            plasticProducts = objE;
            //            if (objE.EngravingStatus!=0)
            //            {
            //                for (int n = 0; n < deliveriesProducts.NecessaryCountDitals; n++) // берем число необходимого кол-ва для конкретной детали
            //                {
            //                    if(deliveriesProducts.ReadyDitals == deliveriesProducts.NecessaryCountDitals)//если позиция выполенена, пропускаем все
            //                    {
            //                        break;
            //                    }
            //                    else //если нет, продолжаем
            //                    {
            //                        //MessageBox.Show(position.ToString());
            //                        for(int a = 0; a < objC; a++) //сколько позиций имеют название нужной детлами (игнорируя тип, название ... пластика)
            //                        {
            //                            var objR = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0).Count();//проверяем, остались ли ещё после каждой  интерации for
            //                            if (objR != 0)
            //                            {

            //                                objE = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0);
            //                                plasticProducts = objE;
            //                                deliveriesProducts.ReadyDitals = deliveriesProducts.ReadyDitals + 1;
            //                                deliveriesProducts.DescriptionOnStok = deliveriesProducts.DescriptionOnStok + " " + objE.ID;
            //                                CountSum = int.Parse(deliveriesProducts.ReadyDitals.ToString());
            //                                Connect.bd.SaveChanges();
            //                                plasticProducts.EngravingStatus = plasticProducts.EngravingStatus - 1;
            //                                plasticProducts.CountOnStoock = plasticProducts.CountOnStoock - 1;
            //                                IDPosition = plasticProducts.ID;
            //                                Connect.bd.SaveChanges();
            //                            }
            //                            //MessageBox.Show(objR.ToString());
            //                            //if (objR == position) //если прошлая деталь закончилась, записывем откуда все взяли
            //                            //{
            //                            //    deliveriesProducts.DescriptionOnStok = deliveriesProducts.DescriptionOnStok + IDPosition.ToString() + " " + CountSum.ToString() + " ";

            //                            //    Connect.bd.SaveChanges();
            //                            //    IDPosition = 0;
            //                            //    CountSum = 0;
            //                            //    position--;
            //                            //    MessageBox.Show("asdasd");
            //                            //}
            //                        }
            //                    } 
            //                }  
            //            }
            //        }

            //        if (objD != 0)
            //        {
            //                                                                                                                                                                                                //из таблици с изделиями с производства
            //            var objE = Connect.bd.DitalesProduction.First(p => p.CodeDitales == objB.CodeDitals && p.EngravingStatus > 0);
            //            deliveriesProducts = objB;
            //            ditalesProduction = objE;
            //            if (objE.EngravingStatus != 0)
            //            {
            //                for (int n = 0; n < deliveriesProducts.NecessaryCountDitals; n++)
            //                {
            //                    if (objE.EngravingStatus != 0)
            //                    {
            //                        if (deliveriesProducts.ReadyDitals == deliveriesProducts.NecessaryCountDitals)
            //                        {
            //                            break;
            //                        }
            //                        else
            //                        {
            //                            deliveriesProducts.ReadyDitals = deliveriesProducts.ReadyDitals + 1;
            //                            Connect.bd.SaveChanges();
            //                            ditalesProduction.EngravingStatus = ditalesProduction.EngravingStatus - 1;
            //                            ditalesProduction.CountOnStoock = ditalesProduction.CountOnStoock - 1;
            //                            Connect.bd.SaveChanges();
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            deliveries = item;
            int  SumNeseseryDitales = 0;
            int SumReadyDitales = 0;
            var objK = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).Count();                                                                                                              //количество позиций в поставке
            var objL = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID);
            int ID = objL.ID;

            // считаем провент готовности
            for (int j = 0; j < objK; j++)
            {
                var objE = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID && p.ID == ID);
                deliveriesProducts = objE;
                SumNeseseryDitales = SumNeseseryDitales + int.Parse(deliveriesProducts.NecessaryCountDitals.ToString());
                SumReadyDitales = SumReadyDitales + int.Parse(deliveriesProducts.ReadyDitals.ToString());
                ID++;
            }
            if (SumNeseseryDitales > 0) deliveries.Status = (SumReadyDitales * 100) / SumNeseseryDitales;
            else deliveries.Status = 0;
            Connect.bd.SaveChanges();
            ProcentText.Text = item.Status.ToString();
            DeliversInfoView.ItemsSource = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).ToList();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseDeliver_Click(object sender, RoutedEventArgs e)                                                                                                                                     //закрыть поставку
        {
            //String s = "Иванов Иван Иванович";
            //String[] words = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //for(int j1 = 0; j1 < 3; j1++)
            //{
            //    MessageBox.Show(words[j1]);
            //}

            if (deliveries.Status == 100)                                                                                                                                                                     //если выполнена полнрстью
            {
                if (MessageBox.Show($"Вы действительно хотите закрыть поставку {deliveries.СustomerТame} ?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var objA = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == deliveries.ID).Count();
                    for (int j = 0; j < objA; j++)
                    {
                        
                        var objB = Connect.bd.DeliveriesProducts.First(p => p.IDInside == deliveries.ID);
                        Connect.bd.DeliveriesProducts.Remove(objB);
                        Connect.bd.SaveChanges();
                        ChangedIDIsnideDeliver();
                    }


                    Connect.bd.Deliveries.Remove(deliveries);
                    Connect.bd.SaveChanges();
                    MyFrame.Navigate(new StatisticsOnOrdersPage(1));
                }
            }
            if (deliveries.Status < 100)                                                                                                                                                                     //если выполнена не полностью
            {
                if (MessageBox.Show($"Вы действительно хотите закрыть поставку {deliveries.СustomerТame} раньше ?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var objA = Connect.bd.DeliveriesProducts.Where(p=>p.IDInside == deliveries.ID).Count();
                    for(int j = 0; j < objA; j++)
                    {
                        var objB= Connect.bd.DeliveriesProducts.First(p=>p.IDInside== deliveries.ID);
                        var objC = Connect.bd.StatisticOrders.First(p => p.NameDitales == objB.CodeDitals);
                        statisticOrders = objC;
                        statisticOrders.NeseseryCount = statisticOrders.NeseseryCount - (objB.NecessaryCountDitals- objB.ReadyDitals);
                        Connect.bd.DeliveriesProducts.Remove(objB);
                        Connect.bd.SaveChanges();
                    }
                    Connect.bd.Deliveries.Remove(deliveries);
                    Connect.bd.SaveChanges();
                    ChangedIDIsnideDeliver();
                    MyFrame.Navigate(new StatisticsOnOrdersPage(0));
                }
            }

        }

        void ChangedIDIsnideDeliver()
        {
            var c = Connect.bd.Deliveries.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.Deliveries.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.Deliveries.Select(q => q.ID).Max();
                int minID = Connect.bd.Deliveries.Select(q => q.ID).Min();


                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.Deliveries.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.Deliveries.First(p => p.ID == n);
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

        private void СancelDeliver_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы действительно хотите отменить поставку {deliveries.СustomerТame} ?", "Уведомление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

            }
        }

        private void ComplectDeliver_Click(object sender, RoutedEventArgs e)
        {
            var item = Connect.bd.Deliveries.First(p => p.ID == DeliverID);
            var CountPosition = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).Count();
            int SumReadyDitales = 0;
            int SumNeseseryDitales = 0;
            CountPosition++;
            for (int j = 1; j < CountPosition; j++)
            {
                var objA = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID && p.NumberPosition == j).Count();//проверям таблицу на пустоту
                if (objA != 0)
                {
                    var objB = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID && p.NumberPosition == j);
                    var objC = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0).Count();                                                                      //проверяем, в какой таблице находится деатль(платик или с произдовдства)
                    var objD = Connect.bd.DitalesProduction.Where(p => p.CodeDitales == objB.CodeDitals && p.EngravingStatus > 0).Count();
                    if (objC != 0)
                    {                                                                                                                                                                             //из таблици с пластиковыми изделяим
                        var objE = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0);
                        deliveriesProducts = objB;
                        plasticProducts = objE;
                        if (objE.EngravingStatus != 0)
                        {
                            for (int n = 0; n < deliveriesProducts.NecessaryCountDitals; n++) // берем число необходимого кол-ва для конкретной детали
                            {
                                if (deliveriesProducts.ReadyDitals == deliveriesProducts.NecessaryCountDitals)//если позиция выполенена, пропускаем все
                                {
                                    break;
                                }
                                else //если нет, продолжаем
                                {
                                    for (int a = 0; a < objC; a++) //сколько позиций имеют название нужной детлами (игнорируя тип, название ... пластика)
                                    {
                                        var objR = Connect.bd.PlasticProducts.Where(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0).Count();//проверяем, остались ли ещё после каждой  интерации for
                                        if (objR != 0)
                                        {
                                            if (deliveriesProducts.ReadyDitals == deliveriesProducts.NecessaryCountDitals)//если позиция выполенена, пропускаем все
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                objE = Connect.bd.PlasticProducts.First(p => p.ProductTypeID == objB.CodeDitals && p.EngravingStatus > 0);
                                                plasticProducts = objE;
                                                deliveriesProducts.ReadyDitals = deliveriesProducts.ReadyDitals + 1;
                                                Connect.bd.SaveChanges();
                                                plasticProducts.EngravingStatus = plasticProducts.EngravingStatus - 1;
                                                plasticProducts.CountOnStoock = plasticProducts.CountOnStoock - 1;
                                                Connect.bd.SaveChanges();
                                                statisticOrders= Connect.bd.StatisticOrders.First(p => p.NameDitales == objB.CodeDitals);
                                                statisticOrders.NeseseryCount = statisticOrders.NeseseryCount - 1;
                                                Connect.bd.SaveChanges();
                                            }
                                        }
                                 
                                    }
                                }
                            }
                        }
                    }

                    if (objD != 0)
                    {
                        //из таблици с изделиями с производства
                        var objE = Connect.bd.DitalesProduction.First(p => p.CodeDitales == objB.CodeDitals && p.EngravingStatus > 0);
                        deliveriesProducts = objB;
                        ditalesProduction = objE;
                        if (objE.EngravingStatus != 0)
                        {
                            for (int n = 0; n < deliveriesProducts.NecessaryCountDitals; n++)
                            {
                                if (objE.EngravingStatus != 0)
                                {
                                    if (deliveriesProducts.ReadyDitals == deliveriesProducts.NecessaryCountDitals)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        deliveriesProducts.ReadyDitals = deliveriesProducts.ReadyDitals + 1;
                                        Connect.bd.SaveChanges();
                                        ditalesProduction.EngravingStatus = ditalesProduction.EngravingStatus - 1;
                                        ditalesProduction.CountOnStoock = ditalesProduction.CountOnStoock - 1;
                                        Connect.bd.SaveChanges();
                                        statisticOrders = Connect.bd.StatisticOrders.First(p => p.NameDitales == objB.CodeDitals);
                                        statisticOrders.NeseseryCount = statisticOrders.NeseseryCount - 1;
                                        Connect.bd.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            deliveries = item;
            SumNeseseryDitales = 0;
            SumReadyDitales = 0;
            var objK = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).Count();                                                                                                              //количество позиций в поставке
            var objL = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID);
            int ID = objL.ID;

            // считаем провент готовности
            for (int j = 0; j < objK; j++)
            {
                var objE = Connect.bd.DeliveriesProducts.First(p => p.IDInside == item.ID && p.ID == ID);
                deliveriesProducts = objE;
                SumNeseseryDitales = SumNeseseryDitales + int.Parse(deliveriesProducts.NecessaryCountDitals.ToString());
                SumReadyDitales = SumReadyDitales + int.Parse(deliveriesProducts.ReadyDitals.ToString());
                ID++;
            }
            if (SumNeseseryDitales > 0) deliveries.Status = (SumReadyDitales * 100) / SumNeseseryDitales;
            else deliveries.Status = 0;
            Connect.bd.SaveChanges();
            ProcentText.Text = item.Status.ToString();
            DeliversInfoView.ItemsSource = Connect.bd.DeliveriesProducts.Where(p => p.IDInside == item.ID).ToList();
        }
    }
}
