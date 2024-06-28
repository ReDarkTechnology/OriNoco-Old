using System.Collections.Generic;

namespace OriNoco
{
    public interface IOriNocoPool<T>
    {
        T Allocate();
        void Deallocate(T obj);
        IOriNocoPool<T> Clone();
    }

    public class OriNocoPool
    {
        public static void Free<T>(T obj)
          where T : IOriNocoPoolable<T>
        {
            obj.Pool.Deallocate(obj);
        }

        public static void SafeReplace<T>(ref T destination, T obj)
          where T : IOriNocoPoolable<T>
        {
            if (destination != null)
                OriNocoPool.Free(destination);
            destination = obj;
        }

        public static void DrainQueue<T>(Queue<T> queue)
          where T : IOriNocoPoolable<T>
        {
            while (queue.Count > 0)
                OriNocoPool.Free(queue.Dequeue());
        }
    }

    internal abstract class VoltPoolBase<T> : IOriNocoPool<T>
      where T : IOriNocoPoolable<T>
    {
        private readonly Stack<T> freeList;

        public abstract IOriNocoPool<T> Clone();
        protected abstract T Create();

        public VoltPoolBase()
        {
            this.freeList = new Stack<T>();
        }

        public T Allocate()
        {
            T obj;
            if (this.freeList.Count > 0)
                obj = this.freeList.Pop();
            else
                obj = this.Create();

            obj.Pool = this;
            obj.Reset();
            return obj;
        }

        public void Deallocate(T obj)
        {
            OriNocoDebug.Assert(obj.Pool == this);

            obj.Reset();
            obj.Pool = null; // Prevent multiple frees
            this.freeList.Push(obj);
        }
    }

    internal class OriNocoPool<T> : VoltPoolBase<T>
      where T : IOriNocoPoolable<T>, new()
    {
        protected override T Create()
        {
            return new T();
        }

        public override IOriNocoPool<T> Clone()
        {
            return new OriNocoPool<T>();
        }
    }

    internal class VoltPool<TBase, TDerived> : VoltPoolBase<TBase>
      where TBase : IOriNocoPoolable<TBase>
      where TDerived : TBase, new()
    {
        protected override TBase Create()
        {
            return new TDerived();
        }

        public override IOriNocoPool<TBase> Clone()
        {
            return new VoltPool<TBase, TDerived>();
        }
    }
}