using System;

namespace OriNoco
{
    public static class OriNocoUtil
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = b;
            b = a;
            a = temp;
        }

        public static int ExpandArray<T>(ref T[] oldArray, int minSize = 1)
        {
            // TODO: Revisit this using next-largest primes like built-in lists do
            int newCapacity = Math.Max(oldArray.Length * 2, minSize);
            T[] newArray = new T[newCapacity];
            Array.Copy(oldArray, newArray, oldArray.Length);
            oldArray = newArray;
            return newCapacity;
        }

        public static bool Filter_StaticOnly(OriNocoBody body)
        {
            return body.IsStatic;
        }

        public static bool Filter_DynamicOnly(OriNocoBody body)
        {
            return (body.IsStatic == false);
        }

        public static bool Filter_All(OriNocoBody body)
        {
            return true;
        }
    }
}