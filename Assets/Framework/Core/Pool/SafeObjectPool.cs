using System;

namespace Framework
{
    /// <summary>
    /// 安全对象池
    /// </summary>
    public class SafeObjectPool<T> : Pool<T>, ISingleton where T : IPoolable, new()
    {

        private SafeObjectPool()
        {
            factory = new DefaultFactory<T>();
        }

        void ISingleton.OnInit() { }

        //获取实例的入口: 单例
        public static SafeObjectPool<T> Instance
        {
            get
            {
                return SingletonProperty<SafeObjectPool<T>>.Instance;
            }
        }

        public void Dispose()
        {
            SingletonProperty<SafeObjectPool<T>>.Dispose();
        }

        //初始化池子的d对象数量
        public void Init(int maxCount, int initCount = 0)
        {
            MaxCacheCount = maxCount;
            if (maxCount > 0)
            {
                initCount = Math.Min(maxCount, initCount);
            }

            if (initCount > cacheStack.Count)
            {
                //创建initCount个对象
                for (int i = 0; i < initCount; i++)
                {
                    cacheStack.Push(factory.Create());
                }
            }
        }

        public int MaxCacheCount
        {
            get { return maxCount; }
            set
            {
                maxCount = value;
                //当栈里的数量大于maxCount，删除
                if (cacheStack != null && maxCount > 0)
                {
                    if (maxCount < cacheStack.Count)
                    {
                        int removeCount = cacheStack.Count - maxCount;
                        for (int i = 0; i < removeCount; i++)
                        {
                            cacheStack.Pop();
                        }
                    }
                }
            }
        }

        //从池子里取出一个
        public override T Get()
        {
            T obj = base.Get();
            obj.IsRecycled = false;

            return obj;
        }

        //回收
        public override bool Recycle(T obj)
        {
            if (obj == null || obj.IsRecycled)
            {
                return false;
            }

            if (MaxCacheCount > 0)
            {
                if (cacheStack.Count >= MaxCacheCount)
                {
                    obj.OnRecycled();
                    return false;
                }
            }

            obj.IsRecycled = true;
            obj.OnRecycled();
            cacheStack.Push(obj);

            return true;
        }
    }
}
