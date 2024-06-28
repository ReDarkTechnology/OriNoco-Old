using System.Collections;
using System.Collections.Generic;

namespace OriNoco
{
    /// <summary>
    /// A very loose partial encapsulation of a list array. Supports fast item
    /// at end, and fast arbitrary element removal. Does not guarantee order.
    /// </summary>
    internal class CheapList<T> : IEnumerable<T>
      where T : class, IIndexedValue
    {
        public int Count { get { return this.count; } }
        public T this[int key] { get { return this.values[key]; } }

        private T[] values;
        private int count;

        public CheapList(int capacity = 10)
        {
            this.values = new T[capacity];
            this.count = 0;
        }

        /// <summary>
        /// Adds a new element to the end of the list. Returns the index of the
        /// newly-indexed object.
        /// </summary>
        public void Add(T value)
        {
            if (this.count >= this.values.Length)
                OriNocoUtil.ExpandArray(ref this.values);

            this.values[this.count] = value;
            value.Index = this.count;
            this.count++;
        }

        /// <summary>
        /// Removes the element by swapping it for the last element in the list.
        /// </summary>
        public void Remove(T value)
        {
            int index = value.Index;
            OriNocoDebug.Assert(index >= 0);
            OriNocoDebug.Assert(index < this.count);

            int lastIndex = this.count - 1;
            if (index < lastIndex)
            {
                T lastValue = this.values[lastIndex];

                this.values[lastIndex].Index = -1;
                this.values[lastIndex] = null;

                this.values[index] = lastValue;
                lastValue.Index = index;
            }

            this.count--;
        }

        public void Clear()
        {
            this.count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
                yield return this.values[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
                yield return this.values[i];
        }
    }
}