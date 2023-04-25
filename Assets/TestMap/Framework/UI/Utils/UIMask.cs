using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Framework
{
	public class UIMask : MonoBehaviour
	{
		#region ----字段----
		[SerializeField] private Image mask;

		private static UIMask instance;
		#endregion

		#region ----属性----
		public static UIMask Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(UIManager.Path + "UIMask"), GameObject.FindGameObjectWithTag("Canvas").transform.Find("Mask"));
					instance = go.GetComponent<UIMask>();
					go.SetActive(false);
				}

				return instance;
			}
		}
		#endregion

		#region ----公有方法----
		public void ShowMask(float fadeTime = 0.5f)
		{
			gameObject.SetActive(true);
			mask.DOFade(1, fadeTime);
		}

		public void HideMask(float fadeTime = 0.5f, Action onFinish = null)
		{
			mask.color = new Color(0, 0, 0, 1);
			mask.DOFade(0, fadeTime).OnComplete(() =>
			{
				gameObject.SetActive(false);
				onFinish?.Invoke();
			});
		}

		public void FadeInAndOut(Action action, float fadeTime = 0.5f, Action onFinish = null)
		{
			gameObject.SetActive(true);
			mask.color = new Color(0, 0, 0, 0);
			mask.DOFade(1, fadeTime).OnComplete(() =>
			{
				action?.Invoke();
				HideMask(fadeTime, onFinish);
			});
		}
		#endregion
	}
}