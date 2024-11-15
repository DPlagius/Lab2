using MatrixOperations;

namespace MatrixTest
{
    [TestClass]
    public class MatrixTest
    {
        [TestMethod]
        public void Correct_Demention_In_Matrix()
        {
            Matrix<int> matrix= new Matrix<int>(5, 4);

            Assert.AreEqual(matrix.columns, 4);
            Assert.AreEqual(matrix.rows, 5);
        }

        [TestMethod]
        public void Negative_Rows_Count_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => new Matrix<int>(-5, 4));
        }

        [TestMethod]
        public void Negative_Columns_Count_Exception()
        {
            Assert.ThrowsException<ArgumentException>(() => new Matrix<int>(5, -4));
        }

        [TestMethod]
        public void Correct_Generate_Matrix()
        {
            Matrix<int> matrix = new Matrix<int>(2, 2);

            matrix.Generate((x, y) => x +  2 * y);

            Assert.AreEqual(matrix[0, 0], 0);
            Assert.AreEqual(matrix[1, 0], 1);
            Assert.AreEqual(matrix[0, 1], 2);
            Assert.AreEqual(matrix[1, 1], 3);
        }

        [TestMethod]
        public void There_Is_No_Generate_Exception()
        {
            Matrix<int> matrix = new Matrix<int>(2, 2);
            Func<int, int, int> generate = null;

            Assert.ThrowsException<ArgumentException>(() => matrix.Generate(generate));
        }

        [TestMethod]
        public void Correct_Matrix_Sum()
        {
            Matrix<int> matrix1 = new Matrix<int>(2, 2);
            Matrix<int> matrix2 = new Matrix<int>(2, 2);

            matrix1.Generate((x, y) => x + 2 * y);
            matrix2.Generate((x, y) => x + 3 * y);
            Matrix<int> matrix3 = matrix1 + matrix2;

            Assert.AreEqual(matrix3[0, 0], 0);
            Assert.AreEqual(matrix3[1, 0], 2);
            Assert.AreEqual(matrix3[0, 1], 5);
            Assert.AreEqual(matrix3[1, 1], 7);
        }

        [TestMethod]
        public void Different_Matrixs_Rows_Demention_In_Sum_Exception()
        {
            Matrix<int> matrix1 = new Matrix<int>(3, 2);
            Matrix<int> matrix2 = new Matrix<int>(2, 2);

            matrix1.Generate((x, y) => x + 2 * y);
            matrix2.Generate((x, y) => x + 3 * y);

            Assert.ThrowsException<ArgumentException>(() => matrix1 + matrix2);
        }

        [TestMethod]
        public void Different_Matrixs_Columns_Demention_In_Sum_Exception()
        {
            Matrix<int> matrix1 = new Matrix<int>(2, 3);
            Matrix<int> matrix2 = new Matrix<int>(2, 2);

            matrix1.Generate((x, y) => x + 2 * y);
            matrix2.Generate((x, y) => x + 3 * y);

            Assert.ThrowsException<ArgumentException>(() => matrix1 + matrix2);
        }

        [TestMethod]
        public void Correct_Matrix_Multiply()
        {
            Matrix<int> matrix1 = new Matrix<int>(2, 2);
            Matrix<int> matrix2 = new Matrix<int>(2, 2);

            matrix1.Generate((x, y) => x + 2 * y);
            matrix2.Generate((x, y) => x + 3 * y);
            Matrix<int> matrix3 = matrix1 * matrix2;

            Assert.AreEqual(matrix3[0, 0], 2);
            Assert.AreEqual(matrix3[1, 0], 3);
            Assert.AreEqual(matrix3[0, 1], 8);
            Assert.AreEqual(matrix3[1, 1], 15);
        }

        [TestMethod]
        public void Different_Matrixs_Demention_In_Multiply_Exception()
        {
            Matrix<int> matrix1 = new Matrix<int>(2, 3);
            Matrix<int> matrix2 = new Matrix<int>(2, 2);

            matrix1.Generate((x, y) => x + 2 * y);
            matrix2.Generate((x, y) => x + 3 * y);

            Assert.ThrowsException<ArgumentException>(() => matrix1 * matrix2);
        }
    }
}
