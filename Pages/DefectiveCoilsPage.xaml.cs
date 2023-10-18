using StockroomBinar.BD;
using StockroomBinar.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Логика взаимодействия для DefectiveCoilsPage.xaml
    /// </summary>
    public partial class DefectiveCoilsPage : Page
    {
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();
        public ColorPlastic colorPlastic = new ColorPlastic();
        public DefectivePlastic defectivePlastic = new DefectivePlastic();

        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string ManufacturerPlast;//для записи названия производителя из комбобокс
        string[,] NameRecuclingPlast = new string[99, 99];//массив для хранения названий выбранных цветов для утелизации



        public DefectiveCoilsPage()
        {
            InitializeComponent();


            var CountPosition = Connect.bd.DefectivePlastic.Where(p => p.IDInside != 0).Count(); //обнуляем все чекбоксы в бд
            if (CountPosition != 0)
            {
                CountPosition++;
                for (int j = 1; j < CountPosition; j++)
                {
                    var objA = Connect.bd.DefectivePlastic.First(p => p.IDInside == j);
                    defectivePlastic = objA;
                    defectivePlastic.StatusRecucling = false;
                    Connect.bd.SaveChanges();
                }
            }

            var a = Connect.bd.PlasticType.Where(p => p.ID != 0).Count();
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
            PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
        }

        private void AddRecyclingName_Checked(object sender, RoutedEventArgs e)
        {
            var a = PlastitDefectiveView.SelectedItem as DefectivePlastic;

            if (a != null)
            {
                if (NameRecuclingPlast[98, 0] != null) MessageBox.Show("Выбрано максимальное количество элементов для одной утелизации!");
                else
                {
                    for (int j = 0; j < 99; j++)
                    {
                        if (NameRecuclingPlast[j, 0] == null)
                        {
                            NameRecuclingPlast[j, 0] = a.ColorName;
                            defectivePlastic = a;
                            defectivePlastic.StatusRecucling = true;
                            Connect.bd.SaveChanges();
                            break;
                        }
                    }
                }
            }
        }

        private void AddRecyclingName_Unchecked(object sender, RoutedEventArgs e)
        {
            var a = PlastitDefectiveView.SelectedItem as DefectivePlastic;
            if (a != null)
            {

                for (int j = 0; j < 99; j++)
                {
                    if (NameRecuclingPlast[j, 0] == a.ColorName)
                    {
                        NameRecuclingPlast[j, 0] = null;

                        defectivePlastic = a;
                        defectivePlastic.StatusRecucling = false;
                        Connect.bd.SaveChanges();
                        break;
                    }
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
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.Manufacturer == ManufacturerPlast && p.PlasticType == TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.PlasticType == TypeNamePlast).ToList();
                    }
                }
            }
            if (PlastType.SelectedIndex == 0)
            {
                if (ManufacturerPlast != null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                }
                else
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
                }

                TypeNamePlast = null;
            }
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //SearchColor.Select(SearchColor.Text.Length, 10);
            if (SearchColor.Text == null || SearchColor.Text == "")
            {
                if (TypeNamePlast != null && ManufacturerPlast != null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.PlasticType == TypeNamePlast && p.Manufacturer == ManufacturerPlast).ToList();
                }

                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
                if (TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
                }

            }
            else
            {
                if (TypeNamePlast != null && ManufacturerPlast != null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.PlasticType == TypeNamePlast && p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.PlasticType == TypeNamePlast).ToList();
                }
                if (TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.ColorName.StartsWith(SearchColor.Text)).ToList();
                }
            }
        }

        private void RecyclingNameDel_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 0; j < 99; j++)
            {
                if (NameRecuclingPlast[j, 0] != null)
                {
                    string NamePlast = NameRecuclingPlast[j, 0];
                    var objA = Connect.bd.DefectivePlastic.First(p => p.ColorName == NamePlast);

                    var m = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == NamePlast && p.PlasticTypeRecucling == objA.PlasticType && p.ManufacturerRecucling == objA.Manufacturer).Count();
                    if (m > 0)
                    {
                        var m1 = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == NamePlast && p.PlasticTypeRecucling == objA.PlasticType && p.ManufacturerRecucling == objA.Manufacturer);
                        recyclingPlastic = m1;
                        recyclingPlastic.WeightRecucling = objA.Weight + recyclingPlastic.WeightRecucling; //!!!!!!!
                        Connect.bd.SaveChanges();
                        Connect.bd.DefectivePlastic.Remove(objA);
                        Connect.bd.SaveChanges();
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();

                    }
                    else
                    {
                        var a = Connect.bd.RecyclingPlastic.Where(p => p.ID != 0).Count(); //считаем количество пластика
                        if (a == 0)
                        {
                            recyclingPlastic.IDInside = 1;
                        }
                        else recyclingPlastic.IDInside = int.Parse(a.ToString()) + 1;

                        recyclingPlastic.ColorNameRecucling = objA.ColorName;
                        recyclingPlastic.PlasticTypeRecucling = objA.PlasticType;
                        recyclingPlastic.ManufacturerRecucling = objA.Manufacturer;
                        recyclingPlastic.WeightRecucling = objA.Weight;
                        recyclingPlastic.PlasticStatus = objA.PlasticStatus;
                        recyclingPlastic.StatusRecucling = false;
                        Connect.bd.RecyclingPlastic.Add(recyclingPlastic);
                        Connect.bd.SaveChanges();
                        Connect.bd.DefectivePlastic.Remove(objA);
                        Connect.bd.SaveChanges();
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
                    }
                }
            }
            for (int j = 0; j < 99; j++)
            {
                NameRecuclingPlast[j, 0] = null;
            }
            PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
            ChangedIDIsnideDeliver();
        }

        void ChangedIDIsnideDeliver()
        {
            var c = Connect.bd.DefectivePlastic.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.DefectivePlastic.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.DefectivePlastic.Select(q => q.ID).Max();
                int minID = Connect.bd.DefectivePlastic.Select(q => q.ID).Min();


                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.DefectivePlastic.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.DefectivePlastic.First(p => p.ID == n);
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
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.Manufacturer == ManufacturerPlast && p.PlasticType == TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                    }
                }
            }

            if (PlastManufact.SelectedIndex == 0)
            {
                if (TypeNamePlast != null)
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
                else
                {
                    PlastitDefectiveView.ItemsSource = Connect.bd.DefectivePlastic.ToList();
                }
                ManufacturerPlast = null;
            }
        }



        private void PlastitDefectiveView_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }
    }
}
