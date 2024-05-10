/* Class SimpleNeuralNetwork in MachineLearning namespace implements a simple feed-forward
artificial neural network with one hidden layer and Gaussian wavelet activation function, 
using Minimization class from same namespace. */

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
        Func<double, double> f = z => z * Exp(-Pow(z, 2));
        Func<double, double> fDerivative = z => Exp(-Pow(z, 2)) - 2*Pow(z, 2)*Exp(-Pow(z, 2));
        Func<double, double> fDoubleDerivative = z => -6*z*Exp(-Pow(z, 2)) + 4*Pow(z, 3)*Exp(-Pow(z, 2));
        Func<double, double> fAntiDerivative = z => -0.5 * Exp(-Pow(z, 2));
        Vector p;
        double quasiNewtonAcc = 0.00001;

        // Constructors.
        public SimpleNeuralNetwork(int numOfHiddenNeurons)
        {
            n = numOfHiddenNeurons;
            p = Vector.RandomVector(3*n);
        }
        public SimpleNeuralNetwork(int numOfHiddenNeurons, Vector initialParameters)
        {
            n = numOfHiddenNeurons;
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
                for (int j = 0; j < this.n; j++)
                {
                    prediction += this.f((x[i] - parameters[3*j])/parameters[3*j + 1]) * parameters[3*j + 2];
                }
                sum += Pow(prediction - y[i], 2);
            }

            // Return result.
            return sum / x.Length;
        }

        // Predict.
        public double Predict(double x)
        {
            // Variables.
            double sum = 0;

            // Compute prediction of network.
            for (int i = 0; i < this.n; i++)
            {
                sum += this.f((x - p[3*i])/p[3*i + 1]) * p[3*i + 2];
            }

            // Return result.
            return sum;
        }

        // Derivative.
        public double Derivative(double x)
        {
            // Variables.
            double sum = 0;

            // Compute prediction of network.
            for (int i = 0; i < this.n; i++)
            {
                sum += (1/p[3*i + 1]) * this.fDerivative((x - p[3*i])/p[3*i + 1]) * p[3*i + 2];
            }

            // Return result.
            return sum;
        }

        // Double derivative.
        public double DoubleDerivative(double x)
        {
            // Variables.
            double sum = 0;

            // Compute prediction of network.
            for (int i = 0; i < this.n; i++)
            {
                sum += Pow(1/p[3*i + 1], 2) * this.fDoubleDerivative((x - p[3*i])/p[3*i + 1]) * p[3*i + 2];
            }

            // Return result.
            return sum;
        }

        // Anti-derivative.
        public double AntiDerivative(double x)
        {
            // Variables.
            double sum = 0;

            // Compute prediction of network.
            for (int i = 0; i < this.n; i++)
            {
                sum += p[3*i + 1] * this.fAntiDerivative((x - p[3*i])/p[3*i + 1]) * p[3*i + 2];
            }

            // Return result.
            return sum;
        }
    }
}