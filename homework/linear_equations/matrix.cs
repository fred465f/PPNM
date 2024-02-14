using System;
using static System.Math;

public class Matrix<T>
{
	// Fields
	private T[][] _data;
	private readonly int _numRows, _numCols;

	// Constructor
	public Matrix(int m, int n)
	{
		_numRows = m;
		_numCols = n;
		_data = new T[_numRows][];
		for (int i = 0; i < _numRows; i++)
		{
			_data[i] = new T[_numCols];
		}
	}
	public Matrix(int n) : this(n, n) {}

	// Methods
	public T this[int i, int j]
	{
		get {return _data[i][j];}
		set {_data[i][j] = value;}
	}
}
