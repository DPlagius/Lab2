using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Matrix<double> matrixA;
        private Matrix<double> matrixB;
        private Matrix<double> matrixC;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void CreateMatrices_Click(object sender, RoutedEventArgs e)
        {
            int m1 = int.Parse(RowsMatrixATextBox.Text);
            int n1 = int.Parse(ColsMatrixATextBox.Text);
            int m2 = int.Parse(RowsMatrixBTextBox.Text);
            int n2 = int.Parse(ColsMatrixBTextBox.Text);

            matrixA = new Matrix<double>(m1, n1);
            matrixB = new Matrix<double>(m2, n2);

            DisplayMatrix(matrixA, MatrixAControl);
            DisplayMatrix(matrixB, MatrixBControl);
        }

        private void GenerateRandomValues_Click(object sender, RoutedEventArgs e)
        {
            if (matrixA == null || matrixB == null)
            {
                MessageBox.Show("Сначала создайте матрицы.");
                return;
            }

            Random rand = new Random();
            matrixA.Generate((i, j) => rand.NextDouble() * 10);
            matrixB.Generate((i, j) => rand.NextDouble() * 10);

            DisplayMatrix(matrixA, MatrixAControl);
            DisplayMatrix(matrixB, MatrixBControl);
        }

        private void DisplayMatrix(Matrix<double> matrix, ItemsControl control)
        {
            control.Items.Clear();
            var grid = new Grid();

            for (int i = 0; i < matrix.rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }

            for (int j = 0; j < matrix.columns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < matrix.rows; i++)
            {
                for (int j = 0; j < matrix.columns; j++)
                {
                    var textBox = new TextBox
                    {
                        Text = matrix[i, j].ToString("F2"),
                        Margin = new Thickness(2),
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center
                    };
                    Grid.SetRow(textBox, i);
                    Grid.SetColumn(textBox, j);
                    grid.Children.Add(textBox);
                }
            }
            control.Items.Add(grid);
        }

        private void UpdateMatrix(Matrix<double> matrix, ItemsControl control)
        {
            if (control.Items.Count == 0)
                return;

            var grid = control.Items[0] as Grid;
            for (int i = 0; i < matrix.rows; i++)
            {
                for (int j = 0; j < matrix.columns; j++)
                {
                    var textBox = grid.Children[i * matrix.columns + j] as TextBox;
                    string input = textBox.Text.Trim();

                    if (double.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out double value))
                    {
                        matrix[i, j] = value;
                    }
                    else
                    {
                        MessageBox.Show("Введите корректные числовые значения в матрице.");
                        return;
                    }
                }
            }
        }

        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (matrixA == null || matrixB == null)
            {
                MessageBox.Show("Сначала создайте и заполните матрицы.");
                return;
            }

            UpdateMatrix(matrixA, MatrixAControl);
            UpdateMatrix(matrixB, MatrixBControl);

            if (OperationComboBox.SelectedIndex == 0)
            {
                matrixC = matrixA + matrixB;
                DisplayMatrix(matrixC, MatrixCControl);
            }
            else if (OperationComboBox.SelectedIndex == 1)
            {
                matrixC = matrixA * matrixB;
                DisplayMatrix(matrixC, MatrixCControl);
            }
        }

        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".csv",
                Filter = "CSV files (*.csv)|*.csv"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                matrixC.SaveToCsv(dlg.FileName);
                MessageBox.Show("Результат сохранен!");
            }
        }
    }
}