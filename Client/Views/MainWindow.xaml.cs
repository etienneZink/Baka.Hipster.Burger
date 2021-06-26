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

        //------------< StackPanel_MouseEnter() >------------
        private void homeme(object sender, MouseEventArgs e)
        {
            homebtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void customerme(object sender, MouseEventArgs e)
        {
            customerbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void orderme(object sender, MouseEventArgs e)
        {
            orderbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void bestsellerme(object sender, MouseEventArgs e)
        {
            bestsellerbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void rankingme(object sender, MouseEventArgs e)
        {
            rankingbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void logoutme(object sender, MouseEventArgs e)
        {
            logoutbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void articleme(object sender, MouseEventArgs e)
        {
            articlebtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void employeeme(object sender, MouseEventArgs e)
        {
            employeebtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void areame(object sender, MouseEventArgs e)
        {
            areabtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }

        private void userme(object sender, MouseEventArgs e)
        {
            userbtn.Background = new SolidColorBrush(Color.FromRgb(217, 68, 72));
        }
        //------------</ StackPanel_MouseEnter() >------------


        //------------< StackPanel_MouseLeave() >------------
        private void homeml(object sender, MouseEventArgs e)
        {
            homebtn.Background = null;
        }

        private void customerml(object sender, MouseEventArgs e)
        {
            customerbtn.Background = null;
        }

        private void orderml(object sender, MouseEventArgs e)
        {
            orderbtn.Background = null;
        }

        private void bestsellerml(object sender, MouseEventArgs e)
        {
            bestsellerbtn.Background = null;
        }

        private void rankingml(object sender, MouseEventArgs e)
        {
            rankingbtn.Background = null;
        }

        private void logoutml(object sender, MouseEventArgs e)
        {
            logoutbtn.Background = null;
        }

        private void articleml(object sender, MouseEventArgs e)
        {
            articlebtn.Background = null;
        }

        private void employeeml(object sender, MouseEventArgs e)
        {
            employeebtn.Background = null;
        }

        private void areaml(object sender, MouseEventArgs e)
        {
            areabtn.Background = null;
        }

        private void userml(object sender, MouseEventArgs e)
        {
            userbtn.Background = null;
        }
        //------------</ StackPanel_MouseLeave() >------------


        //------------< StackPanel_MouseDown() >------------
        private void homemd(object sender, MouseButtonEventArgs e)
        {
            homebtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void customermd(object sender, MouseButtonEventArgs e)
        {
            customerbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void ordermd(object sender, MouseButtonEventArgs e)
        {
            orderbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void bestsellermd(object sender, MouseButtonEventArgs e)
        {
            bestsellerbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void rankingmd(object sender, MouseButtonEventArgs e)
        {
            rankingbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void logoutmd(object sender, MouseButtonEventArgs e)
        {
            logoutbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void articlemd(object sender, MouseButtonEventArgs e)
        {
            articlebtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void employeemd(object sender, MouseButtonEventArgs e)
        {
            employeebtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void areamd(object sender, MouseButtonEventArgs e)
        {
            areabtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }

        private void usermd(object sender, MouseButtonEventArgs e)
        {
            userbtn.Background = new SolidColorBrush(Color.FromRgb(255, 0, 10));
        }
        //------------</ StackPanel_MouseDown() >------------
    }
}