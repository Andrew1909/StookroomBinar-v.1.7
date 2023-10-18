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

namespace StockroomBinar.DialogWindow
{
    /// <summary>
    /// Логика взаимодействия для AddDefectiveCoilsWindow.xaml
    /// </summary>
    public partial class AddDefectiveCoilsWindow : Window
    {
        public AddDefectiveCoilsWindow()
        {
            InitializeComponent();
            DefectiveTypePlast.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DefectiveTypePlast.SelectedIndex == 0)
            {
                MessageBox.Show("Выберите тип дефекта");
            }
            else
            {
                this.DialogResult = true;
            }
            
        }
        public string Defective
        {
            get { return CountDefectiveCoils.Text; }
        }

        public string DefectiveType
        {
            get 
            {
               
                if (DefectiveTypePlast.SelectedIndex== 1) return "Посторонние включения";
                if (DefectiveTypePlast.SelectedIndex == 2) return "Нестабильный диаметр прутка";
                return null; 
            }
        }

        private void CountDefectiveCoils_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }
    }
}
