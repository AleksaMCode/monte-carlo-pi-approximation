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

namespace monte_carlo_pi_approximation
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

        private void firstRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            FirstNumericUpDown.Visibility = Visibility.Visible;
            SecondNumericUpDown.Visibility = Visibility.Hidden;
        }

        private void secondRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            FirstNumericUpDown.Visibility = Visibility.Hidden;
            SecondNumericUpDown.Visibility = Visibility.Visible;
        }

        private void FirstNumericUpDown_TouchEnter(object sender, TouchEventArgs e)
        {
            FirstNumericUpDown.Visibility = Visibility.Visible;

        }
    }
}
