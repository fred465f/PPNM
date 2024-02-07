using static System.Console;
using static System.Math;

public static class main {
	public static void Main(string[] args) {
		WriteLine("----- Part 1 -----");
		WriteLine("x		sin(x)			cos(x)");
		foreach (var arg in args) {
			var words = arg.Split(":");
			if (words[0]=="-numbers") {
				var numbers = words[1].Split(",");
				foreach (var number in numbers) {
					double x = double.Parse(number);
					WriteLine($"{x}	{Sin(x)}	{Cos(x)}");
				}
			}
		}
	}
}
