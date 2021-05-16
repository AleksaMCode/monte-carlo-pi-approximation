using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool isRunning;
        private CancellationTokenSource source = new CancellationTokenSource();
        private bool isCancelRequested;

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

        private void StopSimulation(object sender, RoutedEventArgs e)
        {
            if (!isRunning || isCancelRequested)
            {
                return;
            }

            isCancelRequested = true;
            source.Cancel();
        }

        private async void CalculatePi(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;
            Task task = null;

            if (firstRadioButton.IsChecked.Value == true)
            {
                iterationNumber = (int)FirstNumericUpDown.Value.Value;
                task = Task.Run(() => MonteCarloPiApproximation(source.Token), source.Token);
            }
            else
            {
                var decimalPlaces = (int)SecondNumericUpDown.Value.Value;
                task = Task.Run(() => MonteCarloPiApproximation(Floor(Math.PI, decimalPlaces), decimalPlaces, source.Token), source.Token);
            }

            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                source.Dispose();
                source = new CancellationTokenSource();
                isCancelRequested = false;
            }

            isRunning = false;
        }

        private void MonteCarloPiApproximation(CancellationToken token)
        {
            var piApproximation = 0.0;
            var total = 0;
            var insideCircle = 0;
            double x, y;
            var rnd = new Random();

            while (total < iterationNumber)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                x = rnd.NextDouble();
                y = rnd.NextDouble();

                if (Math.Sqrt(x * x + y * y) <= 1.0)
                {
                    insideCircle++;
                }

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IterationNumber.Content = ++total;
                    CurrentPiValue.Content = Floor(piApproximation = 4 * ((double)insideCircle / (double)total), 8);
                });
            }

            MessageBox.Show($"π≈{piApproximation:F8}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MonteCarloPiApproximation(double piValue, int decimalPlaces, CancellationToken token)
        {
            var piApproximation = 0.0;
            var total = 0;
            var insideCircle = 0;
            double x, y;
            var rnd = new Random();

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                x = rnd.NextDouble();
                y = rnd.NextDouble();

                if (Math.Sqrt(x * x + y * y) <= 1.0)
                {
                    insideCircle++;
                }

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IterationNumber.Content = ++total;
                    CurrentPiValue.Content = Floor(piApproximation = 4 * ((double)insideCircle / (double)total), 8);
                });

                if ((piApproximation = Floor(piApproximation, decimalPlaces)) == piValue)
                {
                    break;
                }
            }

            MessageBox.Show($"π≈{piApproximation}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static double Floor(double value, int decimalPlaces)
        {
            return Math.Floor(value * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces);
        }
    }
}
