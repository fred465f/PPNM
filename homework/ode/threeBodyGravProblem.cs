/* Program uses ODE class in LinearAlgebra namespace to solve the Newtonian
three-body gravitational problem and saves results to a file for later plotting. */

using System;
using Calculus;
using LinearAlgebra;
using DataStructures;
using static System.Math;
using static System.Console;

class Program
{
    public static void Main()
    {
        // Variables.
        double initialParameterValue = 0;
        double finalParameterValue = 2.1;
        Vector initialSolutionValue = new Vector($"{0.4662036850}\n{0.4323657300}\n{-0.93240737}\n{-0.86473146}\n{0.4662036850}\n{0.4323657300}\n{-0.97000436}\n{0.24308753}\n0.0\n0.0\n{0.97000436}\n{-0.24308753}"); // (x_1',y_1',x_2',y_2',x_3',y_3',x_1,y_1,x_2,y_2,x_3,y_3)
        Func<double, Vector, Vector> function = Function;

        // Make instance of ODE class.
        ODE ode = new ODE(function, initialParameterValue, initialSolutionValue);

        // Call driver to solve ODE.
        GenericList<double> tList = new GenericList<double>();
        GenericList<Vector> zList = new GenericList<Vector>();
        Vector yAtFinalParameterValue = ode.Driver(finalParameterValue, tList, zList);

        // Output results to standard output stream for later plotting.
        WriteLine("#t,vx1,vy1,vx2,vy3,vx3,vy3,x1,y1,x2,y2,x3,y3");
        for (int i = 0; i < zList.Length; i++)
        {
            WriteLine($"{tList[i]},{zList[i][0]},{zList[i][1]},{zList[i][2]},{zList[i][3]},{zList[i][4]},{zList[i][5]},{zList[i][6]},{zList[i][7]},{zList[i][8]},{zList[i][9]},{zList[i][10]},{zList[i][11]}");
        }
    }

    // Function f(x,y) is more complicated, so is written out in more readable detail down here.
    public static Vector Function(double t, Vector z)
    {
        // Compute velocity derivative components vx'_i, vy'_i.
        double vxPrime1 = (z[8]-z[6])/Pow(Pow(z[8]-z[6],2)+Pow(z[9]-z[7],2),1.5) + (z[10]-z[6])/Pow(Pow(z[10]-z[6],2)+Pow(z[11]-z[7],2),1.5);
        double vyPrime1 = (z[9]-z[7])/Pow(Pow(z[8]-z[6],2)+Pow(z[9]-z[7],2),1.5) + (z[11]-z[7])/Pow(Pow(z[10]-z[6],2)+Pow(z[11]-z[7],2),1.5);
        double vxPrime2 = (z[6]-z[8])/Pow(Pow(z[6]-z[8],2)+Pow(z[7]-z[9],2),1.5) + (z[10]-z[8])/Pow(Pow(z[10]-z[8],2)+Pow(z[11]-z[9],2),1.5);
        double vyPrime2 = (z[7]-z[9])/Pow(Pow(z[6]-z[8],2)+Pow(z[7]-z[9],2),1.5) + (z[11]-z[9])/Pow(Pow(z[10]-z[8],2)+Pow(z[11]-z[9],2),1.5);
        double vxPrime3 = (z[6]-z[10])/Pow(Pow(z[6]-z[10],2)+Pow(z[7]-z[11],2),1.5) + (z[8]-z[10])/Pow(Pow(z[8]-z[10],2)+Pow(z[9]-z[11],2),1.5);
        double vyPrime3 = (z[7]-z[11])/Pow(Pow(z[6]-z[10],2)+Pow(z[7]-z[11],2),1.5) + (z[9]-z[11])/Pow(Pow(z[8]-z[10],2)+Pow(z[9]-z[11],2),1.5);

        // Create vector to be returned.
        Vector result = new Vector($"{vxPrime1}\n{vyPrime1}\n{vxPrime2}\n{vyPrime2}\n{vxPrime3}\n{vyPrime3}\n{z[0]}\n{z[1]}\n{z[2]}\n{z[3]}\n{z[4]}\n{z[5]}");

        // Return result.
        return result;
    }
}