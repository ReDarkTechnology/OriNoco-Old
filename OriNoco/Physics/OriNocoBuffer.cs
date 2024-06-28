using System;
using System.Collections;
using System.Collections.Generic;

namespace OriNoco
{
    public class OriNocoBuffer<T> : IEnumerable<T>
    {
        public int Count { get { return this.count; } }
        public T this[int key] { get { return this.items[key]; } }

        private T[] items;
        private int count;

        public OriNocoBuffer(int capacity = 256)
        {
            this.items = new T[capacity];
            this.count = 0;
        }

        /// <summary>
        /// Adds a new element to the end of the list. Returns the index of the
        /// newly-indexed object.
        /// </summary>
        internal void Add(T body)
        {
            if (this.count >= this.items.Length)
                OriNocoUtil.ExpandArray(ref this.items);

            this.items[this.count] = body;
            this.count++;
        }

        internal void Add(T[] bodies, int count)
        {
            if ((this.count + count) >= this.items.Length)
                OriNocoUtil.ExpandArray(ref this.items, (this.count + count));

            Array.Copy(bodies, 0, this.items, this.count, count);
            this.count += count;
        }

        public void Clear()
        {
            this.count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
                yield return this.items[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < this.count; i++)
                yield return this.items[i];
        }
    }
}