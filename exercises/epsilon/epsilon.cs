using static System.Console;

class epsilon {
	static void Main() {
		// Part 1
		WriteLine("----- Part 1 -----");

		int i = 1;
		while(i<i+1) {
			i++;
		}
		WriteLine($"My max int is = {i}");

		int j = 1;
		while(j>j-1) {
			j--;
		}
		WriteLine($"My min int is = {j}");
		

		// Part 2
		WriteLine("\n----- Part 2 -----");

		double x = 1;
		while(x+1 != 1) {x /= 2;} x *= 2;
		WriteLine($"Machine epsilon for double is approx. = {x} and should be {System.Math.Pow(2, -52)}");

		float y = 1F;
		while((float)1F+y != 1) {y /= 2F;} y *= 2F;
		WriteLine($"Machine epsilon for float is approx. = {y} and should be {System.Math.Pow(2, -23)}");

		
		// Part 3
		WriteLine("\n----- Part 3 -----");

		int n = (int) 1e6;
		double epsilon = System.Math.Pow(2,-52);
		double tiny = epsilon/2;
		double sum_A = 0, sum_B = 0;
		sum_A += 1; for(int k = 0; k < n; k++) {sum_A += tiny;}
		for(int k = 0; k < n; k++) {sum_B += tiny;} sum_B += 1;
		WriteLine($"Sum A = {sum_A}, \nSum B = {sum_B}, \nDifference is = {sum_A - sum_B}");


		// Part 4
		WriteLine("\n----- Part 4 -----");

		double d1 = 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1;
		double d2 = 8 * 0.1;

		bool same = approx(d1, d2);
		if (same == true) {
			WriteLine("They are equal!");
		}
		else {
			WriteLine("They are not equal!");
		}
	}

	public static bool approx(double a, double b, double acc = 1e-9, double err = 1e-9) {
		if (System.Math.Abs(a - b) <= acc) {
			return true;
		}
		else if (System.Math.Abs(a - b) <= System.Math.Max(System.Math.Abs(a), System.Math.Abs(b))) {
			return true;
		}
		else {
			return false;
		}
	}
}
