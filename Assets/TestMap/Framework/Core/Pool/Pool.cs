using System.Collections.Generic;

namespace Framework
{
    public abstract class Pool<T> : IPool<T>
    {
        #region ----字段----
        protected int maxCount = 10;
        protected IFactory<T> factory;
        protected readonly Stack<T> cacheStack = new Stack<T>();
        #endregion

        #region ----实现ICountable----
        int ICountable.IdleCount
        {
            get
            {
                return cacheStack.Count;
            }
        }

        int ICountable.MaxCount
        {
            get
            {
                return maxCount;
            }
        }
        #endregion

        #region ----实现IPool----
        public virtual T Get()
        {
            if (cacheStack.Count == 0)
            {
                return factory.Create();
            }

            return cacheStack.Pop();
        }

        public abstract bool Recycle(T obj);
        #endregion
    }
}

