/* */

using System;
using LinearAlgebra;
using DataStructures;
using static System.Math;
using static System.Console;

namespace MachineLearning
{
    public static class Minimization
    {
        // Implements Quasi-Newton method with numerical gradient, back-tracking linesearch, and rank-1 update.
        public static Vector QuasiNewton(Func<Vector, double> f, Vector start, double acc)
        {
            // Variables
            int n = start.Length;
            Vector x = start.Copy();
            Vector fGrad = Grad(f, x);
            Matrix inverseHessian = Matrix.Identity(n);

            // Iterate until norm of gradient of objective function f is less than desired accuracy.
            while (Vector.Norm(fGrad) > acc)
            {
                // Compute Newton-step.
                Vector xDelta = - inverseHessian * fGrad;

                // Do linesearch starting with lambda = 1.
                double lambda = 1.0;
                while (true)
                {
                    if (f(x + lambda * xDelta) < f(x))
                    {
                        // Compute desired quantities and avoid division by zero exception.
                        Vector xUpdated = x + lambda * xDelta;
                        Vector fGradUpdated = Grad(f, xUpdated);
                        Vector y = fGradUpdated - fGrad;
                        Vector u = lambda * xDelta - inverseHessian * y;

                        if (Vector.InnerProduct(u, y) < Pow(10, -6))
                        {
                            // Reset inverse of Hessian to the identity.
                            inverseHessian = Matrix.Identity(n);
                        }
                        else
                        {
                            // Update inverse of Hessian.
                            inverseHessian += Vector.OuterProduct(u, u) / Vector.InnerProduct(u, y);
                        }

                        // Update gradient and x.
                        x = xUpdated;
                        fGrad = fGradUpdated;

                        // Break out of while loop.
                        break;
                    }
                    lambda /= 2.0;
                    if (lambda < 1.0/1024.0)
                    {
                        // Update x.
                        x += lambda * xDelta;

                        // Reset inverse of Hessian to the identity.
                        inverseHessian = Matrix.Identity(n);

                        // Update gradient.
                        fGrad = Grad(f, x);

                        // Break out of while loop.
                        break;
                    }
                }
            }

            // Return result.
            return x;
        }

        // Implements the downhill simplex method for minimization.
        public static Vector DownhillSimplex(Func<Vector, double> f, GenericList<Vector> simplexStart, double tolerance = 0.001)
        {
            // Variables.
            int n = 0;
            if (simplexStart.Length != simplexStart[0].Length + 1)
            {
                throw new ArgumentException("A simplex of n-dimensional vectors must contain n + 1 vectors", "Length of vectors = {simplexStart[0].Length} and there are {simplexStart.Length} of these.");
            }
            else
            {
                n = simplexStart.Length - 1;
            }
            double currentSimplexSize = tolerance * 2.0;
            GenericList<Vector> simplex = simplexStart.Copy();

            // Repeat minimization steps until the size of the simplex is smaller than some user specified tolerance.
            while (currentSimplexSize > tolerance)
            {
                // Find largest and smallest points of the current simplex.
                int largestIndex = 0, smallestIndex = 0;
                double fLargest = f(simplex[0]), fSmallest = f(simplex[0]);
                for (int i = 0; i < n + 1; i++)
                {
                    double fCurrent = f(simplex[i]);
                    if (fCurrent < fSmallest)
                    {
                        smallestIndex = i;
                        fSmallest = fCurrent;
                    }
                    else if (fCurrent > fLargest)
                    {
                        largestIndex = i;
                        fLargest = fCurrent;
                    }
                }

                // Compute centroid of simplex.
                Vector centroid = new Vector(n);
                for (int i = 0; i < n + 1; i++)
                {
                    if (i != largestIndex)
                    {    
                        centroid += simplex[i];
                    }
                }
                centroid *= 1.0/n;

                // Try reflection.
                Vector largest = simplex[largestIndex];
                Vector smallest = simplex[smallestIndex];
                Vector reflected = centroid + (centroid - largest);
                if (f(reflected) < f(smallest))
                {
                    // Try expansion.
                    Vector expansion = centroid + 2.0 * (centroid - largest);
                    if (f(expansion) < f(reflected))
                    {
                        // Accept expansion.
                        simplex[largestIndex] = expansion;
                    }
                    else
                    {
                        // Accept reflection.
                        simplex[largestIndex] = reflected;
                    }
                }
                else
                {
                    if (f(reflected) < f(largest))
                    {
                        // Accept reflection.
                        simplex[largestIndex] = reflected;
                    }
                    else
                    {
                        // Try contraction.
                        Vector contraction = centroid + 0.5 * (largest - centroid);
                        if (f(contraction) < f(largest))
                        {
                            // Accept contraction.
                            simplex[largestIndex] = contraction;
                        }
                        else
                        {
                            // Do reduction.
                            for (int i = 0; i < n + 1; i++)
                            {
                                if (i != smallestIndex)
                                {
                                    simplex[i] = 0.5 * (simplex[i] + smallest);
                                }
                            }
                        }
                    }
                }

                // Compute and save size of updated simplex.
                currentSimplexSize = SizeOfSimplex(simplex);
            }

            // Find vector in simplex with lowest value of f.
            int lowestIndex = 0;
            double fLowest = f(simplex[0]);
            for (int i = 0; i < n + 1; i++)
            {
                if (f(simplex[i]) < fLowest)
                {
                    lowestIndex = i;
                    fLowest = f(simplex[i]);
                }
            }

            // Return result.
            return simplex[lowestIndex];
        }

        // Method computes numerical gradient of input real-valued function at input point.
        public static Vector Grad(Func<Vector, double> f, Vector x)
        {
            // Variables.
            Vector dx = x.Apply(Abs) * Pow(2, -26);
            double fx = f(x);
            Vector fGrad = new Vector(x.Length);

            // Compute gradient.
            for (int j = 0; j < x.Length; j++)
            {
                Vector xDisplaced = x.Copy();
                xDisplaced[j] += dx[j];
                fGrad[j] = (f(xDisplaced) - fx)/dx[j];
            }

            // Return result.
            return fGrad;
        }

        // Method computes a size measure for simplices. The radius, largest distance between a vertex and the center, is used as a size measure.
        public static double SizeOfSimplex(GenericList<Vector> simplex)
        {
            // Set up variables.
            double radius = 0;
            int n = simplex.Length - 1;
            Vector center = new Vector(n);

            // Compute size measure.
            for (int i = 0; i < n + 1; i++)
            {
                center += simplex[i];
            }
            center *= 1.0/(n + 1);
            for (int i = 0; i < n + 1; i++)
            {
                double length = Vector.Norm(center - simplex[i]);
                if (length > radius)
                {
                    radius = length;
                }
            }

            // Return size measure.
            return radius;
        }
    }
}