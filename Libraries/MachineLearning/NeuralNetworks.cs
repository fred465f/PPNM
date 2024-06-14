/* Class SimpleNeuralNetwork in MachineLearning namespace implements a simple feed-forward
artificial neural network with one hidden layer and Gaussian wavelet activation function, 
using Minimization class from same namespace. It also implements simple neural network
for solving second order ODE's

Φ[y(x)]≡Φ(y'',y',y,x)=0,

with y function on interval [a,b] and given initial data

y(c)=yc, y'(c)=y'c.

where c∈[a,b]. An appropriate cost function could be

C(p)=∫ab Φ[Fp(x)]² dx + α (Fp(c)-yc)² + β (Fp'(c)-y'c)²,

where α and β are parameters that specify the relative contribution of the initial data to the cost-function. */

using System;
using Calculus;
using LinearAlgebra;
using MachineLearning;
using static System.Math;
using static System.Console;

namespace MachineLearning
{
    // Implements simple ANN mentioned in file description.
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

    // Class for solving ODE problem stated in file description using same simple ANN structure as above.
    public class ODESolver
    {
        // Network variables.
        int n;
        Vector p;
        double quasiNewtonAcc = 0.00001;
        double adaptiveIntegratorAcc = 0.00001;
        double adaptiveIntegratorEps = 0.00001;
        Func<double, double> f = z => z * Exp(-Pow(z, 2));
        Func<double, double> fDerivative = z => Exp(-Pow(z, 2)) - 2*Pow(z, 2)*Exp(-Pow(z, 2));
        Func<double, double> fDoubleDerivative = z => -6*z*Exp(-Pow(z, 2)) + 4*Pow(z, 3)*Exp(-Pow(z, 2));

        // ODE variables.
        Func<Func<double, double>, Func<double, double>, Func<double, double>, double, double> phi; // phi(y, y', y'', x) = 0.
        double[] interval = new double[2]; // [a, b].
        double[] initialData = new double[3]; // [c, y(c), y'(c)].

        // Constructor.
        public ODESolver(Func<Func<double, double>, Func<double, double>, Func<double, double>, double, double> diffEq, double a, double b, double c, double yc, double ycPrime, int numOfHiddenNeurons)
        {
            // Set network parameters.
            n = numOfHiddenNeurons;
            p = Vector.RandomVector(3*n);

            // Set ODE parameters.
            phi = diffEq;
            interval[0] = a;
            interval[1] = b;
            initialData[0] = c;
            initialData[1] = yc;
            initialData[2] = ycPrime;
        }

        // Train.
        public void Train(double alpha, double beta)
        {
            // Create cost function.
            Func<Vector, double> costFunction = delegate(Vector parameters) {return HelperCostFunction(parameters, alpha, beta);};

            // Minimize cost function.
            Vector minimizingParameters = Minimization.QuasiNewton(costFunction, this.p, this.quasiNewtonAcc);

            // Update parameters of network.
            this.p = minimizingParameters;
        }

        // Helper function for creating cost function.
        private double HelperCostFunction(Vector parameters, double alpha, double beta)
        {
            // Variables.
            double sum = 0;

            // Compute integral contribution to cost function.
            Func<double, double> y = x => Predict(x);
            Func<double, double> yPrime = x => Derivative(x);
            Func<double, double> yDoublePrime = x => DoubleDerivative(x);
            Func<double, double> integrand = x => Pow(phi(y, yPrime, yDoublePrime, x), 2);
            (double integral, double error, int numSteps) = AdaptiveIntegrator.Integrate(integrand, this.interval[0], this.interval[1], this.adaptiveIntegratorAcc, this.adaptiveIntegratorEps);
            sum += integral;

            // Compute initial data contribution to cost function.
            sum += alpha * Pow(Predict(this.initialData[0]) - this.initialData[1], 2) + beta * Pow(Derivative(this.initialData[0]) - this.initialData[2], 2);

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
    }
}