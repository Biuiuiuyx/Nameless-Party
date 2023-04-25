namespace Framework
{
    public interface IPoolable
    {
        /// <summary>
        /// 是否已经被回收
        /// </summary>
        bool IsRecycled { get; set; }

        /// <summary>
        /// 被回收时调用
        /// </summary>
        void OnRecycled();
    }
}
