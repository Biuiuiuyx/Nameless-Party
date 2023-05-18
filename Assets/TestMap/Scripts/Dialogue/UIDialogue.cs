using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Framework;

namespace GameProject
{
	/// <summary>
	/// 对话视图界面
	/// </summary>
	public class UIDialogue : MonoBehaviour
	{
		#region ----字段----
		[SerializeField] private Text nameLabel;
		[SerializeField] private Text dlg;
		[SerializeField] private float textSpeed = 0.02f;
		[SerializeField] private Button nextBtn;

		private bool finish;
		private string text;
		private Action onFinish;

		private static UIDialogue instance;
		#endregion

		#region ----属性----
		public static UIDialogue Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = GameObject.Instantiate(Resources.Load<GameObject>(UIManager.Path + "UIDialogue"), GameObject.FindGameObjectWithTag("Canvas").transform.Find("Dialogue"));
					instance = go.GetComponent<UIDialogue>();
					//instance.nextBtn.onClick.AddListener(instance.ClickNextBtn);
					go.SetActive(false);
				}

				return instance;
			}
		}
		#endregion

		#region ----公有方法----
		public void ShowDialogue(string _name, string _content, Action onFinish = null)
		{
			finish = false;
			nameLabel.text = _name;
			text = _content;
            if (onFinish != null)
            {
				this.onFinish += onFinish;
			}
			nextBtn.gameObject.SetActive(false);
			gameObject.SetActive(true);
			StartCoroutine(ScrollingText());
			//Debug.Log($"对话{id}-{lines.Length}-{curIndex}-{hasName}");
		}
        #endregion

        #region ----私有方法----
        private void Update()
        {
            if (finish && Input.anyKeyDown)
            {
				ClickNextBtn();
            }
        }

        private IEnumerator ScrollingText()
		{
			dlg.text = "";
			foreach (char letter in text.ToCharArray())
			{
				dlg.text += letter;
				yield return new WaitForSeconds(textSpeed);
			}
			yield return new WaitForSeconds(0.1f);
			nextBtn.gameObject.SetActive(true);
			finish = true;
		}

		private void ClickNextBtn()
        {
			finish = false;
			gameObject.SetActive(false);
			onFinish?.Invoke();
			onFinish = null;
			DialogueManager.Instance.Next();
        }
		#endregion

	}
}