using MaterialDesignColors;
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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Web.UI.WebControls;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для PlasticStorage.xaml
    /// </summary>
    public partial class PlasticStorage : Page
    {
        public RecyclingPlastic recyclingPlastic = new RecyclingPlastic();
        public PlasticStor plasticStor = new PlasticStor();
        public Notifications notifications = new Notifications();

        private Excel.Application excelapp;
        private Excel.Window excelWindow;
        private Excel.Sheets excelsheets;
        private Excel.Workbooks excelappworkbooks;
        private Excel.Workbook excelappworkbook;
        private Excel.Application excelApp = null;

        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string ManufacturerPlast;//для записи названия производителя из комбобокс
        public PlasticStorage()
        {
            InitializeComponent();
            //SearchColor.CaretIndex = SearchColor.Text.Length;
            SearchColor.Select(SearchColor.Text.Length, 100);
            //SearchColor.CaretIndex = SearchColor.Text.Length;


            MyFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            var a = Connect.bd.PlasticType.Where(p => p.ID != 0).Count(); //считаем количество типов пластика
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


            var plast = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count(); //считаем количество пластика
            plast++;
            for (int i = 0; i < int.Parse(plast.ToString()); i++)
            {
                var plast2 = Connect.bd.PlasticStor.Where(p => p.Weight.Value <= 0.2m ).Count();//считаем количество пластика для списания. МИНИМАЛЬНОЕ ЧИСЛО СПИСАНИЯ ТУТ
                if (plast2 != 0)
                {
                    var objX = Connect.bd.PlasticStor.First(p => p.Weight.Value <= 0.2m);
                    var objY = Connect.bd.RecyclingPlastic.Where(p => p.ColorNameRecucling == objX.ColorName && p.PlasticTypeRecucling==objX.PlasticType &&p.ManufacturerRecucling==objX.Manufacturer).Count();
                    if (objY != 0)
                    {
                        AddNatification(objX.ID);
                        var objY1 = Connect.bd.RecyclingPlastic.First(p => p.ColorNameRecucling == objX.ColorName && p.PlasticTypeRecucling == objX.PlasticType && p.ManufacturerRecucling == objX.Manufacturer);
                        recyclingPlastic = objY1;
                        recyclingPlastic.WeightRecucling = recyclingPlastic.WeightRecucling + objX.Weight;                      
                        Connect.bd.SaveChanges();
                        Connect.bd.PlasticStor.Remove(objX);
                        Connect.bd.SaveChanges();
                    }
                    else
                    {
                        AddNatification(objX.ID);
                        recyclingPlastic.ID = objX.ID;
                        recyclingPlastic.ColorNameRecucling = objX.ColorName;
                        recyclingPlastic.PlasticTypeRecucling = objX.PlasticType;
                        recyclingPlastic.ManufacturerRecucling = objX.Manufacturer;
                        recyclingPlastic.WeightRecucling = objX.Weight;
                        var plast3 = Connect.bd.PlasticStor.Where(p => p.ColorName == objX.ColorName).Count();
                        if (plast3 > 1)
                        {
                            recyclingPlastic.PlasticStatus = 1;
                        }
                        else recyclingPlastic.PlasticStatus = 0;
                        Connect.bd.RecyclingPlastic.Add(recyclingPlastic);
                        Connect.bd.SaveChanges();
                        Connect.bd.PlasticStor.Remove(objX);
                        Connect.bd.SaveChanges();
                    }
                    
                }
            }

            var c = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count();//считаем  количество оставшихся
            if (c != 0)
            {
                var NullElement = Connect.bd.PlasticStor.First(p => p.ID != 0);
                int n = NullElement.ID;
                int t;
                int st = 1;
                int maxID = Connect.bd.PlasticStor.Select(q => q.ID).Max();
                for (int j = 0; j < maxID; j++)
                {
                    t = Connect.bd.PlasticStor.Where(p => p.ID == n).Count();
                    if (t > 0)
                    {
                        NullElement = Connect.bd.PlasticStor.First(p => p.ID == n);
                        NullElement.IDInsaid = st;
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
            PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
        }

        void AddNatification(int id)
        {
            var objX = Connect.bd.PlasticStor.First(p => p.ID == id);
            notifications.Descriptiont = $"Пластик {objX.ColorName} от производятеля {objX.Manufacturer} закончился";
            Connect.bd.Notifications.Add(notifications);
            Connect.bd.SaveChanges();
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
                        PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.Manufacturer == ManufacturerPlast && p.PlasticType == TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.PlasticType == TypeNamePlast).ToList();
                    }
                }
            }
            if (PlastType.SelectedIndex == 0)
            {
                if (ManufacturerPlast != null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                }
                else
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
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
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.PlasticType == TypeNamePlast && p.Manufacturer == ManufacturerPlast).ToList();
                }

                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
                if(TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
                }
                
            }
            else
            {
                if (TypeNamePlast != null && ManufacturerPlast != null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.PlasticType == TypeNamePlast && p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (ManufacturerPlast != null && TypeNamePlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.Manufacturer == ManufacturerPlast).ToList();
                }
                if (TypeNamePlast != null && ManufacturerPlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.ColorName.StartsWith(SearchColor.Text) && p.PlasticType == TypeNamePlast).ToList();
                }
                if (TypeNamePlast == null && ManufacturerPlast == null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.ColorName.StartsWith(SearchColor.Text)).ToList();
                }
            }

        }

        private void AddPlatic_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new AddPlasticPage());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var a = PlastitStoageView.SelectedItem as PlasticStor;
            if (a != null)
            {
                MyFrame.Navigate(new EditInfoPlastPage(a));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintInfo_Click(object sender, RoutedEventArgs e)
        {

            var a = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count();

            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            Excel.Range r = xlWorkSheet.get_Range("A1", "J2");
            //Оформления
            r.Font.Name = "Times New Roman";
            r.Cells.Font.Size = 12;
            r.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;                     //красота
            r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            r.WrapText = true;


            Excel.Range h = xlWorkSheet.get_Range("A1", "O99");
            //Оформления
            h.Font.Name = "Times New Roman";
            h.Cells.Font.Size = 12;
            h.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;                     //красота
            h.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;


            Excel.Range t = xlWorkSheet.get_Range("H1", "J2");
            t.Cells.Font.Name = "Times New Roman";
            //Размер шрифта для диапазона
            t.Cells.Font.Size = 12;
            //Жирный текст
            t.Font.Bold = true;
            t.WrapText = true;

            xlWorkSheet.Cells[1, 1] = "Цвет";
            xlWorkSheet.Cells[1, 1].Font.Bold = true;
            xlWorkSheet.Cells[1, 2] = "Тип платика";
            xlWorkSheet.Cells[1, 2].Font.Bold = true;
            xlWorkSheet.Cells[1, 3] = "Вес";
            xlWorkSheet.Cells[1, 3].Font.Bold = true;
            xlWorkSheet.Cells[1, 4] = "Кол-во катушек";
            xlWorkSheet.Cells[1, 4].Font.Bold = true;
            xlWorkSheet.Cells[1, 5] = "Производитель";                                //заполнение шапки
            xlWorkSheet.Cells[1, 5].Font.Bold = true;

            int CountD = 1;

            for (int j = 0; j < a; j++)
            {
                var objB = Connect.bd.PlasticStor.First(p => p.IDInsaid == CountD);
                CountD++;
                xlWorkSheet.Cells[CountD, 1] = objB.ColorName;
                xlWorkSheet.Cells[CountD, 2] = objB.PlasticType;
                xlWorkSheet.Cells[CountD, 3] = objB.Weight;
                xlWorkSheet.Cells[CountD, 4] = objB.NumberСoils;
                xlWorkSheet.Cells[CountD, 5] = objB.Manufacturer;
            }
            string path = @"C:\BinarStokroom\Reports";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            xlWorkBook.SaveAs("C:\\BinarStokroom\\Reports\\Склад Пластика.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            Process.Start(@"C:\BinarStokroom\Reports\Склад Пластика.xls");

        }

        private void ManufactInfo_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(new ManufacturerInfoPage());
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
                        PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.Manufacturer == ManufacturerPlast && p.PlasticType==TypeNamePlast).ToList();
                    }
                    else
                    {
                        PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.Manufacturer == ManufacturerPlast).ToList();
                    } 
                }
            }

            if (PlastManufact.SelectedIndex == 0)
            {
                if (TypeNamePlast != null)
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.Where(p => p.PlasticType == TypeNamePlast).ToList();
                }
                else
                {
                    PlastitStoageView.ItemsSource = Connect.bd.PlasticStor.ToList();
                }
                ManufacturerPlast = null;
            }
        }

        private void SearchColor_KeyDown(object sender, KeyEventArgs e)
        {
            //if ((int)e.KeyChar >= 97 && (int)e.KeyChar <= 122)
            //{
            //    e.KeyChar = (char)((int)e.KeyChar & 0xDF);
            //}
        }
    }
}
