using Microsoft.Win32;
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
    /// Логика взаимодействия для EngravingProfilPage.xaml
    /// </summary>
    public partial class EngravingProfilPage : Page
    {

        public Profiles profiles = new Profiles();
        public EngravingProfilPage()
        {
            InitializeComponent();
            EngravingProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 1).ToList();
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
                string path1 = @"C:\BinarStokroom\Profiles\ProfilesEgraving\" + openFileDlg.SafeFileName;
                DirectoryInfo dirInfo = new DirectoryInfo(path1);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string path = openFileDlg.FileName;
                string path2 = @"C:\BinarStokroom\Profiles\ProfilesEgraving\" + openFileDlg.SafeFileName + @"\" + openFileDlg.SafeFileName;
                File.Copy(path, path2, true);

                profiles.NameProfile = openFileDlg.SafeFileName;
                profiles.Folder = path1;
                profiles.Type = 1;
                Connect.bd.Profiles.Add(profiles);
                Connect.bd.SaveChanges();
                EngravingProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 1).ToList();
                MyFrame.Navigate(new EngravingProfilPage());
            }
            MyFrame.Navigate(new EngravingProfilPage());
        }

        private void Serch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            EngravingProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.NameProfile.StartsWith(Serch.Text) && p.Type==1).ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var a = EngravingProfilsView.SelectedItem as Profiles;
            if (a != null)
            {
                System.Diagnostics.Process.Start(a.Folder);
            }

        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var a = EngravingProfilsView.SelectedItem as Profiles;
            if (a != null)
            {
                System.IO.Directory.Delete(a.Folder, true);
                Connect.bd.Profiles.Remove(a);
                Connect.bd.SaveChanges();
                EngravingProfilsView.ItemsSource = Connect.bd.Profiles.Where(p => p.Type == 1).ToList();
                MyFrame.Navigate(new EngravingProfilPage());
            }
        }
    }
}
