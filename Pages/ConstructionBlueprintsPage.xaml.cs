using StockroomBinar.BD;
using StockroomBinar.Class;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ConstructionBlueprintsPage.xaml
    /// </summary>
    public partial class ConstructionBlueprintsPage : Page
    {

        public Blueprints blueprints = new Blueprints();
        public ConstructionBlueprintsPage()
        {
            InitializeComponent();
           ConsstractionsBlueprintsView.ItemsSource = Connect.bd.Blueprints.Where(p => p.Type == 2).ToList(); //чертежи  имеют тип 2 
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var a = ConsstractionsBlueprintsView.SelectedItem as Blueprints;
            if (a != null)
            {
                System.Diagnostics.Process.Start(a.Folder);
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var a = ConsstractionsBlueprintsView.SelectedItem as Blueprints;
            if (a != null)
            {
                System.IO.Directory.Delete(a.Folder, true);
                Connect.bd.Blueprints.Remove(a);
                Connect.bd.SaveChanges();
                ConsstractionsBlueprintsView.ItemsSource = Connect.bd.Blueprints.Where(p => p.Type == 2).ToList();
                MyFrame.Navigate(new ConstructionBlueprintsPage());
            }
        }

        private void AddNewCOnstructionsBlueprints_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            MyFrame.Navigate(new ESKBlueprintsPage());
            if (result == true)
            {
                MyFrame.Navigate(new ConstructionBlueprintsPage());
                string path1 = @"C:\BinarStokroom\Blueprints\Construction Blueprints\" + openFileDlg.SafeFileName;
                DirectoryInfo dirInfo = new DirectoryInfo(path1);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string path = openFileDlg.FileName;
                string path2 = @"C:\BinarStokroom\Blueprints\Construction Blueprints\" + openFileDlg.SafeFileName + @"\" + openFileDlg.SafeFileName;
                File.Copy(path, path2, true);

                blueprints.Name = openFileDlg.SafeFileName;
                blueprints.Folder = path1;
                blueprints.Type = 2;
                Connect.bd.Blueprints.Add(blueprints);
                Connect.bd.SaveChanges();
                ConsstractionsBlueprintsView.ItemsSource = Connect.bd.Blueprints.Where(p => p.Type == 2).ToList();
                MyFrame.Navigate(new ConstructionBlueprintsPage());
            }
            MyFrame.Navigate(new ConstructionBlueprintsPage());
        }

        private void Serch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ConsstractionsBlueprintsView.ItemsSource = Connect.bd.Blueprints.Where(p => p.Name.StartsWith(Serch.Text) && p.Type == 2).ToList();
        }
    }
}
