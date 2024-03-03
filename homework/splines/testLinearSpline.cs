/* Program test LinearSpline class by producing data for later plotting. */

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
        Vector xValues = inputData[0]; // xValues in range [-2, 2].
        Vector yValues = inputData[1]; // f(x) = 4 - x^2.

        // Create data to be uploaded.
        WriteLine("#xValues,yValues,integral");
        LinearSpline linearSpline = new LinearSpline(xValues, yValues);
        int numPoints = 100;
        double[] xRange = new double[] {-2, 2};
        double dx = (xRange[1] - xRange[0]) / (numPoints - 1);
        for (int i = 0; i < numPoints; i++)
        {
            double xValueCurrent = xRange[0] + dx * i;
            WriteLine($"{xValueCurrent},{linearSpline.Interpolate(xValueCurrent)},{linearSpline.Integrate(xValueCurrent)}");
        }
    }
}