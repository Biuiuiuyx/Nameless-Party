using GameProject;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
	public class UIWarn : UIBase
	{
		#region ----字段----
		[SerializeField] private GameObject label;
		[SerializeField] private WarnPanel panel;
		[SerializeField] private Transform textParent;
		[SerializeField] private Transform panelParent;

		private static UIWarn instance;
		private Queue<TextView> textQueue;
		private Queue<TextView> panelQueue;
		public const int TextShowCount = 1;
		public const int PanelShowCount = 1;
		private int curTextCount;
		private int curPanelCount;
		#endregion

		#region ----属性----
		public static UIWarn Instance
		{
			get
			{
				if (instance == null)
				{
					instance = UIManager.Instance.Open<UIWarn>();
					instance.textQueue = new Queue<TextView>();
					instance.panelQueue = new Queue<TextView>();
					instance.curTextCount = 0;
					instance.curPanelCount = 0;
				}

				return instance;
			}
		}

		public override UILayer Layer => UILayer.Warn;
        #endregion

        #region ----内部类----
        protected struct TextView
		{
			public string Text { get; private set; }
			public float ShowTime { get; private set; }
			public TextView(string text, float showTime)
			{
				Text = text;
				ShowTime = showTime;
			}
		}
		#endregion

		#region ----公有方法----
		public void ShowWarn(string text, float showTime = 1.5f)
		{
			textQueue.Enqueue(new TextView(text, showTime));
			if (curTextCount < TextShowCount)
			{
				TryNext();
			}
		}

		public void ShowWarnPanel(string text, float showTime = 1.5f)
        {
			panelQueue.Enqueue(new TextView(text, showTime));
            if (curPanelCount < PanelShowCount)
            {
				TryNextPanel();
            }
        }
		#endregion

		#region ----私有方法----
		private void TryNext()
		{
			if (textQueue.Count > 0)
			{
				curTextCount++;
				gameObject.SetActive(true);
				WarnView mv = Create();
				TextView tv = textQueue.Dequeue();
				mv.ShowWarn(tv.Text, tv.ShowTime, OnEnd);
			}
		}

		private void TryNextPanel()
        {
            if (panelQueue.Count > 0)
            {
				curPanelCount++;
				gameObject.SetActive(true);
				WarnPanel wp = CreatePanel();
				TextView tv = panelQueue.Dequeue();
				wp.ShowWarn(tv.Text, tv.ShowTime, OnPanelEnd);
            }
        }

		private void OnEnd()
		{
			curTextCount--;
			TryNext();
		}

		private void OnPanelEnd()
        {
			curPanelCount--;
			TryNextPanel();
        }

		private WarnView Create()
		{
			GameObject go = GameObject.Instantiate(label, textParent, false);
			return go.GetComponent<WarnView>();
		}

		private WarnPanel CreatePanel()
        {
			WarnPanel wp = GameObject.Instantiate(panel, panelParent, false);
			return wp;
        }
		#endregion
	}
}