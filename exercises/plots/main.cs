using System;
using System.Linq;
using static System.Console;
using static System.Math;

public static class main {
	// Error function
	static double erf(double x) {
		if (x<0) return -erf(-x);
		double[] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};
		double t = 1/(1 + 0.3275911*x);
		double sum = t*(a[0] + t*(a[1] + t*(a[2] + t*(a[3] + t*a[4]))));
		return 1 - sum*Exp(-x*x);
	}

	// Gamma function
	static double gamma(double x) {
		if (x<0) return PI/Sin(PI*x)/gamma(1-x);
		else if (x<9) return gamma(1+x)/x;
		double lngamma = x*Log(x+1/(12*x-1/x/10)) - x + Log(2*PI/x)/2;
		return Exp(lngamma);
	}

	// Log of gamma function
	static double lngamma(double x) {
		if (x<=0) throw new ArgumentException("lngamma: x<=0");
		else if (x<9) return lngamma(x+1) - Log(x);
		return x*Log(x+1/(12*x-1/x/10)) - x + Log(2*PI/x)/2;
	}

	// Main function
	public static int Main(string[] args) {
		// Load output file names
		string[] outfiles = new string[0];
		foreach (var arg in args) {
			var words = arg.Split(":");
			if (words[0] == "-output") {
				outfiles = words[1].Split("/");
			}
			else {WriteLine("Wrong argument"); return 1;}
		}
		
		// Part 1
		int length = 5000;
		var xs_erf = Enumerable.Range(0, length).Select(x => -3 + (double)x*(3+3)/(length-1)).ToArray();
		var ys_erf = xs_erf.Select(x => erf(x)).ToArray();
		using (var outstream = new System.IO.StreamWriter(outfiles[0], append:false)) {
			for (int i=0; i<length; i++) {
				outstream.WriteLine($"{xs_erf[i]} {ys_erf[i]}");
			}
		}

		// Part 2
		var xs_gamma = Enumerable.Range(0, length).Select(x => -3.8 + (double)x*(3.8+3.8)/(length-1)).ToArray();
                var ys_gamma = xs_gamma.Select(x => gamma(x)).ToArray();
		using (var outstream = new System.IO.StreamWriter(outfiles[1], append:false)) {
			for (int i=0; i<length; i++) {
                                outstream.WriteLine($"{xs_gamma[i]} {ys_gamma[i]}");
                        }
                }

		// Part 3
		var xs_lngamma = Enumerable.Range(0, length).Select(x => 0.2 + (double)x*(3.8-0.2)/(length-1)).ToArray();
		var ys_lngamma = xs_lngamma.Select(x => lngamma(x)).ToArray();
		using (var outstream = new System.IO.StreamWriter(outfiles[2], append:false)) {
			for (int i=0; i<length; i++) {
                                outstream.WriteLine($"{xs_lngamma[i]} {ys_lngamma[i]}");
                        }
                }

		// Succesful program
		return 0;
	}
}
