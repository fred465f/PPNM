/* LinearAlgebra namespace contains several linear algebra features,
such as matrices, vectors and classes to solve linear algebra problems
using these, such as QR-decomposition using Gram-Schmidt orthogonalization,
and eigenvalue-decomposition using Jacobi rotations and several more, described
in greater detail below. */

using System;
using static System.Console;
using static System.Math;

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

    /* Class Vector contains the implementation of real vectors with entries 
    being doubles. Basic arithmetic operations has been implemented. Further 
    methods enabling easy vector computations has also been implemented. */
    public class Vector
    {    
        // Fields.
		protected double[] _data;
		protected readonly int _numRows, _numCols;

        // Constructors.
        public Vector(int m, int n)
        {
            if (!(n == 1 || m == 1))
            {
                throw new ArgumentException("A vector must either have one column or one row", $"Rows: {n}, Columns: {m}");
            }
            else
            {
                _numRows = m;
			    _numCols = n;
			    _data = new double[m * n];
            }
        }
        public Vector(int n) : this(n, 1) {}
		public Vector(string data)
		{
			var rows = data.Split("\n");
			int m = rows.Length, n = rows[0].Split(",").Length;
			_numRows = m;
			_numCols = n;
			_data = new double[m * n];
			for (int i = 0; i < m; i++)
			{
				var entries = rows[i].Split(",");
				for (int j = 0; j < n; j++)
				{
					try
					{
						_data[i + j * m] = double.Parse(entries[j]);
					}
					catch (InvalidCastException e)
					{
						throw new InvalidCastException("Wrong type of input string", e);
					}
				}
			}
		}

        // Indexing methods.
        public double this[int i]
        {
            get {return _data[i];}
            set {_data[i] = value;}
        }
        public double this[int i, int j]
		{
			get {return _data[i + j * _numRows];}
			set {_data[i + j * _numRows] = value;}
		}

        // Dimensional properties.
        public int NumRows => _numRows;
		public int NumCols => _numCols;
        public int Length => Max(_numRows, _numCols);

        /* Takes strings of the form "a\nb\nc\n..." for column vectors and 
        "a,b,c,..." for row vectors. Notation is equivalent to method used
        for matrices. */
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

        // Terminal visualization of vector data.
        public void PrintVector()
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

        /* Methods for comparison of doubles and vectors within certain
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
		public static bool Approx(Vector v, Vector w)
		{
			if ((v.NumRows != w.NumRows) && (v.NumCols != w.NumCols))
			{
				throw new ArgumentException("Input vectors were of different size", $"({v.NumRows}, {v.NumCols}) and ({w.NumRows}, {w.NumCols})");
			}
			else
			{
				for (int i = 0; i < v.NumRows; i++)
				{
					for (int j = 0; j < v.NumCols; j++)
					{
						if (!Approx(v[i, j], w[i, j]))
							return false;
					}
				}
				return true;
			}
		}

        // Overloading arithmetic operators.
        public static Vector operator +(Vector v, Vector w)
        {
            if ((v.NumRows != w.NumRows) && (v.NumCols != w.NumCols))
			{
				throw new ArgumentException("Input vectors were of different size", $"({v.NumRows}, {v.NumCols}) and ({w.NumRows}, {w.NumCols})");
			}
			else
			{
				Vector u = new Vector(v.NumRows, v.NumCols);
				for (int i = 0; i < u.Length; i++)
				{
					u[i] = v[i] + w[i];
				}
				return u;
			}
        }
        public static Vector operator -(Vector v)
        {
            Vector u = new Vector(v.NumRows, v.NumCols);
            for (int i = 0; i < u.Length; i++)
            {
                u[i] = -v[i];
            }
            return u;
        }
        public static Vector operator -(Vector v, Vector w)
        {
            if ((v.NumRows != w.NumRows) && (v.NumCols != w.NumCols))
			{
				throw new ArgumentException("Input vectors were of different size", $"({v.NumRows}, {v.NumCols}) and ({w.NumRows}, {w.NumCols})");
			}
			else
			{
				Vector u = new Vector(v.NumRows, v.NumCols);
                u = v + (-w);
                return u;
			}
        }
        public static Vector operator *(Vector v, double x)
		{
			Vector u = new Vector(v.NumRows, v.NumCols);
			for (int i = 0; i < v.NumRows; i++)
			{
				for (int j = 0; j < v.NumCols; j++)
				{
					u[i, j] = v[i, j] * x;
				}
			}
			return u;
		}
		public static Vector operator *(double x, Vector v) => v * x;
		public static Vector operator /(Vector v, double x)
		{
			if (Approx(x, 0.0))
			{
				throw new DivideByZeroException();
			}
			else
			{
				return v * (1/x);
			}
		}

        // Transposition of vectors.
        public static Vector Transpose(Vector v)
		{
			Vector u = new Vector(v.NumCols, v.NumRows);
            for (int i = 0; i < u.Length; i++)
            {
                u[i] = v[i];
            }
            return u;
		}
		public Vector T() => Transpose(this);

        // Method for copying data from vector.
        public Vector Copy()
		{
			Vector u = new Vector(this.NumRows, this.NumCols);
			for (int i = 0; i < u.Length; i++)
            {
                u[i] = this[i];
            }
			return u;
		}

        // Inner product and norm.
        public static double InnerProduct(Vector v, Vector w)
        {
            if ((v.NumRows != w.NumRows) && (v.NumCols != w.NumCols))
			{
				throw new ArgumentException("Input vectors were of different size", $"({v.NumRows}, {v.NumCols}) and ({w.NumRows}, {w.NumCols})");
			}
            else
            {
                double innerProduct = 0;
                for (int i = 0; i < v.Length; i++)
                {
                    innerProduct += v[i] * w[i];
                }
                return innerProduct;
            }
        }
        public static double Norm(Vector v) => Sqrt(InnerProduct(v, v));

		// Method to apply transformation to all entries of vector using input delegate.
		public Vector Apply(Func<double, double> transformation)
		{
			Vector transformedVector = new Vector(this.Length);
			for (int i = 0; i < this.Length; i++)
			{
				transformedVector[i] = transformation(this[i]);
			}
			return transformedVector;
		}
    }

    /* Class QRGS in solves linear equations, computes determinants and inverses 
    using the QR-decomp. of a matrix given doing construction of class instance. 
    QR-decomp. is computed using modified Gram-Schmidt orthogonalization. */
    public class QRGS
    {
        // Fields
        public Matrix Q, R; 
        public bool isSquare;
        public int inputNumRows;
        public int inputNumCols;

        /* Constructor - it computes QR-decomp. of input matrix doing
        creation of new instance of QRGS. */
        public QRGS(Matrix A)
        {
            if (A.NumRows < A.NumCols)
            {
                throw new ArgumentException("Input matrix should either be tall or square for algorithms to work", $"({A.NumRows}, {A.NumCols})");
            }
            else
            {
                Q = A.Copy();
                R = new Matrix(A.NumCols, A.NumCols);
                for (int i = 0; i < A.NumCols; i++)
                {
                    R[i, i] = Vector.Norm(Q[i]);
                    Q[i] /= R[i, i];
                    for (int j = i + 1; j < A.NumCols; j++)
                    {
                        R[i, j] = Vector.InnerProduct(Q[i], Q[j]);
                        Q[j] -= Q[i] * R[i, j];
                    }
                }
                if (A.NumRows == A.NumCols)
                {
                    isSquare = true;
                }
                else
                {
                    isSquare = false;
                }
                inputNumRows = A.NumRows;
                inputNumCols = A.NumCols;
            }
        }

        /* Method solves A * x = b using decomp. A = Q * R, by rewriting 
        R * x = Q^T * b and then using back substitution. */
        public Vector SolveLinearEq(Vector b)
        {
            Vector x = Q.T() * b;
            for (int i = x.Length - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < x.Length; j++)
                {
                    sum += R[i, j] * x[j];
                }
                x[i] = (x[i] - sum) / R[i, i];
            }
            return x;
        }

        /* Method computes det. of A using decomp A = Q * R as product of 
        diagonal entries of R. Throws an exception if A is not a square matrix. */
        public double Determinant()
        {
            if (!isSquare)
            {
                throw new ArgumentException("The determinant is only defined for square matrices", $"({inputNumRows}, {inputNumCols})");
            }
            else
            {
                double determinant = 1;
                for (int i = 0; i < R.NumRows; i++)
                {
                    for (int j = 0; j < R.NumCols; j++)
                    {
                        if (i == j)
                        {
                            determinant *= R[i, j];
                        }
                    }
                }
                return determinant;
            }
        }

        // Method computes inverse of A. Throws an exception if det(A) = 0.
        public Matrix Inverse()
        {
            double determinant = this.Determinant();
            if (!isSquare || Matrix.Approx(determinant, 0))
            {
                throw new ArgumentException("To be invertible a matrix must both be a square matrix and have non-trivial determinant", $"Square = {isSquare}, Determinant = {determinant}");
            }
            else
            {
                Matrix B = new Matrix(inputNumRows, inputNumCols);
                for (int i = 0; i < inputNumCols; i++)
                {
                    Vector ei = new Vector(inputNumRows);
                    for (int j = 0; j < inputNumRows; j++)
                    {
                        if (i == j)
                        {
                            ei[j] = 1.0;
                        }
                        else
                        {
                            ei[j] = 0.0;
                        }
                    }
                    Vector bi = SolveLinearEq(ei);
                    B[i] = bi;
                }
                return B;
            }
        }

        // Method computes pseudo-inverse of tall matrix A.
        public Matrix PseudoInverse()
        {
            Matrix B = new Matrix(inputNumCols, inputNumRows);
            for (int i = 0; i < inputNumCols; i++)
            {
                Vector ei = new Vector(inputNumRows);
                for (int j = 0; j < inputNumRows; j++)
                {
                    if (i == j)
                    {
                        ei[j] = 1.0;
                    }
                    else
                    {
                        ei[j] = 0.0;
                    }
                }
                Vector bi = SolveLinearEq(ei);
                B[i] = bi;
            }
            return B;
        }
    }

    /* Class EVD contains implementation of eigenvalue-decomposition of real symmetric 
    matrices using the Jacobi eigenvalue algorithm, with cyclic sweeps. */
    public class EVD
    {
        // ----- Fields -----
        public Vector w;
        public Matrix V;
        public double absoluteError = 1e-15, relativeError = 1e-15;

        // ----- Constructor -----
        public EVD(Matrix M)
        {
            if (M.NumRows != M.NumCols || !Matrix.Approx(M, M.T()))
            {
                throw new ArgumentException("Input matrix should both be square and symmetric for Jacobi eigenvalue algorithm to work");
            }
            {
                Matrix A = M.Copy();
                V = Matrix.Identity(M.NumRows);
                w = new Vector(M.NumRows);
                // Run Jacobi rotations on A and update V.
                // Copy diagonal elements into w.
                bool changed;
                do 
                {
                    changed = false;
                    for (int p = 0; p < A.NumRows - 1; p++)
                    {
                        for (int q = p + 1; q < A.NumRows; q++)
                        {
                            double Apq = A[p, q], App = A[p, p], Aqq = A[q, q];
                            double theta = 0.5 * Atan2(2 * Apq, Aqq - App);
                            double s = Sin(theta), c = Cos(theta);
                            double updatedApp = c * c * App - 2 * s * c * Apq + s * s * Aqq;
                            double updatedAqq = s * s * App + 2 * s * c * Apq + c * c * Aqq;
                            if (!Matrix.Approx(updatedApp, App, absoluteError, relativeError) || !Matrix.Approx(updatedAqq, Aqq, absoluteError, relativeError))
                            {
                                changed = true;
                                TimesJ(A, p, q, theta);
                                JTimes(A, p, q, -theta);
                                TimesJ(V, p, q, theta);
                            }
                        }
                    }
                    for (int i = 0; i < A.NumRows; i++)
                    {
                        w[i] = A[i, i];
                    }
                } while (changed);
            }
        }

        // ----- Jacobi rotation methods -----
        public static void TimesJ(Matrix A, int p, int q, double theta) 
        {
            double s = Sin(theta), c = Cos(theta);
            for (int i = 0; i < A.NumRows; i++)
            {
                double Aip = A[i, p], Aiq = A[i, q];
                A[i, p] = c * Aip - s * Aiq;
                A[i, q] = s * Aip + c * Aiq;
            }
        }
        public static void JTimes(Matrix A, int p, int q, double theta) 
        {
            double s = Sin(theta), c = Cos(theta);
            for (int i = 0; i < A.NumRows; i++)
            {
                double Api = A[p, i], Aqi = A[q, i];
                A[p, i] = c * Api + s * Aqi;
                A[q, i] = -s * Api + c * Aqi;
            }
        }
    }

    /* Class utilizes QR-decomposition to implement least squares method for fitting */
    public static class LeastSquares
    {
        public static (Vector coefficients, Matrix covarianceMatrix) Fit(Func<double, double>[] functions, Vector xValues, Vector yValues, Vector yErrors)
        {
            /* Problem is transformed into linear eq. A * c = b, the number of columns (m) in A
            is equal to the number of functions in linear combination, i.e functions.Length,
            and number of rows (n) equal to the number of data points, i.e xValues.Length. */
            int n = xValues.Length;
            int m = functions.Length;

            // Functions coefficients in least squares fit to be returned.
            Vector coefficients = new Vector(functions.Length);

            /* Covariance matrix to be returned.
            Using QR-decomposition we write A = Q * R with R square matrix of size (m, m).
            Now the covariance matrix is computed as (R^T * R)^(-1), so must have equal size to R. 
            Can equivalently be computed as A^(-1) * A^(-1)^T, with A^(-1) the pseudo-inverse of A. */
            Matrix covarianceMatrix = new Matrix(functions.Length);

            // Throw exception if y-errors are zero.
            for (int i = 0; i < n; i++)
            {
                if (Matrix.Approx(yErrors[i], 0))
                {
                    throw new ArgumentException("Uncertainties needs to be none-zero to avoid division by zero exception.", $"Zero uncertainty found at index {i}.");
                }
            }

            // Construct appropriate vector and matrix for solving least-squares problem.
            Matrix A = new Matrix(n, m);
            Vector b = new Vector(n);
            for (int i = 0; i < n; i++)
            {
                b[i] = yValues[i] / yErrors[i];
                for (int j = 0; j < m; j++)
                {
                    A[i, j] = functions[j](xValues[i]) / yErrors[i];
                }
            }

            // Do QR-decomposition.
            QRGS qrgs = new QRGS(A);

            // Solve linear equation A * c = b for coefficients c.
            coefficients = qrgs.SolveLinearEq(b);
            
            // Get covariance matrix.
            Matrix pseudoInverseA = qrgs.PseudoInverse();
            covarianceMatrix = pseudoInverseA * pseudoInverseA.T();

            // Return results.
            return (coefficients, covarianceMatrix);
        }
    }
}