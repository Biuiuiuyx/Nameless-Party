using System;

namespace Framework
{
    public class SimplePool<T> : Pool<T>
    {
        /// <summary>
        /// 回收时执行的方法委托
        /// </summary>
        protected Action<T> resetMethod;

        /// <summary>
        /// 把创建方法和回收方法都交给用户自定义
        /// </summary>
        /// <param name="fMethod">创建的方法</param>
        /// <param name="rMethod">回收的方法</param>
        /// <param name="initCount">初始化个数</param>
        public SimplePool(Func<T> fMethod, Action<T> rMethod = null,  int initCount = 0)
        {
            factory = new CustomFactory<T>(fMethod);
            resetMethod = rMethod;

            for (int i = 0; i < initCount; i++)
            {
                cacheStack.Push(factory.Create());
            }
        }

        /// <summary>
        /// 回收：将对象放回Stack并执行回收委托
        /// </summary>
        /// <param name="obj">回收的对象</param>
        public override bool Recycle(T obj)
        {
            resetMethod?.Invoke(obj);

            cacheStack.Push(obj);
            return true;
        }
    }
}
