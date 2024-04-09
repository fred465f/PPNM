/* Program fits Higgs data to determine the mass and the experimental width of the Higgs boson. */

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
        // Parse input data.
        var energy = new GenericList<double>();
        var signal = new GenericList<double>();
        var error = new GenericList<double>();
        do
        {
            string line = Console.In.ReadLine();
            if (line == null)
            {
                break;
            }
            else if (line[0] == '#')
            {
                continue;
            }
            var words = line.Split(",");
            energy.Add(double.Parse(words[0]));
            signal.Add(double.Parse(words[1]));
            error.Add(double.Parse(words[2]));
        } while (true);

        // Variables.
        double acc = 0.0001;
        int numPoints = 1000;
        Func<Vector, double> deviation = delegate(Vector v) {return DeviationHelperFunction(energy, signal, error, v[0], v[1], v[2]);};

        // Fit data.
        Vector initialGuess = new Vector("125.3\n-2.0\n10.0");
        Vector minimizingParameters = Minimization.QuasiNewton(deviation, initialGuess, acc);
        double m = minimizingParameters[0];
        double gamma = minimizingParameters[1];
        double A = minimizingParameters[2];
        

        // Save fitting constants and fitting data for later plotting.
        WriteLine($"# Optimal parameters were found to be m = {m}, Gamma = {gamma}, A = {A}.\n#");
        WriteLine("# Energy [GeV], predicted signal [certain units]");
        double[] energyRange = new double[2] {energy[0], energy[energy.Length - 1]};
        for (int i = 0; i < numPoints; i++)
        {
            double currentEnergy = (energyRange[1] - energyRange[0]) * i / (numPoints - 1) + energyRange[0];
            double predictedSignal = F(currentEnergy, m, gamma, A);
            WriteLine($"{currentEnergy},{predictedSignal}");
        }
    }

    // The Breit-Wigner function.
    public static double F(double E, double m, double gamma, double A)
    {
        return A / (Pow(E - m, 2) + Pow(gamma, 2) / 4.0);
    }

    // Creates delegate corresponding to the deviation function.
    public static double DeviationHelperFunction(GenericList<double> energy, GenericList<double> signal, GenericList<double> error, double m, double gamma, double A)
    {
        double sum = 0;
        for (int i = 0; i < energy.Length; i++)
        {
            sum += Pow(F(energy[i], m, gamma, A) - signal[i], 2) / Pow(error[i], 2);
        }
        return sum;
    }
}