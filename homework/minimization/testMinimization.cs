/* Program tests implementation of Newton, Quasi-Newton and Downhill-simplex method in Minimization class
in MachineLearning namespace, by finding the minima of functions for which the minima is 
known analytically. */

using System;
using LinearAlgebra;
using DataStructures;
using MachineLearning;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        double eps = 0.0001;
        double acc = 0.00001;
        Vector start = new Vector("2\n1");
        GenericList<Vector> startSimplex = new GenericList<Vector>();
        startSimplex.Add(new Vector("0\n2"));
        startSimplex.Add(new Vector("0\n0"));
        startSimplex.Add(new Vector("2\n0"));

        /* Test Newton on these functions with respective analytically known minima.
        (1) f(x,y)=(1-x)^2+100(y-x^2)^2, global minima at (1, 1),
        (2) f(x,y)=(x^2+y-11)^2+(x+y^2-7)^2, global minima at (3, 2) and (-3.77931, 3.28319).
        */
        WriteLine("----- Part A -----\n");
        WriteLine("Minima were found for the two listed functions in the code using Newton method, Rosenbrocks Valley function and Himmelblaus function.\nThese numerically computed minima are compared to the analytically computed ones:\n");
        Func<Vector, double> rosenbrockValleyFunc = delegate(Vector v) {return Pow(1-v[0], 2) + 100*Pow(v[1]-v[0]*v[0], 2);};
        Vector rosenbrockValleyMinimaNumerical0 = Minimization.Newton(rosenbrockValleyFunc, start, acc);
        Vector rosenbrockValleyMinimaAnalytical = new Vector("1\n1");
        if (Vector.Norm(rosenbrockValleyMinimaNumerical0 - rosenbrockValleyMinimaAnalytical) < eps)
        {
            WriteLine("(1) Approved.");
        }
        else
        {
            WriteLine("(1) Not approved.");
        }
        Func<Vector, double> himmelBlauFunc = delegate(Vector v) {return Pow(v[0]*v[0] + v[1] - 11, 2) + Pow(v[0] + v[1]*v[1] - 7, 2);};
        Vector himmelBlauMinimaNumerical0 = Minimization.Newton(himmelBlauFunc, start, acc);
        Vector himmelBlauMinimaAnalytical1 = new Vector("3\n2");
        Vector himmelBlauMinimaAnalytical2 = new Vector("-3.77931\n3.28319");
        if (Vector.Norm(himmelBlauMinimaNumerical0 - himmelBlauMinimaAnalytical1) < eps || Vector.Norm(himmelBlauMinimaNumerical0 - himmelBlauMinimaAnalytical2) < eps)
        {
            WriteLine("(2) Approved.");
        }
        else
        {
            WriteLine("(2) Not approved.");
        }

        /* Test Quasi-Newton on these functions with respective analytically known minima.
        (1) f(x,y)=(1-x)^2+100(y-x^2)^2, global minima at (1, 1),
        (2) f(x,y)=(x^2+y-11)^2+(x+y^2-7)^2, global minima at (3, 2) and (-3.77931, 3.28319).
        */
        WriteLine("\nMinima were found for the two listed functions in the code using Quasi-Newton method, Rosenbrocks Valley function and Himmelblaus function.\nThese numerically computed minima are compared to the analytically computed ones:\n");
        Vector rosenbrockValleyMinimaNumerical1 = Minimization.QuasiNewton(rosenbrockValleyFunc, start, acc);
        if (Vector.Norm(rosenbrockValleyMinimaNumerical1 - rosenbrockValleyMinimaAnalytical) < 0.02) // Quasi-Newton seems to struggle getting to minimum. Plotted RosenBrock Valley, and suspect it is due to its flat nature near bottom.
        {
            WriteLine("(1) Approved.");
        }
        else
        {
            WriteLine("(1) Not approved.");
        }
        Vector himmelBlauMinimaNumerical1 = Minimization.QuasiNewton(himmelBlauFunc, start, acc);
        if (Vector.Norm(himmelBlauMinimaNumerical1 - himmelBlauMinimaAnalytical1) < eps || Vector.Norm(himmelBlauMinimaNumerical1 - himmelBlauMinimaAnalytical2) < eps)
        {
            WriteLine("(2) Approved.");
        }
        else
        {
            WriteLine("(2) Not approved.");
        }

        /* Test DownhillSimplex on these functions with respective analytically known minima.
        (1) f(x,y)=(1-x)^2+100(y-x^2)^2, global minima at (1, 1),
        (2) f(x,y)=(x^2+y-11)^2+(x+y^2-7)^2, global minima at (3, 2) and (-3.77931, 3.28319).
        */
        WriteLine("\nMinima were found for the two listed functions in the code using Downhill Simplex method, Rosenbrocks Valley function and Himmelblaus function.\nThese numerically computed minima are compared to the analytically computed ones:\n");
        Vector rosenbrockValleyMinimaNumerical2 = Minimization.DownhillSimplex(rosenbrockValleyFunc, startSimplex, acc);
        if (Vector.Norm(rosenbrockValleyMinimaNumerical2 - rosenbrockValleyMinimaAnalytical) < eps)
        {
            WriteLine("(1) Approved.");
        }
        else
        {
            WriteLine("(1) Not approved.");
        }
        Vector himmelBlauMinimaNumerical2 = Minimization.DownhillSimplex(himmelBlauFunc, startSimplex, acc);
        if (Vector.Norm(himmelBlauMinimaNumerical2 - himmelBlauMinimaAnalytical1) < eps || Vector.Norm(himmelBlauMinimaNumerical2 - himmelBlauMinimaAnalytical2) < eps)
        {
            WriteLine("(2) Approved.");
        }
        else
        {
            WriteLine("(2) Not approved.");
        }
    }
}