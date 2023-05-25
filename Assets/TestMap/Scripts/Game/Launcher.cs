using UnityEngine;
using Framework;

namespace GameProject
{
	/// <summary>
	/// 启动入口
	/// </summary>
	public class Launcher : MonoBehaviour
	{
		#region ----MonoBehaviour----
		void Awake()
		{
            //初始化游戏逻辑模块
            var gameMgr = GameManager.Instance;
			var ui = UIManager.Instance;
			var fMgr = FloatManager.Instance;

			//开始游戏逻辑
			StartGame();
		}
		#endregion

		#region ----私有方法----
		private void StartGame()
		{
			// 打开开始界面
			UIManager.Instance.Open<StartPanel>();
			//Map.Load();
		}
		#endregion
	}
}