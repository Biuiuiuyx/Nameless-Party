using System;

namespace Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; private set; }

        public MonoSingletonPath(string path)
        {
            Path = path;
        }
    }
}
