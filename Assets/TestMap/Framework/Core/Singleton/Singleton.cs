namespace Framework
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T instance;
        private static readonly object lockObj = new object();

        /// <summary>
        /// 获取单例的实例
        /// </summary>
        public static T Instance
        {
            get
            {
                //保证多线程下安全
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = SingletonCreator.Create<T>();
                        instance.OnInit();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// 卸载单例
        /// </summary>
        public virtual void Dispose()
        {
            instance = null;
        }

        /// <summary>
        /// 单例实例创建完成
        /// </summary>
        public virtual void OnInit()
        {

        }

    }
}
