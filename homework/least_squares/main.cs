/* Program checks implementation of LeastSquares class in LinearAlgebra namespace,
by applying it to the given radioactive decay data of ThX. It takes two input filenames,
one storing input data and one referring to file which should contain the output data,
used for plotting in Makefile. */

using System;
using static System.Math;
using static System.Console;
using LinearAlgebra;

class Program
{
    public static int Main(string[] args)
    {
        // Variables.
        string dataInFile = "";
        string dataOutFile = "";

        // Parse command-line inputs.
        foreach (var arg in args)
        {
            var words = arg.Split(":");
            if (words[0] == "-inFile")
            {
                dataInFile = words[1];
            }
            else if (words[0] == "-outFile")
            {
                dataOutFile = words[1];
            }
        }

        // Throw error if no input or output files were provided.
        if (dataInFile == string.Empty || dataOutFile == string.Empty)
        {
            return 1;
        }

        // Extract data from input file.
        Matrix inputData = new Matrix(dataInFile);

        // Radioactive decay data for ThX.
        Vector time = inputData[0]; // In units of days.
        Vector activity = inputData[1]; // In relative units.
        Vector activityError = inputData[2];

        /* We fit after the exponential decay using the logarithmic approach,
        i.e ln(y) = ln(a) - \lambda * t. Define fitting functions for this. */
        var functions = new Func<double, double>[] {x => 1, x => x};

        // Apply logarithmic transformation to activity data.
        Vector activityLog = activity.Apply(Log);
        
        // Uncertainty of logarithm should be properly taken care of as dln = dy / y.
        Vector activityErrorLog = new Vector(activityError.Length);
        for (int i = 0; i < activityError.Length; i++)
        {
            activityErrorLog[i] = activityError[i] / activity[i];
        }

        // Perform least squares fit.
        (Vector coefficients, Matrix covarianceMatrix) = LeastSquares.Fit(functions, time, activityLog, activityErrorLog);

        // Transform coefficients back pre-log form.
        double a = Exp(coefficients[0]);
        double aError = Sqrt(covarianceMatrix[0, 0]); // This is actually not correct! This is needs error propagation trough ln(x). Done below.
        aError = aError * a;
        double lambda = - coefficients[1];
        double lambdaError = Sqrt(covarianceMatrix[1, 1]); // Error propagation for transformation x --> -x is trivial.
        WriteLine($"Exponential fit of data, i.e a * exp(lambda * t), was performed. Fitting parameters was found to be a = {Round(a, 3)} +- {Round(aError, 3)} and lambda = {Round(lambda, 3)} +- {Round(lambdaError, 3)}.");

        // Construct fit data.
        using (var outStream = new System.IO.StreamWriter(dataOutFile, append:false))
        {
            for (int i = 0; i <= 16; i++)
            {
                outStream.WriteLine($"{i},{a * Exp(- lambda * i)},{(a - aError) * Exp(- (lambda - lambdaError) * i)},{(a + aError) * Exp(- (lambda + lambdaError) * i)}");
            }
        }

        // Compute half-life and propagated error.
        double halfLife = Log(2) / lambda;
        double halfLifeError = Log(2) / (lambda * lambda) * lambdaError;
        double comparisonValueForHalfLife = 3.631;
        WriteLine($"Got half-life of ThX to be {Round(halfLife, 3)} +- {Round(halfLifeError, 3)}, which should be compared with {comparisonValueForHalfLife}. Does not seem to be in agreement within the uncertainties!");

        // Successful program.
        return 0;
    }
}