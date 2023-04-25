using System.Reflection;
using System;

namespace Framework
{
    public class NonPublicConstructorFactory<T> : IFactory<T> where T : class
    {
        public T Create()
        {
            ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

            if (ctor == null)
            {
                throw new Exception("Non-Pulbic Constructor() Not Found in " + typeof(T));
            }

            return ctor.Invoke(null) as T;
        }

    }
}

