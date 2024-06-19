using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    /* Class Matrix contains the implementation of real matrices with entries 
    being doubles. Basic arithmetic operations has been implemented, as well 
    as matrix/vector multiplication. Further methods enabling easy matrix/vector 
    computations has also been implemented. */
    public class Matrix
	{
		// Fields.
		private double[] _data;
		private readonly int _numRows, _numCols;

		// Constructors.
		public Matrix(int m, int n)
		{
			_numRows = m;
			_numCols = n;
			_data = new double[_numRows * _numCols];
		}
		public Matrix(int n) : this(n, n) {}
		public Matrix(string inputFile)
		{
			string inputData = "";
			using (var inStream = new System.IO.StreamReader(inputFile))
			{
				inputData = inStream.ReadToEnd();
			}
			var rowsData = inputData.Split("\n");
			_numRows = rowsData.Length;
			_numCols = rowsData[0].Split(",").Length;
			_data = new double[_numRows * _numCols];
			this.DataFromString(inputData);
		}

		// Indexing methods.
		public double this[int i, int j]
		{
			get {return _data[i + j * _numRows];}
			set {_data[i + j * _numRows] = value;}
		}
		public Vector this[int i] // Gets or sets the i'th column of matrix
		{
			get 
			{
				Vector u = new Vector(this.NumRows);
				for (int j = 0; j < this.NumRows; j++)
				{
					u[j] = this[j, i];
				}
				return u;
			}
			set 
			{
				if (value.Length != this.NumRows)
				{
					throw new ArgumentException("Input vector must have as many elements as matrix has rows", $"{value.Length}");
				}
				else
				{
					for (int j = 0; j < this.NumRows; j++)
					{
						this[j, i] = value[j];
					}
				}
			}
		}

		// Dimensional properties.
		public int NumRows => _numRows;
		public int NumCols => _numCols;

		// Takes a string of the form "a11,a12,...,a1k\na21,a22,...,a2k\n...\nal1,al2,...,alk".
		public void DataFromString(string data)
		{
			var rows = data.Split("\n");
			for (int i = 0; i < rows.Length; i++)
			{
				var entries = rows[i].Split(",");
				for (int j = 0; j < entries.Length; j++)
				{
					try
					{
						_data[i + j * _numRows] = double.Parse(entries[j]);
					}
					catch (InvalidCastException e)
					{
						throw new InvalidCastException("Wrong type of input string", e);
					}
				}
			}
		}

		// Terminal visualization of matrix data.
		public void PrintMatrix()
		{
			for (int i = 0; i < this.NumRows; i++)
			{
				for (int j = 0; j < this.NumCols; j++)
				{
					Write($"{_data[i + j * this.NumRows]} ");
				}
				WriteLine("");
			}
		}

		/* Methods for comparison of doubles and matrices within certain
        absolute and relative errors. */
		public static bool Approx(double x, double y, double absoluteError = 1e-9, double relativeError = 1e-9)
		{
			if (Abs(x - y) < absoluteError || Abs(x - y) < Max(Abs(x), Abs(y)) * relativeError)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static bool Approx(Matrix A, Matrix B, double absoluteError = 1e-9, double relativeError = 1e-9)
		{
			if ((A.NumRows != B.NumRows) && (A.NumCols != B.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({A.NumRows}, {A.NumCols}) and ({B.NumRows}, {B.NumCols})");
			}
			else
			{
				for (int i = 0; i < A.NumRows; i++)
				{
					for (int j = 0; j < A.NumCols; j++)
					{
						if (!Approx(A[i, j], B[i, j], absoluteError, relativeError))
							return false;
					}
				}
				return true;
			}
		}

		// Overloading arithmetic operators.
		public static Matrix operator +(Matrix A, Matrix B)
		{
			if ((A.NumRows != B.NumRows) && (A.NumCols != B.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({A.NumRows}, {A.NumCols}) and ({B.NumRows}, {B.NumCols})");
			}
			else
			{
				int numRows = A.NumRows, numCols = A.NumCols;
				Matrix C = new Matrix(numRows, numCols);
				for (int i = 0; i < numRows; i++)
				{
					for (int j = 0; j < numCols; j++)
					{
						C[i, j] = A[i, j] + B[i, j];
					}
				}
				return C;
			}
		}
		public static Matrix operator -(Matrix A)
		{
			int numRows = A.NumRows, numCols = A.NumCols;
			Matrix C = new Matrix(numRows, numCols);
			for (int i = 0; i < numRows; i++)
			{
				for (int j = 0; j < numCols; j++)
				{
					C[i, j] = -A[i, j];
				}
			}
			return C;
		}
		public static Matrix operator -(Matrix A, Matrix B)
		{
			if ((A.NumRows != B.NumRows) && (A.NumCols != B.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({A.NumRows}, {A.NumCols}) and ({B.NumRows}, {B.NumCols})");
			}
			else
			{
				int numRows = A.NumRows, numCols = A.NumCols;
				Matrix C = new Matrix(numRows, numCols);
				for (int i = 0; i < numRows; i++)
				{
					for (int j = 0; j < numRows; j++)
					{
						C[i, j] = A[i, j] - B[i, j];
					}
				}
				return C;
			}
		}
		public static Matrix operator *(Matrix A, double x)
		{
			Matrix C = new Matrix(A.NumRows, A.NumCols);
			for (int i = 0; i < A.NumRows; i++)
			{
				for (int j = 0; j < A.NumCols; j++)
				{
					C[i, j] = A[i, j] * x;
				}
			}
			return C;
		}
		public static Matrix operator *(double x, Matrix A) => A * x;
		public static Matrix operator *(Matrix A, Matrix B)
		{
			if (A.NumCols != B.NumRows)
			{
				throw new ArgumentException("Size of input matrices were inappropriate for matrix multiplication", $"({A.NumRows}, {A.NumCols}) and ({B.NumRows}, {B.NumCols})");
			}
			else
			{
				Matrix C = new Matrix(A.NumRows, B.NumCols);
				for (int i = 0; i < A.NumRows; i++)
				{
					for (int j = 0; j < B.NumCols; j++)
					{
						double entry = 0;
						for (int k = 0; k < A.NumCols; k++)
						{
							entry += A[i, k] * B[k, j];
						}
						C[i, j] = entry;
					}
				}
				return C;
			}
		}
		public static Matrix operator /(Matrix A, double x)
		{
			if (Approx(x, 0.0))
			{
				throw new DivideByZeroException();
			}
			else
			{
				return A * (1/x);
			}
		}

		// Overloading matrix/vector multiplication.
		public static Vector operator *(Matrix A, Vector v)
        {
            if (v.NumRows != A.NumCols)
            {
                throw new ArgumentException("Size of input matrix and vector were inappropriate for matrix multiplication", $"Matrix: ({A.NumRows}, {A.NumCols}), Vector: ({v.NumRows}, {v.NumCols})");
            }
            else
            {
                Vector u = new Vector(A.NumRows);
                for (int i = 0; i < A.NumRows; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < v.Length; j++)
                    {
                        sum += A[i, j] * v[j];
                    }
                    u[i] = sum;
                }
                return u;
            }
        }
        public static Vector operator *(Vector v, Matrix A)
        {
            if (v.NumCols != A.NumRows)
            {
                throw new ArgumentException("Size of input matrix and vector were inappropriate for matrix multiplication", $"Vector: ({v.NumRows}, {v.NumCols}), Matrix: ({A.NumRows}, {A.NumCols})");
            }
            else
            {
                Vector u = new Vector(A.NumCols);
                u = u.T();
                for (int i = 0; i < A.NumCols; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < v.Length; j++)
                    {
                        sum += A[j, i] * v[j];
                    }
                    u[i] = sum;
                }
                return u;
            }
        }

		// Transposition of matrices
		public static Matrix Transpose(Matrix A)
		{
			Matrix C = new Matrix(A.NumCols, A.NumRows);
			for (int i = 0; i < A.NumCols; i++)
			{
				for (int j = 0; j < A.NumRows; j++)
				{
					C[i, j] = A[j, i];
				}
			}
			return C;
		}
		public Matrix T() => Transpose(this);

		// Method for copying data from matrix
		public Matrix Copy()
		{
			Matrix C = new Matrix(this.NumRows, this.NumCols);
			for (int i = 0; i < this.NumRows; i++)
			{
				for (int j = 0; j < this.NumCols; j++)
				{
					C[i, j] = this[i, j];
				}
			}
			return C;
		}

		// Static method returning identity matrix.
		public static Matrix Identity(int n)
		{
			Matrix C = new Matrix(n);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (i == j)
					{
						C[i, j] = 1;
					}
					else
					{
						C[i, j] = 0;
					}
				}
			}
			return C;
		}

		// Static method returning zero matrix.
		public static Matrix Zero(int n)
		{
			Matrix C = new Matrix(n);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = 0;
				}
			}
			return C;
		}
		public static Matrix Zero(int m, int n)
		{
			Matrix C = new Matrix(m, n);
			for (int i = 0; i < m; i++)
			{
				for (int j = 0; j < n; j++)
				{
					C[i, j] = 0;
				}
			}
			return C;
		}

		// Method returning diagonal elements of square matrix as vector.
		public Vector Diag()
		{
			if (this.NumRows != this.NumCols)
			{
				throw new ArgumentException("To retrieve diagonal elements matrix must be a square matrix", $"({this.NumRows}, {this.NumCols})");
			}
			else
			{
				Vector u = new Vector(this.NumRows);
				for (int i = 0; i < this.NumRows; i++)
				{
					u[i] = this[i, i];
				}
				return u;
			}
		}

		// Static method constructing diagonal matrix from vector.
		public static Matrix Diag(Vector v)
		{
			Matrix C = new Matrix(v.Length);
			for (int i = 0; i < v.Length; i++)
			{
				for (int j = 0; j < v.Length; j++)
				{
					if (i == j)
					{
						C[i, i] = v[i];
					}
					else
					{
						C[i, j] = 0;
					}
				}
			}
			return C;
		}
	}
}