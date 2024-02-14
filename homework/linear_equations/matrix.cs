using System;
using static System.Console;
using static System.Math;

public class Matrix
{
	// Fields

	private double[] _data;
	private readonly int _numRows, _numCols;

	// Constructors

	public Matrix(int m, int n)
	{
		_numRows = m;
		_numCols = n;
		_data = new double[_numRows * _numCols];
	}

	public Matrix(int n) : this(n, n) {}

	// Methods

	public double this[int i, int j]
	{
		get {return _data[i + j * _numRows];}
		set {_data[i + j * _numRows] = value;}
	}

	public int NumRows => _numRows;

	public int NumCols => _numCols;

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
}
