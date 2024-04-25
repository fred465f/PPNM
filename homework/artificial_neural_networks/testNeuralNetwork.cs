/* Program tests implementation of simple feed-forward neural network with a
single hidden-layer, identity input neuron and summation output neuron. */

using System;
using LinearAlgebra;
using MachineLearning;
using static System.Math;
using static System.Console;

public class Program
{
    public static void Main()
    {
        // Variables.
        int n = 5;
        Func<double, double> gaussianWavelet = z => z * Exp(-Pow(z, 2));
        Func<double, double> g = z => Cos(5*z - 1) * Exp(-Pow(z, 2));

        // Create tabulated data for g.
        int numPoints = 100;
        double[] xRange = new double[]{-1.0, 1.0};
        Vector x = new Vector(numPoints);
        Vector y = new Vector(numPoints);
        for (int i = 0; i < numPoints; i++)
        {
            double xi = (xRange[1] - xRange[0]) * i / (numPoints - 1) + xRange[0];
            x[i] = xi;
            y[i] = g(xi);
        }

        // Make simple neural network with "n" hidden neurons to approximate g.
        SimpleNeuralNetwork snn = new SimpleNeuralNetwork(n, gaussianWavelet);

        // Train network.
        snn.Train(x, y);

        // Make prediction.
        WriteLine($"{snn.Predict(0)}");
    }
}