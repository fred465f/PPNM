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
        private int _availableSpace;

        // Constructors.
        public GenericList()
        {
            _data = new T[1];
            _length = 1;
            _availableSpace = 1;
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
            if (_availableSpace != 0)
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
                _length += 1;
                _availableSpace = _length - 1;
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
    }
}