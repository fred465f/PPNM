/* Class SimpleNeuralNetwork in MachineLearning namespace implements a simple feed-forward
artificial neural network, using Minimization class from same namespace. */

using System;
using LinearAlgebra;
using MachineLearning;
using static System.Math;
using static System.Console;

namespace MachineLearning
{
    public class SimpleNeuralNetwork
    {
        // Variables.
        int n;
        Func<double, double> f;
        Vector p;
        double quasiNewtonAcc = 0.0001;

        // Constructors.
        public SimpleNeuralNetwork(int numOfHiddenLayers, Func<double, double> activationFunction)
        {
            n = numOfHiddenLayers;
            f = activationFunction;
            p = Vector.RandomVector(3*n);
        }
        public SimpleNeuralNetwork(int numOfHiddenLayers, Func<double, double> activationFunction, Vector initialParameters)
        {
            n = numOfHiddenLayers;
            f = activationFunction;
            if (initialParameters.Length != 3*n)
            {
                throw new ArgumentException("Wrong number of initial parameters for given network. There should be 3 * 'number of hidden neurons'", $"Length of p = {p.Length}");
            }
            p = initialParameters;
        }

        // Training.
        public void Train(Vector x, Vector y)
        {
            // Throw exception if the length of x and y does not match.
            if (x.Length != y.Length)
            {
                throw new ArgumentException("Input data needs to be on table format, i.e the length of x and y must match", $"Length of x = {x.Length} and length of y = {y.Length}");
            }

            // Create cost function.
            Func<Vector, double> costFunction = delegate(Vector parameters) {return HelperCostFunction(parameters, x, y);};

            // Minimize cost function.
            Vector minimizingParameters = Minimization.QuasiNewton(costFunction, this.p, this.quasiNewtonAcc);

            // Update parameters of network.
            this.p = minimizingParameters;
        }

        // Helper function for creating cost function.
        private double HelperCostFunction(Vector parameters, Vector x, Vector y)
        {
            // Variables.
            double sum = 0;

            /* Compute network predictions given parameters p on values in x as well as
            compute cost of network with parameters p and table data {x, y}. */
            for (int i = 0; i < x.Length; i++)
            {
                double prediction = 0;
                for (int j = 0; j < this.n; i++)
                {
                    prediction += this.f((x[j] - parameters[j])/parameters[j + n]) * parameters[j + 2*n];
                }
                sum += Pow(prediction - y[i], 2);
            }

            // Return result.
            return sum;
        }

        // Predict.
        public double Predict(double x)
        {
            // Variables.
            double sum = 0;

            // Compute prediction of network.
            for (int i = 0; i < this.n; i++)
            {
                sum += this.f((x - p[i])/p[i + n]) * p[i + 2*n];
            }

            // Return result.
            return sum;
        }
    }
}