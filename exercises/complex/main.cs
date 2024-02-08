using static System.Console;
using static System.Math;

public static class main {
	public static void Main() {
		// Part 1
		WriteLine("----- Part 1 -----");
		complex a1 = new complex(-1, 0);
		complex a2 = cmath.sqrt(a1);
		complex I = cmath.I;
		complex b1 = cmath.sqrt(I);
		complex c1 = cmath.exp(I);
		complex d1 = cmath.exp(I*PI);
		complex e1 = cmath.pow(I, I);
		complex f1 = cmath.log(I);
		complex g1 = cmath.sin(I*PI);
		WriteLine($"Sqrt of -1 is = {a2}, which is {(approx(a2, I)).ToString()}");
		WriteLine($"Sqrt of i is = {b1}, which is {(approx(b1, new complex(1/Sqrt(2), 1/Sqrt(2)))).ToString()}");
		WriteLine($"Exp of i is = {c1}, which is {(approx(c1, new complex(Cos(1), Sin(1)))).ToString()}");
		WriteLine($"Exp of i * pi is = {d1}, which is {(approx(d1, new complex(-1, 0))).ToString()}");
		WriteLine($"i to the power of i is = {e1}, which is {(approx(e1, new complex(Exp(-PI/2), 0))).ToString()}");
		WriteLine($"Log of i is = {f1}, which is {(approx(f1, new complex(0, PI/2))).ToString()}");
		WriteLine($"Sin of i is = {g1}, which is {(approx(g1, new complex(0, Sinh(PI)))).ToString()}");

		// Part 2
		WriteLine("----- Part 2 -----");
		WriteLine($"Cosh(i) = {cmath.cosh(I)}, which is  {(approx(cmath.cosh(I), new complex(Cos(1), 0))).ToString()}");
		WriteLine($"Cosh(i) = {cmath.sinh(I)}, which is  {(approx(cmath.sinh(I), new complex(0, Sin(1)))).ToString()}");
	}
	
	public static bool approx(complex x, complex y, double acc=1e-9, double err=1e-9) {
		if (!( (Abs(x.Re-y.Re)<acc) || (Abs(x.Re-y.Re)<Max(Abs(x.Re),Abs(y.Re))*err) )) return false;
		else if (!( (Abs(x.Im-y.Im)<acc) || (Abs(x.Im-y.Im)<Max(Abs(x.Im),Abs(y.Im))*err) )) return false;
		else return true;
	}
}
