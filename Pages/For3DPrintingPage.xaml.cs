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
    /// Логика взаимодействия для For3DPrintingPage.xaml
    /// </summary>
    public partial class For3DPrintingPage : Page
    {
        public ForPrinters forPrinters = new ForPrinters();

        public For3DPrintingPage()
        {
            InitializeComponent();

            var objB = Connect.bd.ForPrinters.Where(p => p.Count== 0).Count();
            if (objB != 0)
            {
                for(int j = 0; j < objB; j++)
                {
                    var objC = Connect.bd.ForPrinters.First(p => p.Count == 0);
                    Connect.bd.ForPrinters.Remove(objC);
                    Connect.bd.SaveChanges();
                }
            }
            ForPrintView.ItemsSource = Connect.bd.ForPrinters.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var a = ForPrintView.SelectedItem as ForPrinters;
            if (a != null)
            {
                Connect.bd.SaveChanges();
                ForPrintView.ItemsSource = Connect.bd.ForPrinters.ToList();
            }
        }

        private void SearchColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ForPrintView.ItemsSource = Connect.bd.ForPrinters.Where(p => p.Name.StartsWith(SearchColor.Text)).ToList();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            var a = ForPrintView.SelectedItem as ForPrinters;
            if (a != null)
            {
                forPrinters = a;
                forPrinters.Count = forPrinters.Count+1;
                Connect.bd.SaveChanges();
                ForPrintView.ItemsSource = Connect.bd.ForPrinters.ToList();
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            var a = ForPrintView.SelectedItem as ForPrinters;
            if (a != null)
            {
                forPrinters = a;
                if (forPrinters.Count != 0)
                {
                    forPrinters.Count = forPrinters.Count - 1;
                    Connect.bd.SaveChanges();
                }
                ForPrintView.ItemsSource = Connect.bd.ForPrinters.ToList();
            }
        }

        private void AddNewManufacturer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow DataWindow = new AddItemWindow();
            if (DataWindow.ShowDialog() == true)
            {
                forPrinters.Name = DataWindow.NameItem.ToString();
                forPrinters.Count = int.Parse(DataWindow.Count);
                Connect.bd.ForPrinters.Add(forPrinters);
                Connect.bd.SaveChanges();
                ForPrintView.ItemsSource = Connect.bd.ForPrinters.ToList();
            }
        }
    }
}
