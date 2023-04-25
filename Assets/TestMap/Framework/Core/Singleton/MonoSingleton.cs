using UnityEngine;

namespace Framework
{
    public abstract class MonoSingleton<T> : MonoBehaviour, ISingleton where T : MonoSingleton<T>
    {
        private static T instance;
        private static bool isPlaying = true;
        /// <summary>
        /// 获取单例的实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (isPlaying && instance == null)
                {
                    instance = MonoSingletonCreator.Create<T>();
                    instance.OnInit();
                }

                return instance;
            }
        }

        /// <summary>
        /// 卸载单例
        /// </summary>
        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// 单例创建完成
        /// </summary>
        public virtual void OnInit()
        {

        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            isPlaying = false;
        }
    }
}
