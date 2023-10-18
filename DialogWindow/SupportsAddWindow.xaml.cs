﻿using System;
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
    /// Логика взаимодействия для SupportsAddWindow.xaml
    /// </summary>
    public partial class SupportsAddWindow : Window
    {
        public SupportsAddWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

        }
        public string Supports
        {
            get { return NewWightSuports.Text; }
        }

        private void NewWightSuports_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val) && e.Text != ",")
            {
                e.Handled = true; // отклоняем ввод
            }
        }
    }
}
