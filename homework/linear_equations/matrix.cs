/* Class Matrix in namespace LinearAlgebra contains the implementation
of real matrices with entries being doubles. Basic arithemtic operations
has been implemented, as well as matrix/vector multiplication. Further
methods enabling easy matrix/vector computations has also been implemented. */

using System;
using LinearAlgebra;
using static System.Console;
using static System.Math;

namespace LinearAlgebra
{
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
						throw new InvalidCastException("Wrong type of inputstring", e);
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
		public static bool Approx(Matrix a, Matrix b)
		{
			if ((a.NumRows != b.NumRows) && (a.NumCols != b.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({a.NumRows}, {a.NumCols}) and ({b.NumRows}, {b.NumCols})");
			}
			else
			{
				for (int i = 0; i < a.NumRows; i++)
				{
					for (int j = 0; j < a.NumCols; j++)
					{
						if (!Approx(a[i, j], b[i, j]))
							return false;
					}
				}
				return true;
			}
		}

		// Overloading arithmetic operators.
		public static Matrix operator +(Matrix a, Matrix b)
		{
			if ((a.NumRows != b.NumRows) && (a.NumCols != b.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({a.NumRows}, {a.NumCols}) and ({b.NumRows}, {b.NumCols})");
			}
			else
			{
				int numRows = a.NumRows, numCols = a.NumCols;
				Matrix c = new Matrix(numRows, numCols);
				for (int i = 0; i < numRows; i++)
				{
					for (int j = 0; j < numCols; j++)
					{
						c[i, j] = a[i, j] + b[i, j];
					}
				}
				return c;
			}
		}
		public static Matrix operator -(Matrix a)
		{
			int numRows = a.NumRows, numCols = a.NumCols;
			Matrix c = new Matrix(numRows, numCols);
			for (int i = 0; i < numRows; i++)
			{
				for (int j = 0; j < numCols; j++)
				{
					c[i, j] = -a[i, j];
				}
			}
			return c;
		}
		public static Matrix operator -(Matrix a, Matrix b)
		{
			if ((a.NumRows != b.NumRows) && (a.NumCols != b.NumCols))
			{
				throw new ArgumentException("Input matrices were of different size", $"({a.NumRows}, {a.NumCols}) and ({b.NumRows}, {b.NumCols})");
			}
			else
			{
				int numRows = a.NumRows, numCols = a.NumCols;
				Matrix c = new Matrix(numRows, numCols);
				for (int i = 0; i < numRows; i++)
				{
					for (int j = 0; j < numRows; j++)
					{
						c[i, j] = a[i, j] - b[i, j];
					}
				}
				return c;
			}
		}
		public static Matrix operator *(Matrix a, double x)
		{
			Matrix c = new Matrix(a.NumRows, a.NumCols);
			for (int i = 0; i < a.NumRows; i++)
			{
				for (int j = 0; j < a.NumCols; j++)
				{
					c[i, j] = a[i, j] * x;
				}
			}
			return c;
		}
		public static Matrix operator *(double x, Matrix a) => a * x;
		public static Matrix operator *(Matrix a, Matrix b)
		{
			if (a.NumCols != b.NumRows)
			{
				throw new ArgumentException("Size of input matrices were inappropriate for matrix multiplication", $"({a.NumRows}, {a.NumCols}) and ({b.NumRows}, {b.NumCols})");
			}
			else
			{
				Matrix c = new Matrix(a.NumRows, b.NumCols);
				for (int i = 0; i < a.NumRows; i++)
				{
					for (int j = 0; j < b.NumCols; j++)
					{
						double entry = 0;
						for (int k = 0; k < a.NumCols; k++)
						{
							entry += a[i, k] * b[k, j];
						}
						c[i, j] = entry;
					}
				}
				return c;
			}
		}
		public static Matrix operator /(Matrix a, double x)
		{
			if (Approx(x, 0.0))
			{
				throw new DivideByZeroException();
			}
			else
			{
				return a * (1/x);
			}
		}

		// Overloading matrix/vector multiplication.
		public static Vector operator *(Matrix a, Vector v)
        {
            if (v.NumRows != a.NumCols)
            {
                throw new ArgumentException("Size of input matrix and vector were inappropriate for matrix multiplication", $"Matrix: ({a.NumRows}, {a.NumCols}), Vector: ({v.NumRows}, {v.NumCols})");
            }
            else
            {
                Vector u = new Vector(a.NumRows);
                for (int i = 0; i < a.NumRows; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < v.Length; j++)
                    {
                        sum += a[i, j] * v[j];
                    }
                    u[i] = sum;
                }
                return u;
            }
        }
        public static Vector operator *(Vector v, Matrix a)
        {
            if (v.NumCols != a.NumRows)
            {
                throw new ArgumentException("Size of input matrix and vector were inappropriate for matrix multiplication", $"Vector: ({v.NumRows}, {v.NumCols}), Matrix: ({a.NumRows}, {a.NumCols})");
            }
            else
            {
                Vector u = new Vector(a.NumCols);
                u = u.T();
                for (int i = 0; i < a.NumCols; i++)
                {
                    double sum = 0;
                    for (int j = 0; j < v.Length; j++)
                    {
                        sum += a[j, i] * v[j];
                    }
                    u[i] = sum;
                }
                return u;
            }
        }

		// Transposition of matrices
		public static Matrix Transpose(Matrix a)
		{
			Matrix c = new Matrix(a.NumCols, a.NumRows);
			for (int i = 0; i < a.NumCols; i++)
			{
				for (int j = 0; j < a.NumRows; j++)
				{
					c[i, j] = a[j, i];
				}
			}
			return c;
		}
		public Matrix T() => Transpose(this);

		// Method for copying data from matrix
		public Matrix Copy()
		{
			Matrix c = new Matrix(this.NumRows, this.NumCols);
			for (int i = 0; i < this.NumRows; i++)
			{
				for (int j = 0; j < this.NumCols; j++)
				{
					c[i, j] = this[i, j];
				}
			}
			return c;
		}

		// Static method returning identity matrix.
		public static Matrix Identity(int n)
		{
			Matrix c = new Matrix(n);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (i == j)
					{
						c[i, j] = 1;
					}
					else
					{
						c[i, j] = 0;
					}
				}
			}
			return c;
		}

		// Static method returning zero matrix.
		public static Matrix Zero(int n)
		{
			Matrix c = new Matrix(n);
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					c[i, j] = 0;
				}
			}
			return c;
		}
	}
}
