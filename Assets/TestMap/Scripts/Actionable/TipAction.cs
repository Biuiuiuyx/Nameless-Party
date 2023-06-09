﻿using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using Random = GameProject.Random;

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
            yield return new WaitForSeconds(.5f);
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
            var p1 = UIManager.Instance.Open<ButtonRacePanel>();
            yield return new WaitUntil(() => p1.Completed);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Game3)
        {
            var p1 = UIManager.Instance.Open<ReactionSpeedPanel>();
            yield return new WaitUntil(() => p1.Completed);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Game4)
        {
            var p1 = UIManager.Instance.Open<StretchPanel>();
            yield return new WaitUntil(() => p1.Completed);
            Finish();
            yield break;
        }else if (g.Type == GridType.Game)
        {
            int r = Random.GetValue(0, 9);
            IGameCompleted p;
            switch (r)
            {
                case 0:
                    p = UIManager.Instance.Open<RockPaperScissorsPanel>();
                    break;
                case 1:
                    p = UIManager.Instance.Open<ButtonRacePanel>();
                    break;
                case 2:
                    p = UIManager.Instance.Open<ReactionSpeedPanel>();
                    break;
                case 3:
                    p = UIManager.Instance.Open<StretchPanel>();
                    break;
                case 4:
                    p = UIManager.Instance.Open<ReactionSpeed2Panel>();
                    break;
                case 5:
                    p = UIManager.Instance.Open<AnswerPanel>();
                    break;
                case 6:
                    p = UIManager.Instance.Open<DodgePanel>();
                    break;
                case 7:
                    p = UIManager.Instance.Open<ArrowPanel>();
                    break;
                case 8:
                    p = UIManager.Instance.Open<PokerPanel>();
                    break;
                default:
                    p = UIManager.Instance.Open<DodgePanel>();
                    break;
            }
            //p = UIManager.Instance.Open<ArrowPanel>();
            yield return new WaitUntil(() => p.Completed);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Destination)
        {
            //DialogueManager.Instance.ShowDialogue(new DialogueData($"<{user.Name()}>抵达了终点，制作中未完成"), () =>
            //{
            //    Finish();
            //});
            Finish();
            yield break;
        }
        else if (g.Type == GridType.Prop)
        {
            var p = UIManager.Instance.Open<PropPanel>();
            yield return new WaitUntil(() => p.Completed);
            Finish();
            yield break;
        }
        else if (g.Type == GridType.RandGame)
        {
            var p = UIManager.Instance.Open<RandGamePanel>();
            yield return new WaitUntil(() => p.Completed);
            Finish();
            yield break;
        }
        else
        {
            Finish();
            yield break;
        }
    }
}
