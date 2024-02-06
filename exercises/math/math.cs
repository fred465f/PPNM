class math {
	static void Main() {
		// Compute sqrt of 2
		double x_1 = System.Math.Sqrt(2);
		System.Console.WriteLine($"Sqrt(2)^2 = {x_1 * x_1}");

		// Compute 2 to power of 1/5
		double x_2 = System.Math.Pow(2, (double) 1/5);
		System.Console.WriteLine($"(2^(1/5))^5 = {System.Math.Pow(x_2, 5)}");

		// Compute exp of pi
		double x_3 = System.Math.Exp(System.Math.PI);
		System.Console.WriteLine($"ln(exp(pi)) = {System.Math.Log(x_3)}");

		// Compute pi to power of e
		double x_4 = System.Math.Pow(System.Math.PI, System.Math.Exp(1));
		System.Console.WriteLine($"(pi^e)^(1/e) = {System.Math.Pow(x_4, (double) 1/System.Math.Exp(1))}");

		// Compute gamma function for different inputs
		double x = 1;
		for (int i = 0; i < 10; i++)
		{
			System.Console.WriteLine($"gamma({x}) = {sfuns.fgamma(x)}");
			x++;
		}
	}
}
