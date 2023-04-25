using System;

namespace Framework
{
    /// <summary>
    /// 自定义创建方法的工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomFactory<T> : IFactory<T>
    {
        /// <summary>
        /// 自定义的创建实例的方法
        /// </summary>
        private Func<T> mMethod;

        public CustomFactory(Func<T> mFactoryMethod)
        {
            mMethod = mFactoryMethod;
        }

        public T Create()
        {
            return mMethod();
        }
    }
}
