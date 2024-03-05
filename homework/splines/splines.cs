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

// Class implementing natural cubic spline interpolation for chosen set of discrete data points.
public class CubicSpline
{
    // Fields.
    Vector x, y, b, c, d;

    // Constructor.
    public CubicSpline(Vector xValues, Vector yValues)
    {
        // Initialize memory.
        x = xValues.Copy();
        y = yValues.Copy();
        int n = x.Length;
        b = new Vector(n);
        c = new Vector(n - 1);
        d = new Vector(n - 1);

        // Initialize memory to vectors used in computation.
        Vector p = new Vector(n - 1);
        Vector h = new Vector(n - 1);
        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i];
            p[i] = (y[i + 1] - y[i]) / h[i];
        }

        // Build tri-diagonal system.
        Matrix A = new Matrix(n);
        Vector B = new Vector(n);
        A[0, 0] = 2;
        A[0, 1] = 1;
        A[n - 1, n - 1] = 2;
        B[0] = 3 * p[0];
        B[n - 1] = 3 * p[n - 2];
        for (int i = 0; i < n - 1; i++)
        {
            if (i == n - 2)
            {
                A[i + 1, i] = 1;
            }
            else
            {
                A[i + 1, i] = 1;
                A[i + 1, i + 1] = 2 * h[i] / h[i + 1] + 2;
                A[i + 1, i + 2] = h[i] / h[i + 1];
                B[i + 1] = 3 * (p[i] + p[i + 1] * h[i] / h[i + 1]);
            }
        }

        // Do a run of Gauss elimination.
        for (int i = 0; i < n - 1; i++)
        {
            double entry = A[i + 1, i];
            if (!Matrix.Approx(entry, 0.0))
            {
                A[i + 1, i] = 0;
                A[i + 1, i + 1] -= A[i, i + 1] * (entry / A[i, i]);
                B[i + 1] -= B[i] * (entry / A[i, i]);
            }
        }

        // Solve using back-substitution.
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += A[i, j] * B[j];
            }
            B[i] = (B[i] - sum) / A[i, i];
        }

        // Compute b, c and d.
        b = B.Copy();
        for (int i = 0; i < n - 1; i++)
        {
            c[i] = (-2 * b[i] - b[i + 1] + 3 * p[i]) / h[i];
            d[i] = (b[i] + b[i + 1] - 2 * p[i]) / (h[i] * h[i]);
        }
    }

    // Do interpolation using quadratic splines.
    public double Interpolate(double z)
    {
        int i = BinarySearch(x, z);
        return y[i] + b[i] * (z - x[i]) + c[i] * Pow(z - x[i], 2) + d[i] * Pow(z - x[i], 3);
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
                integral += y[j] * z + 1.0/2.0 * b[j] * Pow(z - x[j], 2) + 1.0/3.0 * c[j] * Pow(z - x[j], 3) + 1.0/4.0 * d[j] * Pow(z - x[j], 4);
                integral -= y[j] * x[j];
            }
            else
            {
                integral += y[j] * x[j + 1] + 1.0/2.0 * b[j] * Pow(x[j + 1] - x[j], 2) + 1.0/3.0 * c[j] * Pow(x[j + 1] - x[j], 3) + 1.0/4.0 * d[j] * Pow(x[j + 1] - x[j], 4);
                integral -= y[j] * x[j];
            }
        }
        return integral;
    }

    // Perform differentiation from first x-value x[0] to chosen z > x[0].
    public double Derivative(double z)
    {
        int i = BinarySearch(x, z);
        return b[i] + 2.0 * c[i] * (z - x[i]) + 3.0 * d[i] * Pow(z - x[i], 2);
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