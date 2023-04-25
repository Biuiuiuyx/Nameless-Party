using UnityEngine;
using UnityEngine.UI;
using System;

namespace Framework
{
	public class WarnPanel : MonoBehaviour
	{
		#region ----字段----
		[SerializeField] private Text label;
		[SerializeField] private Transform panel;
		private Action onEnd;
		#endregion

		#region ----公有方法----
		public void ShowWarn(string text, float showTime = 2.5f, Action onEnd = null)
        {
			if (onEnd != null)
            {
				this.onEnd += onEnd;
            }
            if (!string.IsNullOrWhiteSpace(text) && showTime > 0)
            {
				//panel.localScale = Vector3.one * 0.2f;
				label.text = text;
				gameObject.SetActive(true);
				Invoke("OnEnd", showTime);
            }
            else
            {
				OnEnd();
            }
        }
		#endregion

		#region ----私有方法----
		private void DoEnd()
        {
			onEnd?.Invoke();
			onEnd = null;
        }

		private void OnEnd()
        {
			DoEnd();
			Destroy(gameObject);
        }
		#endregion
	}
}