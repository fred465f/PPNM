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
        int n = 12;
        Func<double, double> g = z => Cos(5*z-1) * Exp(-Pow(z, 2));
        Func<double, double> gDerivative = z => -5*Sin(5*z-1) * Exp(-Pow(z, 2)) - 2*z*Cos(5*z-1) * Exp(-Pow(z, 2));
        Func<double, double> gDoubleDerivative = z => (-27*Cos(5*z-1) + 20*z*Sin(5*z-1) + 4*Pow(z, 2)*Cos(5*z-1)) * Exp(-Pow(z, 2));

        // Create tabulated data for g.
        int numPoints = 100;
        double[] xRange = new double[]{-1.0, 1.0};
        Vector x = new Vector(numPoints);
        Vector y = new Vector(numPoints);
        Vector yDerivative = new Vector(numPoints);
        Vector yDoubleDerivative = new Vector(numPoints);
        for (int i = 0; i < numPoints; i++)
        {
            double xi = (xRange[1] - xRange[0]) * i / (numPoints - 1) + xRange[0];
            x[i] = xi;
            y[i] = g(xi);
            yDerivative[i] = gDerivative(xi);
            yDoubleDerivative[i] = gDoubleDerivative(xi);
        }

        // Make simple neural network with "n" hidden neurons to approximate g.
        SimpleNeuralNetwork snn = new SimpleNeuralNetwork(n);

        // Train network.
        snn.Train(x, y);

        /* Make prediction and save results. y, y' and y'' are analytical results while f, f', f'' and F are numerical ones 
        using ANN. I wont compare analytic results with the numerically computed anti-derivative F(x), since the analytic
        result is quite complicated. Could be compared with results from online artificial intelligence */
        WriteLine("# x, y, y', y'', f(x), f'(x), f''(x), F(x)");
        for (int i = 0; i < numPoints; i++)
        {
            // Predict function value, derivative, double derivative and anti-derivative of desired function.
            double functionValue = snn.Predict(x[i]);
            double derivative = snn.Derivative(x[i]);
            double doubleDerivative = snn.DoubleDerivative(x[i]);
            double antiDerivative = snn.AntiDerivative(x[i]);

            // Save results.
            WriteLine($"{x[i]},{y[i]},{yDerivative[i]},{yDoubleDerivative[i]},{functionValue},{derivative},{doubleDerivative},{antiDerivative}");
        }
    }
}