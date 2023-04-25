using System;
using Framework;

namespace GameProject
{
	// 对话事件
	public class DialogueAction : IPoolable, IRecyclable
	{
		#region ----字段----
		private Action action;
		#endregion

		#region ----公有方法----
		public static DialogueAction Get(Action action)
        {
			var da = SafeObjectPool<DialogueAction>.Instance.Get();
			da.RegisterAction(action);

			return da;
        }

		public void RegisterAction(Action action)
		{
			if (action != null)
			{
				this.action += action;
			}
		}

		public void DoAction()
		{
			action?.Invoke();
			Recycle();
		}
		#endregion

		#region ----实现IPoolable----
		public bool IsRecycled { get; set; }
		public void OnRecycled()
		{
			action = null;
		}

		public void Recycle()
		{
			SafeObjectPool<DialogueAction>.Instance.Recycle(this);
		}
		#endregion

	}
}