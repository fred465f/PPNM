using System;
using static System.Math;
using static System.Console;
using LinearAlgebra;

class Program
{
    public static void Main()
    {
        // Radioactive decay data for ThX.
        Vector time = new Vector(9);
        time.DataFromString("1\n2\n3\n4\n6\n9\n10\n13\n15"); // In units of days.
        Vector activity = new Vector(9);
        activity.DataFromString("117\n100\n88\n72\n53\n29.5\n25.2\n15.2\n11.1"); // In relative units.
        Vector activityError = new Vector(9);
        activityError.DataFromString("5\n5\n5\n4\n4\n3\n3\n2\n2");

        Matrix A = new Matrix(3, 2);
        A.DataFromString("1,0\n0,1\n0,0");
        QRGS qrgs = new QRGS(A);
        Matrix B = qrgs.PseudoInverse();
        A.PrintMatrix();
        B.PrintMatrix();
    }
}