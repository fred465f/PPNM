using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    public class Vector : Matrix
    {    
        // Constructors

        public Vector(int n, int m) : base(n, m) 
        {
            if (!(n == 1 || m == 1))
            {
                throw new ArgumentException("A vector must either have one column or one row", $"Rows: {n}, Columns: {m}");
            }
        }

        public Vector(int n) : base(n, 1) {}

        // Methods

        public double this[int i]
        {
            get {return _data[i];}
            set {_data[i] = value;}
        }

        public int Length => Max(_numRows, _numCols);

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

        public static Vector Transpose(Vector v)
		{
			Vector u = new Vector(v.NumCols, v.NumRows);
            for (int i = 0; i < u.Length; i++)
            {
                u[i] = v[i];
            }
            return u;
		}

		new public Vector T() => Transpose(this);

        new public Vector Copy()
		{
			Vector u = new Vector(this.NumRows, this.NumCols);
			for (int i = 0; i < u.Length; i++)
            {
                u[i] = this[i];
            }
			return u;
		}

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

        public static double Norm(Vector v) => Abs(Pow(InnerProduct(v, v), 2));
    }
}