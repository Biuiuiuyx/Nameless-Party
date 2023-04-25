using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace Framework
{
	public class WarnView : MonoBehaviour
	{
		#region ----字段----
		private Text label;
		private Action onEnd;
		#endregion

		#region ----公有方法----
		public void ShowWarn(string text, float showTime = 2f, Action onEnd = null)
		{
			if (onEnd != null)
			{
				this.onEnd += onEnd;
			}
			if (!string.IsNullOrWhiteSpace(text) && showTime > 0)
			{
				label = GetComponent<Text>();
				gameObject.SetActive(true);
				label.text = text;
				Sequence seq = DOTween.Sequence();
				seq.Append(label.DOFade(1, 0.1f).From(0));
				seq.Append(label.transform.DOLocalMoveY(4, showTime));
				seq.Append(label.transform.DOLocalMoveY(20, 0.5f));
				//seq.AppendInterval(showTime);
				seq.Insert(showTime + 0.2f, label.DOFade(0, 0.5f));
				seq.AppendCallback(OnEnd);
			}
			else
			{
				OnEnd();
			}
		}
		#endregion

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
	}
}