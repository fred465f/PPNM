using static System.Console;
using static System.Math;

public class vec {
	// Fields
	public double x, y, z;

	// Constructors
    public vec() {x=y=z=0;}
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

    // Dot products, vector products and norms
    public double dot(vec other) {
        return this.x*other.x + this.y*other.y + this.z*other.z;
    }
    public static double dot(vec v, vec u) {
        return v.x*u.x + v.y*u.y + v.z*u.z;
    }
    public double norm() {
        return Sqrt(vec.dot(this, this));
    }
}
