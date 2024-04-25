/* Program test QuadraticSpline class by comparing numerical parameters with analytically computed ones. */

using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Create dataset 1 to be tested.
        Vector xValues1 = new Vector(5);
        Vector yValues1 = new Vector(5);
        for (int i = 0; i < 5; i++)
        {
            xValues1[i] = i;
            yValues1[i] = 1.0;
        }
        Vector b1Analytical = new Vector("0\n0\n0\n0");
        Vector c1Analytical = new Vector("0\n0\n0\n0");

        // Create dataset 2 to be tested.
        Vector xValues2 = new Vector(5);
        Vector yValues2 = new Vector(5);
        for (int i = 0; i < 5; i++)
        {
            xValues2[i] = i;
            yValues2[i] = i;
        }
        Vector b2Analytical = new Vector("1\n1\n1\n1");
        Vector c2Analytical = new Vector("0\n0\n0\n0");

        // Create dataset 3 to be tested.
        Vector xValues3 = new Vector(5);
        Vector yValues3 = new Vector(5);
        for (int i = 0; i < 5; i++)
        {
            xValues3[i] = i;
            yValues3[i] = i * i;
        }
        Vector b3Analytical = new Vector("0\n2\n4\n6");
        Vector c3Analytical = new Vector("1\n1\n1\n1");

        // Compute parameters b and c of quadratic splines for each dataset numerically.
        (Vector b1, Vector c1) = ComputeQuadraticSplineParameters(xValues1, yValues1);
        (Vector b2, Vector c2) = ComputeQuadraticSplineParameters(xValues2, yValues2);
        (Vector b3, Vector c3) = ComputeQuadraticSplineParameters(xValues3, yValues3);

        // Compare numerically and analytically calculated results.
        WriteLine("Check that numerically computed quadratic spline parameters agrees with the analytically computed ones for different functions:");
        if (Vector.Approx(b1, b1Analytical) && Vector.Approx(c1, c1Analytical))
        {
            WriteLine(" - constant function (passed)");
        }
        else
        {
            WriteLine(" - constant function (failed)");
        }
        if (Vector.Approx(b2, b2Analytical) && Vector.Approx(c2, c2Analytical))
        {
            WriteLine(" - linear function (passed)");
        }
        else
        {
            WriteLine(" - linear function (failed)");
        }
        if (Vector.Approx(b3, b3Analytical) && Vector.Approx(c3, c3Analytical))
        {
            WriteLine(" - quadratic function (passed)");
        }
        else
        {
            WriteLine(" - quadratic function (failed)");
        }
    }

    // Function computes quadratic spline parameters b, c.
    public static (Vector b, Vector c) ComputeQuadraticSplineParameters(Vector xValues, Vector yValues)
    {
        QuadraticSpline quadraticSpline = new QuadraticSpline(xValues, yValues);
        return (quadraticSpline.b, quadraticSpline.c);
    }
}