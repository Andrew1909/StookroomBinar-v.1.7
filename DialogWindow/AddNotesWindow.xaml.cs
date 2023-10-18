using StockroomBinar.BD;
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
    /// Логика взаимодействия для AddNotesWindow.xaml
    /// </summary>
    public partial class AddNotesWindow : Window
    {

        public NotesUser notesUser = new NotesUser();
        public AddNotesWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewNotes.Text == null || NewNotes.Text == "") MessageBox.Show("Не все понля заполнены");
            else
            {
                this.DialogResult = true;

            }

        }

        public string Color
        {
            get { return NewNotes.Text; }
        }
    }
}
