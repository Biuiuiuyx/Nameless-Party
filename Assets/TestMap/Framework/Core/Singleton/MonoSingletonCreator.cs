using UnityEngine;

namespace Framework
{
    public static class MonoSingletonCreator
    {
        /// <summary>
        /// 创建Mono泛型单例的实例
        /// </summary>
        /// <typeparam name="T">单例的类型</typeparam>
        /// <returns></returns>
        public static T Create<T>() where T : MonoBehaviour, ISingleton
        {
            T instance = null;
            //检查单例实例是否已经存在
            instance = UnityEngine.Object.FindObjectOfType<T>();
            if (instance != null)
            {
                return instance;
            }

            //检查特性
            object[] attrs = typeof(T).GetCustomAttributes(true);
            foreach (object attr in attrs)
            {
                MonoSingletonPath atr = attr as MonoSingletonPath;
                if (atr == null)
                {
                    continue;
                }
                //根据路径创建实例
                instance = CreateComponent<T>(atr.Path, true);
                break;
            }

            if (instance == null)
            {
                instance = CreateComponent<T>(true);
            }

            return instance;
        }

        private static T CreateComponent<T>(string path, bool dontDestroy) where T : MonoBehaviour, ISingleton
        {
            //检查path
            if (string.IsNullOrEmpty(path))
            {
                return CreateComponent<T>(dontDestroy);
            }

            T instance = null;
            //检查是否已经存在
            GameObject go = GameObject.Find(path);
            if (go != null)
            {
                instance = go.GetComponent<T>();
                return instance;
            }

            string[] paths = path.Split('/');
            for (int i = 0; i < paths.Length; ++i)
            {
                go = FindGameObject(go, paths[i], true, true);
            }

            instance = go?.AddComponent<T>();

            return instance;
        }

        private static GameObject FindGameObject(GameObject root, string name, bool build, bool dontDestroy)
        {
            //检查name
            if (string.IsNullOrEmpty(name)) return null;

            GameObject go = null;
            //查找名字为name的GameObject
            if (root == null)
            {
                go = GameObject.Find(name);
            }
            else
            {
                Transform tsf = root.transform.Find(name);
                if (tsf != null) go = tsf.gameObject;
            }

            if (go == null)
            {
                if (build)
                {
                    go = new GameObject(name);

                    if (root != null)
                        go.transform.SetParent(root.transform);

                    if (dontDestroy)
                        Object.DontDestroyOnLoad(go.transform.root.gameObject);
                }
            }

            return go;
        }

        private static T CreateComponent<T>(bool dontDestroy) where T : MonoBehaviour, ISingleton
        {
            GameObject go = new GameObject(typeof(T).Name);
            if (dontDestroy)
            {
                Object.DontDestroyOnLoad(go.transform.root.gameObject);
            }

            return go.AddComponent<T>();
        }

    }
}
