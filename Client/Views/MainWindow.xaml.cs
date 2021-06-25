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

namespace Baka.Hipster.Burger.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void me(object sender, MouseEventArgs e)
        {
            //------------< StackPanel_MouseEnter() >------------
            pnlPath.Background = new SolidColorBrush(Color.FromRgb(255, 192, 203));
            //------------</ StackPanel_MouseEnter() >------------
        }

        private void ml(object sender, MouseEventArgs e)
        {
            //------------< StackPanel_MouseLeave() >------------
            pnlPath.Background = null;
            //------------</ StackPanel_MouseLeave() >------------
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //------------< StackPanel_MouseDown() >------------
            pnlPath.Background = new SolidColorBrush(Color.FromRgb(255, 192, 203));
            //------------</ StackPanel_MouseDown() >------------
        }
    }
}
