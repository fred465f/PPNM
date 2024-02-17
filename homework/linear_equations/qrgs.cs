using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

namespace LinearAlgebra
{
    public class QRGS
    {
        // Fields

        public Matrix Q, R; 

        // Constructor

        public QRGS(Matrix a)
        {
            Q = a.Copy();
            R = new Matrix(a.NumRows, a.NumCols);
            // Orthogonalize Q and fill in R ...
        }

        // Methods

        public Vector SolveLinearEq(Vector b)
        {  
            // Solve A * x = b ...
            return b;
        }

        public double Determinant()
        {
            // Use QR decomp to compute determinant ...
            return 0.0;
        }

        public Matrix Inverse()
        {
            // Use QR decomp to compute inverse ...
            return this.Q;
        }
    }
}