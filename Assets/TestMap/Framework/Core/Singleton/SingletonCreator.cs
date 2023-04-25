using System;
using System.Reflection;

namespace Framework
{
	public static class SingletonCreator
	{
		public static T Create<T>() where T : class, ISingleton
        {
			//获取所有构造函数
			ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
			//筛选无参数的
			ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
				throw new Exception("Non-Pulbic Constructor() Not Found in " + typeof(T));
			}

			return ctor.Invoke(null) as T;
		}
	}
}