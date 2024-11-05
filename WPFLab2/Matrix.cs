using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFLab2
{
    public class Matrix<T> where T : struct
    {
        public int rows { get; }
        public int columns { get; }
        private T[,] elements;

        public Matrix(int rows, int columns) 
        {
            if (rows <= 0 || columns <= 0)
                throw new ArgumentException("Negative demention");
            this.rows = rows;
            this.columns = columns;
            this.elements = new T[rows, columns];
        }
        
        public T this[int row, int col]
        {
            get => elements[row, col];
            set => elements[row, col] = value;
        }

        public void Generate(Func<int, int, T> generator)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    elements[i, j] = generator(i, j);
        }

        public static Matrix<T> operator +(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            if (matrix1.columns != matrix2.columns || matrix1.rows != matrix2.rows) 
                throw new ArgumentException("Dimensions of matrices must be the same for addition.");
            int rows = matrix1.rows;
            int columns = matrix1.columns;
            var result = new Matrix<T>(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] = (dynamic) matrix1[i, j] + matrix2[i, j];
            return result;
        }

        public static Matrix<T> operator *(Matrix<T> matrix1, Matrix<T> matrix2)
        {
            if (matrix1.columns != matrix2.rows)
                throw new ArgumentException("Wrong dimensions of matrices.");
            int rows = matrix1.rows;
            int sameDimention = matrix1.columns;
            int columns = matrix2.columns;
            var result = new Matrix<T>(rows, columns);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < sameDimention; j++)
                {
                    dynamic sum = default(T);
                    for (int k = 0; k < columns; k++)
                        sum += (dynamic)matrix1[i, k] * matrix2[k, j];
                    result[i, j] = sum;
                }
            return result;
        }

        public void SaveToCsv(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++)
                {
                    var row = new List<string>();
                    for (int j = 0; j < columns; j++)
                    {
                        double value = Convert.ToDouble(this[i, j]);
                        row.Add(value.ToString("F2", CultureInfo.InvariantCulture).Replace('.', ','));
                    }
                    writer.WriteLine(string.Join(";", row));
                }
            }
        }

    }
}
