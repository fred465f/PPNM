/* Implements a generic list with basic features
and slight performance optimization. */

using System;

namespace DataStructures
{
    public class GenericList<T>
    {
        // Fields.
        private T[] _data; 
        private int _length;
        public int _availableSpace;

        // Constructors.
        public GenericList()
        {
            _data = new T[2];
            _length = 0;
            _availableSpace = 2;
        }
        public GenericList(T[] data)
        {
            _data = data;
            _length = data.Length;
            _availableSpace = 0;
        }

        // Getter and setter.
        public T this[int i]
        {
            get {return _data[i];}
            set {_data[i] = value;}
        }

        // Dimensional properties.
        public int Length => _length;

        // Method to add and element to generic list.
        public void Add(T item)
        {
            if (_availableSpace > 0)
            {
                _data[_length] = item;
                _availableSpace -= 1;
                _length += 1;
            }
            else
            {
                T[] newData = new T[2 * _length];
                Array.Copy(_data, newData, _length);
                newData[_length] = item;
                _data = newData;
                _availableSpace = _length - 1;
                _length += 1;
            }
        }

        // Method to remove element at specified index.
        public void Remove(int i)
        {
            T[] newData = new T[_length + _availableSpace];
            for (int j = 0; j < _length; j++)
            {
                if (j != i && j < i)
                {
                    newData[j] = _data[j];
                }
                else if (j != i && j > i)
                {
                    newData[j - 1] = _data[j];
                }
            }
            _data = newData;
            _length -= 1;
            _availableSpace += 1;
        }

        // Method copies content of GenericList instance into another instance.
        public GenericList<T> Copy()
        {
            GenericList<T> copy = new GenericList<T>();
            for (int i = 0; i < this.Length; i++)
            {
                copy.Add(this[i]);
            }
            return copy;
        }
    }
}