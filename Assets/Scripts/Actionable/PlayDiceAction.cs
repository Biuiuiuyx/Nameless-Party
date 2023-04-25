using Framework;
using GameProject;
using System.Collections;
using UnityEngine;

/// <summary>
/// 摇骰子行为
/// </summary>
public class PlayDiceAction : Actionable
{
    protected Camp user;            // 正在操作的用户

    public PlayDiceAction(UserStep _step, Camp _user) : base(_step)
    {
        user = _user;
    }

    protected override void ToDoAction()
    {
        GameManager.Instance.StartCoroutine(PlayDice());
    }

    private IEnumerator PlayDice()
    {
        yield return new WaitForSeconds(1f);
        // 从棋盘获取摇到的骰子点数
        int dice = Map.Instance.PlayDice(user);
        //Debug.Log($"<{user.Name()}> 随机到{dice}");
        // 打开骰子界面播放动画
        RandomPanel rp = UIManager.Instance.Open<RandomPanel>();
        // 设置最终显示的点数
        rp.SetDice(dice);
        rp.SetCamp(user);
        // 等待播放完成，行为设置为完成
        yield return new WaitUntil(() => rp.Completed);
        // 关闭摇骰子界面
        UIManager.Instance.Close(nameof(RandomPanel), true);
        yield return new WaitForSeconds(0.5f);
        // 行为完成
        Completed = true;
    }
}
