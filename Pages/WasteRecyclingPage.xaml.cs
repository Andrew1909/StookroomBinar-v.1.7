using StockroomBinar.BD;
using StockroomBinar.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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
    /// Логика взаимодействия для WasteRecyclingPage.xaml
    /// </summary>
    public partial class WasteRecyclingPage : Page
    {
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();
        public ColorPlastic colorPlastic = new ColorPlastic();

        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string ManufacturerPlast;//для записи названия производителя из комбобокс
        string[,] NameRecuclingPlast = new string[99, 99];//массив для хранения названий выбранных цветов для утелизации

        public WasteRecyclingPage()
        {
            InitializeComponent();

            MyFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            var CountPosition = Connect.bd.RecyclingPlastic.Where(p => p.IDInside != 0).Count(); //обнуляем все чекбоксы в бд
            if (CountPosition != 0)
            {
                CountPosition++;
                for (int j = 1; j < CountPosition; j++)
                {
                    var objA = Connect.bd.RecyclingPlastic.First(p => p.IDInside == j);
                    recyclingPlastic = objA;
                    recyclingPlastic.StatusRecucling = false;
                    Connect.bd.SaveChanges();
                }
            }

            var a = Connect.bd.PlasticType.Where(p => p.ID != 0).Count(); //добавлние типов пластика из БД
            PlastType.Items.Add("Все типы");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var a1 = Connect.bd.PlasticType.First(p => p.ID == j);
                PlastType.Items.Add(a1.NameType.ToString());
            }
            PlastType.SelectedIndex = 0;

            var a2 = Connect.bd.IDManufacturer.Where(p => p.ID != 0).Count(); //считаем количество производителей
            PlastManufact.Items.Add("Все производители");
            for (int j = 1; j <= int.Parse(a2.ToString()); j++)
            {
                var a3 = Connect.bd.IDManufacturer.First(p => p.IDInside == j);
                PlastManufact.Items.Add(a3.NameManufacturer.ToString());
            }
            PlastManufact.SelectedIndex = 0;

            PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.ToList();
        }
        private void RecyclingPlast_Click(object sender, RoutedEventArgs e)
        {
            for(int j = 0; j < 99; j++)
            {
                if(NameRecuclingPlast[j, 0] != null)
                {
                    var a = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == NameRecuclingPlast[j, 0]);
                    MessageBox.Show(a.ColorNameRecucling);
                }
            }
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //SearchColor.Select(SearchColor.Text.Length, 10);
            if (SearchColor.Text == null || SearchColor.Text == "")
            {
                if (TypeNamePlast != null && ManufacturerPlast != null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.PlasticTypeRecucling == TypeNamePlast && p.ManufacturerRecucling == ManufacturerPlast).ToList();
                }

                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ManufacturerRecucling == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.PlasticTypeRecucling == TypeNamePlast).ToList();
                }
                if (TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.ToList();
                }

            }
            else
            {
                if (TypeNamePlast != null && ManufacturerPlast != null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling.StartsWith(SearchColor.Text) && p.PlasticTypeRecucling == TypeNamePlast && p.ManufacturerRecucling == ManufacturerPlast).ToList();
                }
                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling.StartsWith(SearchColor.Text) && p.PlasticTypeRecucling == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling.StartsWith(SearchColor.Text) && p.PlasticTypeRecucling == TypeNamePlast).ToList();
                }
                if (TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling.StartsWith(SearchColor.Text)).ToList();
                }
            }
        }

        private void PlastType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = PlastType.SelectedIndex;
            if (PlastType.SelectedIndex == index)
            {
                if (index > 0)
                {
                    var a1 = Connect.bd.PlasticType.First(p => p.ID == index);
                    TypeNamePlast = a1.NameType;

                    if (ManufacturerPlast != null)
                    {
                        PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ManufacturerRecucling == ManufacturerPlast && p.PlasticTypeRecucling == TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.PlasticTypeRecucling == TypeNamePlast).ToList();
                    }
                }
            }
            if (PlastType.SelectedIndex == 0)
            {
                if (ManufacturerPlast != null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ManufacturerRecucling == ManufacturerPlast).ToList();
                }
                else
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.ToList();
                }

                TypeNamePlast = null;
            }
        }

        private void AddRecyclingName_Checked(object sender, RoutedEventArgs e)
        {
            var a = PlastitRecyclingView.SelectedItem as RecyclingPlastic;
            if (a != null)
            {
                if (NameRecuclingPlast[98, 0] != null) MessageBox.Show("Выбрано максимальное количество элементов для одной утелизации!");
                else
                {
                    for (int j = 0; j < 99; j++)
                    {
                        if (NameRecuclingPlast[j,0] == null)
                        {
                            NameRecuclingPlast[j, 0] = a.ColorNameRecucling;
                            recyclingPlastic = a;
                            recyclingPlastic.StatusRecucling = true;
                            Connect.bd.SaveChanges();
                            break;
                        }
                    }
                }   
            }
        }

        private void AddRecyclingName_Unchecked(object sender, RoutedEventArgs e)
        {
            var a = PlastitRecyclingView.SelectedItem as RecyclingPlastic;
            if (a != null)
            {
                for (int j = 0; j < 99; j++)
                {
                    if (NameRecuclingPlast[j, 0] == a.ColorNameRecucling)
                    {
                        NameRecuclingPlast[j, 0] = null;
                        recyclingPlastic = a;
                        recyclingPlastic.StatusRecucling = false;
                        Connect.bd.SaveChanges();
                        break;
                    }
                }
            }
        }

        private void RecyclingNameDel_Click(object sender, RoutedEventArgs e)
        {
            int PlasticStat;
            for (int j = 0; j < 99; j++)
            {
                if (NameRecuclingPlast[j, 0] != null)
                {
                    string v = NameRecuclingPlast[j, 0];
                    var a = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == v);
                    PlasticStat = a.PlasticStatus.Value;
                    Connect.bd.RecyclingPlastic.Remove(a);
                    Connect.bd.SaveChanges();
                    var objA = Connect.bd.PlasticStor.Where(p => p.ColorName == v).Count();
                    var objB = Connect.bd.DefectivePlastic.Where(p => p.ColorName == v).Count();
                    var objC = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == v).Count();
                    if (objA==0&&objB==0&&objC==0)
                    { 
                        var b = Connect.bd.ColorPlastic.First(p => p.NameColor == v);
                        Connect.bd.ColorPlastic.Remove(b);
                        Connect.bd.SaveChanges();
                    }
                }
            }   
            for(int j=0; j <99; j++)
            {
                NameRecuclingPlast[j, 0] = null;
            }


            var c = Connect.bd.ColorPlastic.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.ColorPlastic.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.ColorPlastic.Select(q => q.ID).Max();
                int minID = Connect.bd.ColorPlastic.Select(q => q.ID).Min();


                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.ColorPlastic.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.ColorPlastic.First(p => p.ID == n);
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
            ChangedIDIsnideDeliver();


            PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.ToList();
        }


        void ChangedIDIsnideDeliver()
        {
            var c = Connect.bd.RecyclingPlastic.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.RecyclingPlastic.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.RecyclingPlastic.Select(q => q.ID).Max();
                int minID = Connect.bd.RecyclingPlastic.Select(q => q.ID).Min();


                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.RecyclingPlastic.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.RecyclingPlastic.First(p => p.ID == n);
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

        private void PlastManufact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index2 = PlastManufact.SelectedIndex;
            if (PlastManufact.SelectedIndex == index2)
            {
                if (index2 > 0)
                {
                    var a1 = Connect.bd.IDManufacturer.First(p => p.IDInside == index2);
                    ManufacturerPlast = a1.NameManufacturer;
                    if (TypeNamePlast != null)
                    {
                        PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ManufacturerRecucling == ManufacturerPlast && p.PlasticTypeRecucling == TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.ManufacturerRecucling == ManufacturerPlast).ToList();
                    }
                }
            }

            if (PlastManufact.SelectedIndex == 0)
            {
                if (TypeNamePlast != null)
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.Where(p => p.PlasticTypeRecucling == TypeNamePlast).ToList();
                }
                else
                {
                    PlastitRecyclingView.ItemsSource = Connect.bd.RecyclingPlastic.ToList();
                }
                ManufacturerPlast = null;
            }
        }
    }
}
