using Framework;
using GameProject;
using System.Collections;
using UnityEngine;

/// <summary>
/// 提示行为
/// </summary>
public class TipAction : Actionable
{
    protected Camp user;        // 正在操作的用户

    public TipAction(UserStep _step, Camp _camp) : base(_step)
    {
        user = _camp;
    }

    protected override void ToDoAction()
    {
        GameManager.Instance.StartCoroutine(ShowTip());
    }

    private IEnumerator ShowTip()
    {
        yield return new WaitForSeconds(0.2f);
        // 从地图获取正在操作的用户
        User u = Map.Instance.GetUser(user);
        // 获取用户当前所在的格子
        Grid g = Map.Instance.GetGrid(u.CurGridId);
        if (g.Type == GridType.Back)
        {
            // 退格
            UIWarn.Instance.ShowWarn($"Take {g.Value} steps back", .5f);
            u.RetreatStep(g.Value);
            // 等待棋子移动完毕
            yield return new WaitUntil(() => u.StepCompleted);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Game1)
        {
            var p1 = UIManager.Instance.Open<RockPaperScissorsPanel>();
            yield return new WaitUntil(() => p1.Completed);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Game2)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{user.Name()}>触发了[{g.Type.Name()}]，制作中未完成"), () =>
            {
                Finish();
            });
            yield break;
        }
        else if (g.Type == GridType.Game3)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{user.Name()}>触发了[{g.Type.Name()}]，制作中未完成"), () =>
            {
                Finish();
            });
            yield break;
        }
        else if (g.Type == GridType.Game4)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{user.Name()}>触发了[{g.Type.Name()}]，制作中未完成"), () =>
            {
                Finish();
            });
            yield break;
        }
        else if (g.Type == GridType.Destination)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{user.Name()}>抵达了终点，制作中未完成"), () =>
            {
                Finish();
            });
            yield break;
        }
        else
        {
            Finish();
            yield break;
        }
    }
}
