using static System.Console;
using static System.Math;

public class vec {
	// Fields
	public double x, y, z;

	// Constructors
    public vec() {x=0; y=0; z=0;}
    public vec(double x, double y, double z) {this.x=x; this.y=y; this.z=z;}

    // Overload usefull operators
    public static vec operator+(vec v, vec u) {
        return new vec(v.x+u.x, v.y+u.y, v.z+u.z);
    }
    public static vec operator-(vec v, vec u) {
        return new vec(v.x-u.x, v.y-u.y, v.z-u.z);
    }
    public static vec operator*(vec v, double c) {
        return new vec(c*v.x, c*v.y, c*v.z);
    }
    public static vec operator*(double c, vec v) {
        return v*c;
    }
    public static vec operator-(vec u) {
        return new vec(-u.x, -u.y, -u.z);
    }

    // Print method for debugging
    public void print(string s) {
        Write(s);
        WriteLine($"{x} {y} {z}");
    }
    public void print() {
        this.print("");
    }

    // Dot product and norm
    public double dot(vec other) {
        return this.x*other.x + this.y*other.y + this.z*other.z;
    }
    public static double dot(vec v, vec u) {
        return v.x*u.x + v.y*u.y + v.z*u.z;
    }
    public double norm() {
		return Sqrt(this.dot(this));
    }

    // Comparison method
    static bool approx(double a, double b, double acc=1e-9, double err=1e-9) {
        if (Abs(a-b) < acc) return true;
        else if (Abs(a-b) < Max(Abs(a), Abs(b))*err) return true;
        else return false;
    }
    public bool approx(vec other) {
        if (!approx(this.x, other.x)) return false;
        else if (!approx(this.y, other.y)) return false;
        else if (!approx(this.z, other.z)) return false;
        else return true;
    }
    public static bool approx(vec u, vec v) => u.approx(v);

    // Override ToString method
    public override string ToString() {return $"{this.x} {this.y} {this.z}";}
}
