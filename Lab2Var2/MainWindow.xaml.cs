using System;
using System.Windows;
using System.Windows.Input;

namespace Lab2Var2
{
    public partial class MainWindow : Window
    {
        public ViewData ViewModel { get; set; }
        public bool IsMeasured { get; set; }
        public bool IsSplined { get; set; }

        public MainWindow()
        {
            try
            {
                ViewModel = new();

                DataContext = this;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            InitializeComponent();
            Func.ItemsSource = Enum.GetValues(typeof(LogicLibrary.SPf));
        }

        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !ViewModel.Params.Error1;
        }
        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // Clear some text
                ViewModel.TextBlocks.SetDefaults();

                ViewModel.UpdateData();
                ViewModel.Data.Measures.CreateGrid();
                ViewModel.Data.Measures.MeasureValues(ViewModel.ListOutput);

                IsMeasured = true;
                IsSplined = false;

                ViewModel.Plot.Series.Clear();
                ViewModel.Plot.AddToSeries(ViewModel.Data.Measures.Grid, ViewModel.Data.Measures.Values, "Функция", 0);
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (!ViewModel.Params.Error2) && IsMeasured && (!IsSplined);
        }

        private void Splines_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                IsSplined = true;

                double error_val = ViewModel.Interpolate();
                if (error_val == 0)
                {
                    ViewModel.TextBlocks.TextBlock_Der_1rst_l = ViewModel.Data.Derivatieves[0];
                    ViewModel.TextBlocks.TextBlock_Der_1rst_r = ViewModel.Data.Derivatieves[1];
                    ViewModel.TextBlocks.TextBlock_Der_2nd_l = ViewModel.Data.Derivatieves[2];
                    ViewModel.TextBlocks.TextBlock_Der_2nd_r = ViewModel.Data.Derivatieves[3];
                    ViewModel.TextBlocks.TextBlock_Spl1 = ViewModel.Data.Splines[0];
                    ViewModel.TextBlocks.TextBlock_Spl2 = ViewModel.Data.Splines[ViewModel.Data.Measures.Length - 1];

                    double[] GridUniform = new double[ViewModel.Data.Params.LengthUni];
                    double step = (ViewModel.Data.Measures.Right - ViewModel.Data.Measures.Left) / (ViewModel.Data.Params.LengthUni - 1);
                    for (int i = 0; i < ViewModel.Data.Params.LengthUni; i++)
                    {
                        GridUniform[i] = ViewModel.Data.Measures.Left + (i * step);
                    }

                    ViewModel.Plot.AddToSeries(GridUniform, ViewModel.Data.Splines, "Сплайн", 1);

                    error_val = ViewModel.Integrate();
                    if (error_val == 0)
                    {
                        ViewModel.TextBlocks.TextBlock_Integ1 = ViewModel.Data.Integrals[0];
                        ViewModel.TextBlocks.TextBlock_Integ2 = ViewModel.Data.Integrals[1];
                    }
                    else
                    {
                        MessageBox.Show($"Error in Integration: {error_val}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Error in Interpolation: {error_val}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public static class Commands
    {
        public static readonly RoutedUICommand MeasuredData = new
            (
                "MeasuredData",
                "MeasuredData",
                typeof(Commands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Splines = new
            (
                "Splines",
                "Splines",
                typeof(Commands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );
    }
}
