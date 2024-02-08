using System.Collections.Generic;

// First implementation
public class genlist<T> {
	// Fields
	private T[] data;

	// Constructor
	public genlist() {data = new T[0];}
	public genlist(T[] data) {this.data = data;}

	// Methods
	public int size => this.data.Length;
	public T this[int i] => this.data[i];
	public void add(T item) {
		T[] newdata = new T[this.size+1];
		System.Array.Copy(this.data, newdata, this.size);
		newdata[this.size] = item;
		this.data = newdata;
	}
	public void remove(int i) {
		T[] newdata = new T[this.size - 1];
		for (int j=0; j<this.size; j++) {
			if (j!=i && j<i) {newdata[j] = this.data[j];}
			else if (j!=i && j>i) {newdata[j-1] = this.data[j];}
		}
		this.data = newdata;
	}
}

// Implementation using nodes
public class node<T> {
	// Fields
	public T item;
	public node<T> next;

	// Constructor
	public node(T item) {this.item = item;}
}

public class list<T> {
	// Fields
	public node<T> first=null, current=null;

	// Methods
	public void add(T item) {
		if (first==null) {
			first = new node<T>(item);
			current = first;
		}
		else {
			current.next = new node<T>(item);
			current = current.next;
		}
	}
	public void start() {current = first;}
	public void next() {current = current.next;}
}
