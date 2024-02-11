using System.Threading;
using System.Threading.Tasks;
using static System.Console;

public static class main {
	// Data class
	public class data {public int a, b; public double sum;}

	// Harmonic sum function
	public static void harmonicsum(object obj) {
		data local = (data) obj;
		local.sum = 0.0;
		for (int i=local.a; i<local.b; i++) local.sum+=1.0/i;
	}

	// Main function
	public static void Main(string[] args) {
		// Take to args from cml
		int nthreads=1, nterms=(int)1e8;
		foreach (var arg in args) {
			var words = arg.Split(":");
			if (words[0]=="-threads") nthreads = int.Parse(words[1]);
			else if (words[0]=="-terms") nterms = int.Parse(words[1]);
		}
		WriteLine($"----- Using {nthreads} threads -----");

		// Initialize data
		data[] x = new data[nthreads];
		for (int i=0; i<nthreads; i++) {
			x[i] = new data();
			x[i].a = 1 + nterms/nthreads*i;
			x[i].b = 1 + nterms/nthreads*(i+1);
		}
		x[x.Length-1].b = nterms + 1;

		// Prepare threads
		var threads = new Thread[nthreads];
		for (int i=0; i<nthreads; i++) {
			threads[i] = new Thread(harmonicsum);
			threads[i].Start(x[i]);
		}

		// Join threads
		for (int i=0; i<nthreads; i++) threads[i].Join();

		// Compute total sum
		double totalsum = 0;
		for (int i=0; i<nthreads; i++) totalsum += x[i].sum;
	}
}
