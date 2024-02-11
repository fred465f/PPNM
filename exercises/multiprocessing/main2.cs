using System.Threading;
using System.Threading.Tasks;
using static System.Console;

public static class main {
	public static void Main(string[] args) {
		// Take to arg from cml
                int nterms = (int) 1e8;
                foreach (var arg in args) {
                        var words = arg.Split(":");
                        if (words[0]=="-terms") nterms = int.Parse(words[1]);
                }

                // Compute using dotnet parallel forloops
                double totalsum2 = 0;
                Parallel.For(1, nterms+1, delegate(int i) {totalsum2+=1.0/i;});
                WriteLine("----- Using .net parallel loops -----");
	}
}
