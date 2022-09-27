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
                task = Task.Run(() => MonteCarloPiApproximation(MonteCarloPiApproxGenerator.Floor(Math.PI, decimalPlaces), decimalPlaces, source.Token), source.Token);
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
            var simulator = new MonteCarloPiApproxGenerator();


            while (simulator.NumberOfPoints < iterationNumber)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                simulator.GeneratePoint();
                piApproximation = simulator.CalculatePiValue();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IterationNumber.Content = simulator.NumberOfPoints;
                    CurrentPiValue.Content = piApproximation;
                });
            }
            
            MessageBox.Show($"π≈{piApproximation:F8}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MonteCarloPiApproximation(double piValue, int decimalPlaces, CancellationToken token)
        {
            var piApproximation = 0.0;
            var simulator = new MonteCarloPiApproxGenerator();

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                simulator.GeneratePoint();
                piApproximation = simulator.CalculatePiValue(decimalPlaces);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    IterationNumber.Content = simulator.NumberOfPoints;
                    CurrentPiValue.Content = piApproximation;
                });

                if (piApproximation == piValue)
                {
                    break;
                }
            }

            MessageBox.Show($"π≈{piApproximation}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
