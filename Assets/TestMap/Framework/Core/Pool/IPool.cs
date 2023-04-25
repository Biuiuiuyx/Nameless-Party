namespace Framework
{
    public interface IPool<T> : ICountable
    {
        /// <summary>
        /// 从池子里取出
        /// </summary>
        T Get();

        /// <summary>
        /// 回收到池子里
        /// </summary>
        /// <param name="obj">回收的对象</param>
        /// <returns></returns>
        bool Recycle(T obj);
    }
}


