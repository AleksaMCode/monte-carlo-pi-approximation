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
        public static int iterationNumber = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FirstRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            FirstNumericUpDown.Visibility = Visibility.Visible;
            SecondNumericUpDown.Visibility = Visibility.Hidden;
        }

        private void SecondRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            FirstNumericUpDown.Visibility = Visibility.Hidden;
            SecondNumericUpDown.Visibility = Visibility.Visible;
        }

        private async void CalculatePi(object sender, RoutedEventArgs e)
        {
            if (firstRadioButton.IsChecked.Value == true)
            {
                iterationNumber = Convert.ToInt32(FirstNumericUpDown.Value.Value);
                MonteCarloPiApproximation();
            }
            else
            {
                var t = Convert.ToInt32(SecondNumericUpDown.Value.Value);
                Task.Run(() =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() => MonteCarloPiApproximation(Floor(Math.PI, Convert.ToInt32(SecondNumericUpDown.Value.Value))));
                });
            }
        }

        private void MonteCarloPiApproximation()
        {
            var piApproximation = 0.0;
            var total = 0;
            var insideCircle = 0;
            double x, y;
            var rnd = new Random();

            while (total < iterationNumber)
            {
                x = rnd.NextDouble();
                y = rnd.NextDouble();

                if (Math.Sqrt(x * x + y * y) <= 1.0)
                {
                    insideCircle++;
                }

                IterationNumber.Content = ++total;
                CurrentPiValue.Content = piApproximation = 4 * ((double)insideCircle / (double)total);
            }

            MessageBox.Show($"{piApproximation:F8}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MonteCarloPiApproximation(double piValue)
        {
            double piApproximation;
            var total = 0;
            var insideCircle = 0;
            double x, y;
            var rnd = new Random();

            while (true)
            {
                x = rnd.NextDouble();
                y = rnd.NextDouble();

                if (Math.Sqrt(x * x + y * y) <= 1.0)
                {
                    insideCircle++;
                }

                IterationNumber.Content = ++total;
                CurrentPiValue.Content = piApproximation = 4 * ((double)insideCircle / (double)total);

                if ((piApproximation = Floor(piApproximation, Convert.ToInt32(SecondNumericUpDown.Value.Value))) == piValue)
                {
                    break;
                }
            }

            MessageBox.Show($"{piApproximation}", "Approximated Pi");
        }

        private static double Floor(double value, int decimalPlaces)
        {
            return Math.Floor(value * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
    }
}
