using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        Vector xValues = new Vector("0\n1\n2\n3\n4\n5");
        Vector yValues = new Vector("0\n1\n2\n3\n4\n5");
        LinearSpline linearSpline = new LinearSpline(xValues, yValues);
        double three = linearSpline.Interpolate(3.0);
        double half = linearSpline.Integrate(1.0);
        WriteLine($"three = {three}");
        WriteLine($"half = {half}");
    }
}