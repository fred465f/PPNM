using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
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

		// Static method returns a random vector.
		public static Vector RandomVector(int size, int seed = 1)
		{
			var rnd = new Random(seed);
			Vector u = new Vector(size);
			for (int i = 0; i < size; i++)
			{
				u[i] = rnd.NextDouble();
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

		// Outer product.
		public static Matrix OuterProduct(Vector v, Vector w)
		{
			if (v.NumCols != w.NumCols)
			{
				throw new ArgumentException("Input vectors needs to be inputted both as column vectors", $"({v.NumRows}, {v.NumCols}) and ({w.NumRows}, {w.NumCols})");
			}
			else
			{
				Matrix outerProduct = new Matrix(w.Length, v.Length);
				for (int i = 0; i < w.Length; i++)
				{
					for (int j = 0; j < v.Length; j++)
					{
						outerProduct[i, j] = v[j] * w[i];
					}
				}
				return outerProduct;
			}
		}

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
}