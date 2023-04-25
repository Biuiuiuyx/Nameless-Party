using System;

namespace GameProject
{
	public static class Random
	{
		#region ----公有方法----
		/// <summary>
		/// 随机浮点数 [min, max] 
		/// </summary>
		public static float GetValue(float min, float max)
        {
			UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
			return UnityEngine.Random.Range(min, max);
        }

		/// <summary>
		/// 随机整数 [min, max)
		/// </summary>
		public static  int GetValue(int min, int max)
        {
			UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
			return UnityEngine.Random.Range(min, max);
        }

		/// <summary>
		/// [0f, 1f] 随机
		/// </summary>
		/// <returns></returns>
		public static float GetRange01()
        {
			return GetValue(0f, 1f);
        }
		#endregion
	}
}