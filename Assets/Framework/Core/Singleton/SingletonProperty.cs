namespace Framework
{
	public static class SingletonProperty<T> where T : class, ISingleton
	{
		#region ----字段----
		private static T instance;
		private static readonly object lockObj = new object();
		#endregion

		#region ----属性----
		public static T Instance
        {
            get
            {
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
		#endregion

		#region ----公有方法----
		public static void Dispose() => instance = null;
		#endregion

	}
}