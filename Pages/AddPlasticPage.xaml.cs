using StockroomBinar.BD;
using StockroomBinar.Class;
using StockroomBinar.DialogWindow;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace StockroomBinar.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPlasticPage.xaml
    /// </summary>
    public partial class AddPlasticPage : Page
    {
        public PlasticStor plasticStor = new PlasticStor();
        public ColorPlastic colorPlastic = new ColorPlastic();
        public PlasticType plasticType = new PlasticType();
        public DefectivePlastic defectivePlastic = new DefectivePlastic();

        string ColorNamePlast;//для запси названия цвета платика, выбранного из комбобокс
        string TypeNamePlast;//для запси названия типа платика, выбранного из комбобокс
        string AddManufactPlastic;
        string[,] DefectivePlastics = new string[99, 2];
        int DefectiveCoilsCount=0;//количество бракованных катушек

        byte[] image_bytes;
        byte[] noimage_bytes;
        public AddPlasticPage()
        {
            InitializeComponent();
           
            for(int j = 0; j < 99; j++)
            {
                DefectivePlastics[j, 0] = null;
            }
            var a = Connect.bd.ColorPlastic.Where(p => p.ID != 0).Count();
            AddColordNamePlastic.Items.Add("Выберите цвет платика");
            for (int j = 1; j <= int.Parse(a.ToString()); j++)
            {
                var s = Connect.bd.ColorPlastic.Where(p => p.IDInside == j).Count();
                if (s != 0)
                {
                    var a1 = Connect.bd.ColorPlastic.First(p => p.IDInside == j);
                    AddColordNamePlastic.Items.Add(a1.NameColor.ToString());
                }
                
            }
            AddColordNamePlastic.SelectedIndex = 0;

            var a2 = Connect.bd.IDManufacturer.Where(p => p.ID != 0).Count(); //считаем количество производителей
            AddManufact.Items.Add("Выберите производителя");
            for (int j = 1; j <= int.Parse(a2.ToString()); j++)
            {
                var a3 = Connect.bd.IDManufacturer.First(p => p.IDInside == j);
                AddManufact.Items.Add(a3.NameManufacturer.ToString());
            }
            AddManufact.SelectedIndex = 0;

            var b = Connect.bd.PlasticType.Where(p => p.ID != 0).Count();
            AddTypePlastic.Items.Add("Выберите тип пластика");
            for (int j = 1; j <= int.Parse(b.ToString()); j++)
            {
                var b1 = Connect.bd.PlasticType.First(p => p.ID == j);
                AddTypePlastic.Items.Add(b1.NameType.ToString());
            }
            AddTypePlastic.SelectedIndex = 0;
            CountDefectCoilsText.Visibility = Visibility.Hidden;
        }

        private void AddPlast_Click(object sender, RoutedEventArgs e)
        {
            if(AddTypePlastic.SelectedIndex==0|| AddColordNamePlastic .SelectedIndex== 0|| AddWightPlastic.Text==null|| AddCoilsPlastic.Text==null||AddManufact.SelectedIndex==0|| AddWightPlastic.Text == "" || AddCoilsPlastic.Text == "" || AddManufact.SelectedIndex == 0)
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                int index1 = AddColordNamePlastic.SelectedIndex;
                if (AddColordNamePlastic.SelectedIndex == index1)
                {
                    if (index1 > 0)
                    {
                        var a1 = Connect.bd.ColorPlastic.First(p => p.IDInside == index1);
                        ColorNamePlast = a1.NameColor;
                    }
                }
                int index2 = AddTypePlastic.SelectedIndex;
                if (AddTypePlastic.SelectedIndex == index2)
                {
                    if (index2 > 0)
                    {
                        var a1 = Connect.bd.PlasticType.First(p => p.ID == index2);
                        TypeNamePlast = a1.NameType;
                    }
                }

                var objA = Connect.bd.PlasticStor.Where(p => p.ColorName == ColorNamePlast && p.PlasticType== TypeNamePlast && p.Manufacturer==AddManufactPlastic).Count();
                if (objA != 0)
                {
                    if (DefectiveCoilsCount == int.Parse(AddCoilsPlastic.Text)) { 
                        SaveDefectivePlast(1);
                        MessageBox.Show("Вся пратия пластика добавлена в брак!");
                        MyFrame.Navigate(new AddPlasticPage());
                    }
                    else
                    {
                        if (DefectiveCoilsCount != 0) SaveDefectivePlast(1);
                        var ObjB = Connect.bd.PlasticStor.First(p => p.ColorName == ColorNamePlast && p.PlasticType == TypeNamePlast && p.Manufacturer == AddManufactPlastic);
                        ObjB.Weight = (decimal.Parse(AddWightPlastic.Text) - DefectiveCoilsCount) + ObjB.Weight;
                        ObjB.NumberСoils = (int.Parse(AddCoilsPlastic.Text) - DefectiveCoilsCount) + ObjB.NumberСoils;
                        ObjB.Notes = AddNotesPlastic.Text;
                        Connect.bd.SaveChanges();
                        MessageBox.Show("Пластик добавлен!");
                        MyFrame.Navigate(new AddPlasticPage());
                    }
                }

                if (objA == 0)
                {
                    if (DefectiveCoilsCount == int.Parse(AddCoilsPlastic.Text))
                    {
                        SaveDefectivePlast(0);
                        MessageBox.Show("Вся пратия пластика добавлена в брак!");
                        MyFrame.Navigate(new AddPlasticPage());
                    }
                    else
                    {
                        if (DefectiveCoilsCount != 0) SaveDefectivePlast(0);
                        var a = Connect.bd.PlasticStor.Where(p => p.ID != 0).Count(); //считаем количество типов пластика
                        plasticStor.IDInsaid = int.Parse(a.ToString()) + 1;
                        plasticStor.ColorName = ColorNamePlast;
                        plasticStor.PlasticType = TypeNamePlast;
                        plasticStor.Weight = decimal.Parse(AddWightPlastic.Text) - DefectiveCoilsCount;
                        plasticStor.NumberСoils = int.Parse(AddCoilsPlastic.Text) - DefectiveCoilsCount;
                        plasticStor.Manufacturer = AddManufactPlastic;
                        plasticStor.Notes = AddNotesPlastic.Text;
                        if (image_bytes != null)
                        {
                            plasticStor.Image = image_bytes;
                        }
                        else
                        {
                            plasticStor.Image = File.ReadAllBytes(@"D:\StockroomBinar — копия\Image\plastikNoPhotoIco.png");
                        }
                        
                        Connect.bd.PlasticStor.Add(plasticStor);
                        Connect.bd.SaveChanges();
                        MessageBox.Show("Пластик добавлен!");
                        MyFrame.Navigate(new AddPlasticPage());
                    }
                    
                }
            }
        }

        void SaveDefectivePlast(int status)
        {
            for(int j = 0; j < 99; j++)
            {
                if (DefectivePlastics[j, 0] != null)
                {
                    string deffect = DefectivePlastics[j, 1];
             
                    var objZ = Connect.bd.DefectivePlastic.Where(p => p.ColorName == ColorNamePlast && p.PlasticType == TypeNamePlast && p.Manufacturer == AddManufactPlastic && p.DefectiveType == deffect).Count();
                    if (objZ != 0)
                    {
                        var objX = Connect.bd.DefectivePlastic.First(p => p.ColorName == ColorNamePlast && p.PlasticType == TypeNamePlast && p.Manufacturer == AddManufactPlastic && p.DefectiveType == deffect);
                        defectivePlastic = objX;
                        defectivePlastic.Weight = defectivePlastic.Weight + int.Parse(DefectivePlastics[j, 0]);
                        defectivePlastic.NumberСoils = defectivePlastic.NumberСoils + int.Parse(DefectivePlastics[j, 0]);
                        Connect.bd.SaveChanges();
                        //обновляем
                    }
                    else
                    {
                        var IDCount = Connect.bd.DefectivePlastic.Where(p => p.IDInside != 0).Count();
                        if (IDCount == 0) defectivePlastic.IDInside = 1;
                        else defectivePlastic.IDInside = IDCount + 1;
                        defectivePlastic.ColorName = ColorNamePlast;
                        defectivePlastic.PlasticType = TypeNamePlast;
                        defectivePlastic.Manufacturer = AddManufactPlastic;
                        defectivePlastic.Weight = decimal.Parse(DefectivePlastics[j, 0]);
                        defectivePlastic.NumberСoils = int.Parse(DefectivePlastics[j, 0]);
                        defectivePlastic.PlasticStatus = status;
                        defectivePlastic.DefectiveType = DefectivePlastics[j, 1];
                        defectivePlastic.StatusRecucling = false;
                        Connect.bd.DefectivePlastic.Add(defectivePlastic);
                        Connect.bd.SaveChanges();
                    }
                }
            }  
        }

        private void AddCoilsPlastic_SelectionChanged(object sender, RoutedEventArgs e)
        {
            AddWightPlastic.Text = AddCoilsPlastic.Text;
        
        }

        private void AddColordNamePlastic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index2 = AddColordNamePlastic.SelectedIndex;
            if (AddColordNamePlastic.SelectedIndex == index2)
            {
                if (index2 > 0)
                {
                    var a1 = Connect.bd.ColorPlastic.First(p => p.IDInside == index2);
                    var objA = Connect.bd.PlasticStor.Where(p => p.ColorName == a1.NameColor).Count(); //проверка, есть ли цвет или тип в главной таблице
                    if (objA != 0)
                    {
                        var ObjB = Connect.bd.PlasticStor.First(p => p.ColorName == a1.NameColor);
                        var objC = Connect.bd.PlasticType.First(p => p.NameType == ObjB.PlasticType);
                        var objD = Connect.bd.IDManufacturer.First(p => p.NameManufacturer == ObjB.Manufacturer);
                        AddTypePlastic.SelectedIndex = objC.ID;
                        AddManufactPlastic = ObjB.Manufacturer;
                        AddManufact.SelectedIndex = int.Parse((objD.IDInside).ToString());
                        AddNotesPlastic.Text = ObjB.Notes;
                    }
                }
            }
        }

        private void AddNewNameColor_Click(object sender, RoutedEventArgs e)
        {
            AddNewNameColorWindow ColorWindow = new AddNewNameColorWindow();
            var a = Connect.bd.ColorPlastic.Where(p => p.ID != null).Count();
            if (ColorWindow.ShowDialog() == true)
            {
                var check = Connect.bd.ColorPlastic.Where(p => p.NameColor == ColorWindow.Color).Count();
                if (check == 0)
                {
                    colorPlastic.IDInside = a + 1;
                    colorPlastic.NameColor = ColorWindow.Color;
                    Connect.bd.ColorPlastic.Add(colorPlastic);
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Новый цвет добавлен!");
                    MyFrame.Navigate(new AddPlasticPage());
                }
                else MessageBox.Show("Такой цвет уже существует!");

            }
        }

        private void AddNewNameType_Click(object sender, RoutedEventArgs e)
        {
            AddNewTypeWindow TypeWindow = new AddNewTypeWindow();
            var a = Connect.bd.PlasticType.Where(p => p.ID != null).Count();
            if (TypeWindow.ShowDialog() == true)
            {
                var check = Connect.bd.PlasticType.Where(p => p.NameType == TypeWindow.Type).Count();
                if (check == 0)
                {
                    plasticType.ID = a + 1;
                    plasticType.NameType = TypeWindow.Type;
                    Connect.bd.PlasticType.Add(plasticType);
                    Connect.bd.SaveChanges();
                    MessageBox.Show("Новый тип пластика добавлен!");
                    MyFrame.Navigate(new AddPlasticPage());
                }
                else MessageBox.Show("Такой тип уже есть!");
            }
        }

        private void Brak_Checked(object sender, RoutedEventArgs e)
        {
            AddDefectiveCoilsWindow TypeWindow = new AddDefectiveCoilsWindow();
            var a = Connect.bd.PlasticType.Where(p => p.ID != null).Count();
            if (TypeWindow.ShowDialog() == true)
            {
                for(var i = 0; i < 99; i++)
                {
                    if (DefectivePlastics[i, 0] == null)
                    {
                       
                        DefectivePlastics[i, 0] = TypeWindow.Defective;
                        DefectivePlastics[i, 1] = TypeWindow.DefectiveType;
                        break;
                    }
                }
                DefectiveCoilsCount = int.Parse(TypeWindow.Defective.ToString());
                CountDefectCoilsText.Visibility = Visibility.Visible;
                if (DefectiveCoilsCount == 1) CountDefectCoilsText.Text = DefectiveCoilsCount + " катушка будет списана как брак";
                //if (DefectiveCoilsCount>1 && DefectiveCoilsCount <=4) CountDefectCoilsText.Text = DefectiveCoilsCount + " катушки будут списаны как брак";
                else CountDefectCoilsText.Text = DefectiveCoilsCount + " катушек будет списано как брак";
            }
            else
            {
                Brak.IsChecked = false;
            }
        }

        private void Brak_Unchecked(object sender, RoutedEventArgs e)
        {
            CountDefectCoilsText.Visibility = Visibility.Hidden;
            DefectiveCoilsCount = 0;
        }

        private void AddTypePlastic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void AddCoilsPlastic_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void AddWightPlastic_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void AddWightPlastic_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back | e.Key == Key.Space)
            //{
            //    e.Handled = true;
            //}
            //if (e.KeyChar == ',') e.Handled = true;
        }

        private void AddManufact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index2 = AddManufact.SelectedIndex;
            if (AddManufact.SelectedIndex == index2)
            {
                if (index2 > 0)
                {
                    var objA = Connect.bd.IDManufacturer.First(p => p.IDInside == index2);
                    AddManufactPlastic = objA.NameManufacturer;
                }
            }
            if(AddManufact.SelectedIndex == 0)
            {
                AddManufactPlastic = null;
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); // создаем диалоговое окно
            openFileDialog.ShowDialog(); // показываем
           image_bytes = File.ReadAllBytes(openFileDialog.FileName);

        }

        private void AddNotesPlastic_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void AddNotesPlastic_KeyDown(object sender, KeyEventArgs e)
        {
     
        }

        private void AddNotesPlastic_TextInput_1(object sender, TextCompositionEventArgs e)
        {

        }

        private void AddNotesPlastic_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // если пробел, отклоняем ввод
            }
        }

        private void AddNotesPlastic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void AddWightPlastic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }

        private void AddCoilsPlastic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true; // отклоняем ввод
            }
        }
    }


}
