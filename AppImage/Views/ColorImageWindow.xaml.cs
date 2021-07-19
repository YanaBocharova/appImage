using AppImage.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppImage.Views
{
    /// <summary>
    /// Interaction logic for ColorImageWindow.xaml
    /// </summary>
    public partial class ColorImageWindow : Window
    {
        public ColorImageWindow()
        {
            InitializeComponent();

            DataContext = new ColorImageViewModel();
        }
    }
}
