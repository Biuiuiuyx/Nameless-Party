using System;
using Framework;

namespace GameProject
{
    /// <summary>
    /// 对话管理器
    /// </summary>
    [MonoSingletonPath("Singleton/DialogueManager")]
    public class DialogueManager : MonoSingleton<DialogueManager>
    {
        #region ----字段----
        private DialogueData[] curDialogues;
        private Action onCompleted;
        private int curIndex;
        #endregion

        #region ----公有方法----
        /// <summary>
        /// 显示多条对话
        /// </summary>
        public void ShowDialogue(DialogueData[] dialogues, Action onCompleted)
        {
            curDialogues = dialogues;
            if (onCompleted != null)
            {
                this.onCompleted += onCompleted;
            }
            curIndex = 0;
            LoadDialogue();
            //UIManager.Instance.Close("MainPanel");
        }

        /// <summary>
        /// 显示一条对话
        /// </summary>
        public void ShowDialogue(DialogueData dialogue, Action onCompleted)
        {
            DialogueData[] ds = new DialogueData[] { dialogue};
            ShowDialogue(ds, onCompleted);
        }

        /// <summary>
        /// 加载当前的对话
        /// </summary>
        public void LoadDialogue()
        {
            DialogueData dia = curDialogues[curIndex];
            string[] nt = dia.Text.Split('-');
            if (nt.Length > 1)
            {
                UIDialogue.Instance.ShowDialogue(nt[0], nt[1]);
            }
            else
            {
                UIDialogue.Instance.ShowDialogue("", nt[0]);
            }
        }

        /// <summary>
        /// 下一步
        /// </summary>
        public void Next()
        {
            curIndex++;
            if (curIndex < curDialogues.Length)
            {
                LoadDialogue();
            }
            else
            {
                Action temp = onCompleted;
                onCompleted = null;
                temp?.Invoke();
            }
        }
        #endregion
    }
}