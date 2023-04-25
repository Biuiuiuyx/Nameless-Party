using UnityEngine;

namespace Framework
{
	public static class MonoSingletonProperty<T> where T : MonoBehaviour, ISingleton
	{
		#region ----字段----
		private static T instance;
		#endregion

		#region ----属性----
		public static T Instance
        {
            get
            {
                if (instance == null)
                {
					instance = MonoSingletonCreator.Create<T>();
					instance.OnInit();
                }

				return instance;
            }
        }
		#endregion

		#region ----公有方法----
		public static void Dispose()
        {
			Object.Destroy(instance.gameObject);
			instance = null;
        }
		#endregion

	}
}