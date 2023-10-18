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
using System.IO;
using System.Diagnostics;
using StockroomBinar.BD;
namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для Print3DProfilsPage.xaml
    /// </summary>
    public partial class Print3DProfilsPage : Page
    {
        public Profiles profiles = new Profiles();
        public Print3DProfilsPage()
        {
            InitializeComponent();
            PrintProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 2).ToList();
        }

        private void Search_SelectionChanged(object sender, RoutedEventArgs e)
        {
            PrintProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.NameProfile.StartsWith(Search.Text) && p.Type == 2).ToList();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var a = PrintProfilsView.SelectedItem as Profiles;
            if (a != null)
            {
                System.Diagnostics.Process.Start(a.Folder);
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var a = PrintProfilsView.SelectedItem as Profiles;
            if (a != null)
            {
                System.IO.Directory.Delete(a.Folder, true);
                Connect.bd.Profiles.Remove(a);
                Connect.bd.SaveChanges();
                PrintProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 2).ToList();
                MyFrame.Navigate(new Print3DProfilsPage());
            }
        }

        private void AddNewProfile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            // Launch OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == true)
            {
                string path1 = @"C:\BinarStokroom\Profiles\Profiles3Dprint\" + openFileDlg.SafeFileName;
                DirectoryInfo dirInfo = new DirectoryInfo(path1);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string path = openFileDlg.FileName;
                string path2 = @"C:\BinarStokroom\Profiles\Profiles3Dprint\" + openFileDlg.SafeFileName + @"\" + openFileDlg.SafeFileName;
                File.Copy(path, path2, true);

                profiles.NameProfile = openFileDlg.SafeFileName;
                profiles.Folder = path1;
                profiles.Type = 2;
                Connect.bd.Profiles.Add(profiles);
                Connect.bd.SaveChanges();
                PrintProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 2).ToList();
                MyFrame.Navigate(new Print3DProfilsPage());
            }
            MyFrame.Navigate(new Print3DProfilsPage());
        }
    }
}
