using System;
using LinearAlgebra;
using static System.Math;
using static System.Console;

// Class implementing linear spline interpolation for chosen set of discrete data points.
public class LinearSpline
{
    // Fields.
    public Vector x, y;

    // Constructor.
    public LinearSpline(Vector xValues, Vector yValues)
    {
        x = xValues.Copy();
        y = yValues.Copy();
    }

    // Do interpolation using linear splines.
    public double Interpolate(double z) 
    {
        if (!(z >= x[0] && z <= x[x.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{x[0]}, {x[x.Length - 1]}].");
        }
        else
        {
            int i = BinarySearch(x, z);
            double dx = x[i + 1] - x[i];
            if (!(dx > 0))
            {
                throw new ArgumentException("Either dx = 0 or dx < 0, either case should not occur and is a result of faulty input values.", $"dx = {dx}");
            }
            double dy = y[i + 1] - y[i];
            return y[i] + dy/dx * (z - x[i]);
        }
    }

    // Perform integration from first x-value x[0] to chosen z > x[0].
    public double Integrate(double z)
    {
        if (!(z >= x[0] && z <= x[x.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{x[0]}, {x[x.Length - 1]}].");
        }
        else
        {
            int i = BinarySearch(x, z);
            double integral = 0.0;
            for (int j = 0; j <= i; j++)
            {
                double dx = x[j + 1] - x[j];
                if (!(dx > 0))
                {
                    throw new ArgumentException("Either dx = 0 or dx < 0, either case should not occur and is a result of faulty input values.", $"dx = {dx}");
                }
                double dy = y[j + 1] - y[j];
                if (j == i)
                {
                    integral += (0.5) * (dy/dx) * z*z + (y[j] - (dy/dx) * x[j]) * z;
                    integral -= (0.5) * (dy/dx) * x[j]*x[j] + (y[j] - (dy/dx) * x[j]) * x[j];
                }
                else
                {
                    integral += (0.5) * (dy/dx) * x[j + 1]*x[j + 1] + (y[j] - (dy/dx) * x[j]) * x[j + 1];
                    integral -= (0.5) * (dy/dx) * x[j]*x[j] + (y[j] - (dy/dx) * x[j]) * x[j];
                }
            }
            return integral;
        }
    }

    // Perform binary search.
    public static int BinarySearch(Vector xValues, double z)
    {
        if (!(z >= xValues[0] && z <= xValues[xValues.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{xValues[0]}, {xValues[xValues.Length - 1]}].");
        }
        else
        {
            int i = 0;
            int j = xValues.Length - 1;
            while (j - i > 1)
            {
                int middle = (i + j) / 2;
                if (z > xValues[middle])
                {
                    i = middle;
                }
                else
                {
                    j = middle;
                }
            }
            return i;
        }
    }
}

// Class implementing quadratic spline interpolation for chosen set of discrete data points.
public class QuadraticSpline
{
    // Fields.
    public Vector x, y, b, c;

    // Constructor.
    public QuadraticSpline(Vector xValues, Vector yValues)
    {
        // Initialize memory.
        x = xValues.Copy();
        y = yValues.Copy();
        b = new Vector(x.Length - 1);
        c = new Vector(x.Length - 1);

        // Initialize memory to vectors used in computation
        Vector p = new Vector(x.Length - 1);
        Vector h = new Vector(x.Length - 1);
        for (int i = 0; i < x.Length - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        // Up recursion with c_0 = 0.
        c[0] = 0;
        for (int i = 0; i < x.Length - 2; i++)
        {
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];
        }

        // Down recursion with c_(x.Length-1) := c_(x.Length-1)/2.
        c[x.Length - 2] /= 2.0;
        for (int i = x.Length - 3; i >= 0; i--)
        {
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];
        }

        // Computing b.
        for (int i = 0; i < x.Length - 1; i++)
        {
            b[i] = p[i] - c[i] * h[i];
        }
    }

    // Do interpolation using quadratic splines.
    public double Interpolate(double z)
    {
        int i = BinarySearch(x, z);
        return y[i] + b[i] * (z - x[i]) + c[i] * Pow(z - x[i], 2);
    }

    // Perform integration from first x-value x[0] to chosen z > x[0].
    public double Integrate(double z)
    {
        int i = BinarySearch(x, z);
        double integral = 0;
        for (int j = 0; j <= i; j++)
        {
            if (i == j)
            {
                integral += y[j] * z + 1.0/2.0 * b[j] * Pow(z - x[j], 2) + 1.0/3.0 * c[j] * Pow(z - x[j], 3);
                integral -= y[j] * x[j];
            }
            else
            {
                integral += y[j] * x[j + 1] + 1.0/2.0 * b[j] * Pow(x[j + 1] - x[j], 2) + 1.0/3.0 * c[j] * Pow(x[j + 1] - x[j], 3);
                integral -= y[j] * x[j];
            }
        }
        return integral;
    }

    // Perform differentiation from first x-value x[0] to chosen z > x[0].
    public double Derivative(double z)
    {
        int i = BinarySearch(x, z);
        return b[i] + 2.0 * c[i] * (z - x[i]);
    }

    // Perform binary search.
    public static int BinarySearch(Vector xValues, double z)
    {
        if (!(z >= xValues[0] && z <= xValues[xValues.Length - 1]))
        {
            throw new ArgumentException("Chosen z-value must lie in interval [xValues[0], xValues[Length - 1]].", $"z-value = {z} does not lie in interval [{xValues[0]}, {xValues[xValues.Length - 1]}].");
        }
        else
        {
            int i = 0;
            int j = xValues.Length - 1;
            while (j - i > 1)
            {
                int middle = (i + j) / 2;
                if (z > xValues[middle])
                {
                    i = middle;
                }
                else
                {
                    j = middle;
                }
            }
            return i;
        }
    }
}