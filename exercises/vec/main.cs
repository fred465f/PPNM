using static vec;
using static System.Math;
using static System.Console;

public static class main {
    public static void Main() {
		// Basis vectors
        vec e1 = new vec(1, 0, 0);
        vec e2 = new vec(0, 1, 0);
        vec e3 = new vec(0, 0, 1);

        // Check orthogonality
        if (!approx_double(vec.dot(e1, e2), 0)) WriteLine("Orthogonality failed!");
        else if (!approx_double(vec.dot(e1, e3), 0)) WriteLine("Orthogonality failed!");
        else if (!approx_double(vec.dot(e2, e3), 0)) WriteLine("Orthogonality failed!");
        else WriteLine("Orthogonality of standard basis vectors holds!");

        // Check -v is additive inverse of v
        if (!approx_double((e1 + (-e1)).norm(), 0)) WriteLine("- operator does not return additive inverse!");
        else if (!approx_double((e2 + (-e2)).norm(), 0)) WriteLine("- operator does not return additive inverse!");
        else if (!approx_double((e2 + (-e2)).norm(), 0)) WriteLine("- operator does not return additive inverse!");
        else WriteLine("- operator does return additive inverse for standard basis vectors!");

        // Check mult. with 0 yields 0 vector
        if (!approx_double((0*e2).norm(), 0)) WriteLine("Mult. with 0 did not behave as expected!");
        else if (!approx_double((0*e2).norm(), 0)) WriteLine("Mult. with 0 did not behave as expected!");
        else if (!approx_double((0*e2).norm(), 0)) WriteLine("Mult. with 0 did not behave as expected!");
        else WriteLine("Mult. with 0 yields 0 vector!");
    }

    static bool approx_double(double a, double b, double acc=1e-9, double err=1e-9) {
		if (Abs(a-b) < acc) return true;
		else if (Abs(a-b) < Max(Abs(a),Abs(b))*err) return true;
		else return false;
    }
}
