/* Program test QuadraticSpline class by producing data for later plotting. */

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
            yValues1[i] = 1;
        }
        Vector b1Analytical = new Vector("0\n0\n0\n0");
        Vector c1Analytical = new Vector("0\n0\n0\n0");

        // Create dataset 2 to be tested.
        Vector xValues2 = new Vector(5);
        Vector yValues2 = new Vector(5);
        for (int i = 0; i < 5; i++)
        {
            xValues1[i] = i;
            yValues1[i] = i;
        }
        Vector b2Analytical = new Vector("1\n1\n1\n1");
        Vector c2Analytical = new Vector("1\n1\n1\n1");

        // Create dataset 3 to be tested.
        Vector xValues3 = new Vector(5);
        Vector yValues3 = new Vector(5);
        for (int i = 0; i < 5; i++)
        {
            xValues1[i] = i;
            yValues1[i] = i * i;
        }
        Vector b3Analytical = new Vector("1\n1\n1\n1");
        Vector c3Analytical = new Vector("1\n1\n1\n1");

        // Compute parameters b and c of quadratic splines for each dataset numerically.
        (Vector b1, Vector c1) = ComputeQuadraticSplineParameters(xValues1, yValues1);
        (Vector b2, Vector c2) = ComputeQuadraticSplineParameters(xValues2, yValues2);
        (Vector b3, Vector c3) = ComputeQuadraticSplineParameters(xValues3, yValues3);

        // Compare numerically and analytically calculated results.
        if (Vector.Approx(b1, b1Analytical) && Vector.Approx(c1, c1Analytical))
        {
            WriteLine("Numerically computed parameters of quadratic splines agreed with the analytically computed ones.");
        }
        else
        {
            WriteLine("Numerically computed parameters of quadratic splines did not agree with the analytically computed ones.");
        }
        if (Vector.Approx(b2, b2Analytical) && Vector.Approx(c2, c2Analytical))
        {
            WriteLine("Numerically computed parameters of quadratic splines agreed with the analytically computed ones.");
        }
        else
        {
            WriteLine("Numerically computed parameters of quadratic splines did not agree with the analytically computed ones.");
        }
        if (Vector.Approx(b3, b3Analytical) && Vector.Approx(c3, c3Analytical))
        {
            WriteLine("Numerically computed parameters of quadratic splines agreed with the analytically computed ones.");
        }
        else
        {
            WriteLine("Numerically computed parameters of quadratic splines did not agree with the analytically computed ones.");
        }
    }

    // Function computes quadratic spline parameters b, c.
    public static (Vector b, Vector c) ComputeQuadraticSplineParameters(Vector xValues, Vector yValues)
    {
        QuadraticSpline quadraticSpline = new QuadraticSpline(xValues, yValues);
        return (quadraticSpline.b, quadraticSpline.c);
    }
}