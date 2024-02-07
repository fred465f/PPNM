using static System.Console;
using static System.Math;

public static class main {
	public static void Main() {
		Error.WriteLine("----- Part 2 -----");
		Error.WriteLine("x        sin(x)          cos(x)");
		char[] split_delimiters = {' ', '\t', '\n'};
		var split_options = System.StringSplitOptions.RemoveEmptyEntries;
		for (string line = ReadLine(); line!=null; line=ReadLine()) {
			var numbers = line.Split(split_delimiters, split_options);
			foreach (var number in numbers) {
				double x = double.Parse(number);
				Error.WriteLine($"{x} {Sin(x)}    {Cos(x)}");
			}
		}
	}
}
