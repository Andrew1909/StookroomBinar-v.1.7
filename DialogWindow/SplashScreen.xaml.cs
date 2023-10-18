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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Data.SqlClient;

using System.Net.NetworkInformation; //Include this
using System.Data;

namespace StockroomBinar.DialogWindow
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChenged;
            worker.RunWorkerAsync();
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for(int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(15);
            }
        }
       

        void worker_ProgressChenged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (progressBar.Value == 100)
            {
                string connectionString = "Data Source=IT-OPERATOR\\MSSQLSERVER01;Initial Catalog=StockroomBinar;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    MainWindow mainwindow = new MainWindow();
                    Close();
                    mainwindow.ShowDialog();
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к БД.");
                    Close();
                }

            }
        }
    }
}
