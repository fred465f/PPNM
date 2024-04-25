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
        string outFile = "";

        // Parse command-line inputs.
        foreach (var arg in args)
        {
            var words = arg.Split(":");
            if (words[0] == "-outFile")
            {
                outFile = words[1];
            }
        }

        // Create random data set.
        int numRandomPoints = 10;
        Vector xValues = Vector.RandomVector(numRandomPoints);
        for (int i = 0; i < numRandomPoints; i++)
        {
            xValues[i] = i;
        }
        Vector yValues = Vector.RandomVector(numRandomPoints);

        // Create natural quadratic spline.
        QuadraticSpline quadraticSpline = new QuadraticSpline(xValues, yValues);

        // Write random data to outFile.
        using (var outStream = new System.IO.StreamWriter(outFile, append:false))
        {
            outStream.WriteLine("#xValues,yValues");
            for (int i = 0; i < numRandomPoints; i++)
            {
                outStream.WriteLine($"{xValues[i]},{yValues[i]}");
            }
        }

        // Write interpolation data to standard output stream.
        WriteLine("#xValues,yValues,integral,derivative");
        int numInterpolationPoints = 100;
        double[] xRange = new double[] {xValues[0], xValues[numRandomPoints - 1]};
        double dx = (xRange[1] - xRange[0]) / (numInterpolationPoints - 1);
        for (int i = 0; i < numInterpolationPoints; i++)
        {
            double xValueCurrent = xRange[0] + dx * i;
            WriteLine($"{xValueCurrent},{quadraticSpline.Interpolate(xValueCurrent)},{quadraticSpline.Integrate(xValueCurrent)},{quadraticSpline.Derivative(xValueCurrent)}");
        }
    }
}