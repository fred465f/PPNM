/* Program test QuadraticSpline class by producing data for later plotting. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main(string[] args)
    {
        // Variables.
        string inFile = "";

        // Parse command line inputs.
        foreach (var arg in args)
        {
            var words = arg.Split(":");
            if (words[0] == "-inFile")
            {
                inFile = words[1];
            }
        }

        // Load in data to be tested.
        Matrix inputData = new Matrix(inFile);
        Vector xValues = inputData[0]; // xValues in range [?, ?].
        Vector yValues = inputData[1]; // f(x) = ?

        // Create data to be uploaded.
        WriteLine("#xValues,yValues,integral,differential");
    }
}