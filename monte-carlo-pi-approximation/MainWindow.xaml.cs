using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        public string GraphTitle { get; private set; } = "Scaled Pi Approximation Graph";

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            lsCircle.ItemsSource = MonteCarloPiApproxGenerator.GenerateQuarterCircleUpperRightQuadrant();
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

        private void UpdateCoordinateSystem(MonteCarloPiApproxGenerator simulator)
        {
            lsPointInCircle.ItemsSource = null;
            lsPointNotInCircle.ItemsSource = null;
            lsPointInCircle.ItemsSource = simulator.PointsInCircle;
            lsPointNotInCircle.ItemsSource = simulator.PointsNotInCircle;
        }

        private void UpdateWindowText(MonteCarloPiApproxGenerator simulator)
        {
            IterationNumber.Content = simulator.NumberOfPoints;
            CurrentPiValue.Content = simulator.CalculatePiValue();
        }

        private void MonteCarloPiApproximation(CancellationToken token)
        {
            var simulator = new MonteCarloPiApproxGenerator();

            while (simulator.NumberOfPoints < iterationNumber)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                simulator.GeneratePoint();
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (simulator.NumberOfPoints % 1000 == 0)
                    {
                        UpdateCoordinateSystem(simulator);
                    }
                    if (simulator.NumberOfPoints % 10 == 0)
                    {
                        UpdateWindowText(simulator);
                    }
                });
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateCoordinateSystem(simulator);
                UpdateWindowText(simulator);
            });

            MessageBox.Show($"π≈{simulator.CalculatePiValue():F8}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MonteCarloPiApproximation(double piValue, int decimalPlaces, CancellationToken token)
        {
            var simulator = new MonteCarloPiApproxGenerator();

            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                simulator.GeneratePoint();
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (simulator.NumberOfPoints % 1000 == 0)
                    {
                        UpdateCoordinateSystem(simulator);
                    }
                    if (simulator.NumberOfPoints % 10 == 0)
                    {
                        UpdateWindowText(simulator);
                    }
                });

                if (simulator.CalculatePiValue(decimalPlaces) == piValue)
                {
                    break;
                }
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateCoordinateSystem(simulator);
                UpdateWindowText(simulator);
            });

            MessageBox.Show($"π≈{simulator.CalculatePiValue(decimalPlaces)}", "Approximated Pi", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
