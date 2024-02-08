using static System.Console;

public static class main {
	public static int Main(string[] args) {
		// Check that generic class works as expected
		WriteLine("Initial quick test of generic class");
		genlist<int> list1 = new genlist<int>(new int[2] {1, 2});
		list1.add(3);
		list1.add(4);
		list1.remove(2);
		for (int i=0; i<3; i++) {
			WriteLine(list1[i]);
		}
		WriteLine("");

		// Instantiate new generic list
		genlist<double[]> list2 = new genlist<double[]>();
		
		// Load in data from input.txt
		string infile=null;
		var argssplit=args[0].Split(':');
		if (argssplit[0]=="-input") infile=argssplit[1];
		if (infile==null) {
			Error.WriteLine("Wrong filename argument");
			return 1;
		}
		char[] delimiters = {' ', '\t'};
		var options = System.StringSplitOptions.RemoveEmptyEntries;
		var instream = new System.IO.StreamReader(infile);
		for (string line=instream.ReadLine(); line!=null; line=instream.ReadLine()) {
			var words = line.Split(delimiters, options);
			var numbers = new double[words.Length];
			for (int i=0; i<words.Length; i++) {
				numbers[i] = double.Parse(words[i]);
			}
			list2.add(numbers);
		}

		// Print result in exponential form
		WriteLine("----- Part 1 -----");
		for (int i=0; i<list2.size; i++) {
			var numbers = list2[i];
			foreach (var number in numbers) {
				Write($"{number : 0.00e+00;-0.00e+00}");
			}
			WriteLine("");
		}

		// Check implementation using nodes works
		WriteLine("\n----- Part 4 -----");
		list<int> list3 = new list<int>();
		list3.add(1);
		list3.add(2);
		list3.add(3);
		for (list3.start(); list3.current!=null; list3.next()) {
			WriteLine(list3.current.item);
		}

		// Succesfull run
		return 0;
	}
}
