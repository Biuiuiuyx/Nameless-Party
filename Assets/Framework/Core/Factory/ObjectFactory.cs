using System;
using System.Reflection;

namespace Framework
{
    public static class ObjectFactory
    {
        /// <summary>
        /// 创建t类的实例
        /// </summary>
        public static object Create(Type t, params object[] args)
        {
            return Activator.CreateInstance(t, args);
        }

        public static T Create<T>(params object[] args)
        {
            return (T)Create(typeof(T), args);
        }

        /// <summary>
        /// 通过调用t类私有无参构造方法创建实例
        /// </summary>
        public static object CreateNonPublicConstructor(Type t)
        {
            ConstructorInfo[] ctors = t.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if(ctor == null)
            {
                throw new Exception("Non-Public Constructor() Not Found in " + t);
            }

            return ctor.Invoke(null);
        }

        public static T CreateNonPublicConstructor<T>()
        {
            return (T) CreateNonPublicConstructor(typeof(T));
        }

        /// <summary>
        /// 创建t类实例后回调onInitial
        /// </summary>
        public static object CreateWithInitialAction(Type t, Action<object> onInitial, params object[] args)
        {
            object o = Create(t, args);
            onInitial(o);

            return o;
        }

        public static T CreateWithInitialAction<T>(Action<T> onInitial, params object[] args)
        {
            T t = Create<T>(args);
            onInitial(t);

            return t;
        }

    }
}
