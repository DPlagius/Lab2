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
using MatrixOperations;

namespace WPFLab2
{
    public partial class MainWindow : Window
    {
        private Matrix<double> matrix1;
        private Matrix<double> matrix2;
        private Matrix<double> matrix3;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateMatrices_Click(object sender, RoutedEventArgs e)
        {
            int rows1 = Convert.ToInt32(RowsMatrixATextBox.Text);
            int columns1 = Convert.ToInt32(ColsMatrixATextBox.Text);
            int rows2 = Convert.ToInt32(RowsMatrixBTextBox.Text);
            int columns2 = Convert.ToInt32(ColsMatrixBTextBox.Text);

            matrix1 = new Matrix<double>(rows1, columns1);
            matrix2 = new Matrix<double>(rows2, columns2);

            DisplayMatrix(matrix1, MatrixAControl);
            DisplayMatrix(matrix2, MatrixBControl);
        }

        private void GenerateRandomValues_Click(object sender, RoutedEventArgs e)
        {
            if (matrix1 == null || matrix2 == null)
            {
                MessageBox.Show("Сначала создайте матрицы.");
                return;
            }

            Random rand = new Random();
            matrix1.Generate((i, j) => rand.NextDouble() * 10);
            matrix2.Generate((i, j) => rand.NextDouble() * 10);

            DisplayMatrix(matrix1, MatrixAControl);
            DisplayMatrix(matrix2, MatrixBControl);
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
            if (matrix1 == null || matrix2 == null)
            {
                MessageBox.Show("Сначала создайте и заполните матрицы.");
                return;
            }

            UpdateMatrix(matrix1, MatrixAControl);
            UpdateMatrix(matrix2, MatrixBControl);

            if (OperationComboBox.SelectedIndex == 0)
            {
                matrix3 = matrix1 + matrix2;
                DisplayMatrix(matrix3, MatrixCControl);
            }
            else if (OperationComboBox.SelectedIndex == 1)
            {
                matrix3 = matrix1 * matrix2;
                DisplayMatrix(matrix3, MatrixCControl);
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
                matrix3.SaveToCsv(dlg.FileName);
                MessageBox.Show("Результат сохранен!");
            }
        }
    }
}