using GameProject;
using System.Collections;
using UnityEngine;


/// <summary>
/// 移动行为
/// </summary>
public class MoveAction : Actionable
{
    protected Camp user;

    public MoveAction(UserStep _step, Camp _camp) : base(_step)
    {
        user = _camp;
    }

    protected override void ToDoAction()
    {
        GameManager.Instance.StartCoroutine(WaitForMove());
    }

    private IEnumerator WaitForMove()
    {

        // 从地图获取摇到的点数
        int dice = Map.Instance.CurDice;
        // 获取当前操作的用户
        User u = Map.Instance.GetUser(user);
        if (u.StopRound > 0)
        {
            Finish();
            yield break;
        }
        // 移动点数的步数
        u.MoveStep(dice);

        // 等待棋子移动完毕
        yield return new WaitUntil(() => u.StepCompleted);
        // 行为完成
        Finish();
    }
}
