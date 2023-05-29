using GameProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户
/// </summary>
public class User
{
    public Camp Camp { get; private set; }                    // 用户id
    public string Name => Camp.Name();
    public int CurGridId { get; private set; }              // 当前所在格子id
    public int Gold { get; private set; }                   // 当前拥有金币
    public UserStep Step { get; private set; }              // 当前步骤
    public int StopRound { get; set; }
    public bool StepCompleted => chess.StepCompleted;
    public bool Reach { get; private set; } = false;

    private Dictionary<UserStep, Actionable> actions;       // 行为步骤集合
    private UserChess chess;

    public Action<int> onChangeGold;

    public UserChess Chess => chess;

    public User(Camp _camp, int _gridId, int _gold, UserChess _chess)
    {
        Camp = _camp;
        CurGridId = _gridId;
        Gold = _gold;
        chess = _chess;
        chess.Init(Map.Instance.GetGrid(_gridId).transform.position);
        // 初始化行为
        actions = new Dictionary<UserStep, Actionable>();
        actions.Add(UserStep.PlayDice, new PlayDiceAction(UserStep.PlayDice, Camp));
        actions.Add(UserStep.Move, new MoveAction(UserStep.Move, Camp));
        actions.Add(UserStep.Tip, new TipAction(UserStep.Tip, Camp));
    }
    
    /// <summary>
    /// 移动到格子id
    /// </summary>
    public void MoveToGrid(int _gid)
    {
        CurGridId = _gid;
    }

    /// <summary>
    /// 移动几步
    /// </summary>
    public void MoveStep(int _step)
    {
        if (_step > 0 )
        {
            // 剩余步数
            int remainStep = _step;
            Map m = Map.Instance;
            Grid g = m.GetGrid(CurGridId);
            List<Vector2> way = new List<Vector2>();
            while (remainStep > 0)
            {
                g = m.GetGrid(g.NextId);
                way.Add(g.transform.position);
                remainStep--;
                if (g.Type == GridType.Origin)
                {
                    chess.OnMove(way, g.GridId);
                    way.Clear();
                    chess.GetOriginGold();
                }else if (g.Type == GridType.Destination)
                {
                    Reach = true;
                    break;
                }
            }

            // 所有格子走完，现在g是最后一个位置
            if (way.Count > 0)
            {
                chess.OnMove(way, g.GridId);
            }
        }
    }

    /// <summary>
    /// 后退几步
    /// </summary>
    /// <param name="_step"></param>
    public void RetreatStep(int _step)
    {
        if (_step > 0)
        {
            // 剩余步数
            int remainStep = _step;
            Map m = Map.Instance;
            Grid g = m.GetGrid(CurGridId);
            List<Vector2> way = new List<Vector2>();
            while (remainStep > 0)
            {
                g = m.FindLast(g.GridId);
                way.Add(g.transform.position);
                remainStep--;
            }

            // 所有格子走完，现在g是最后一个位置
            if (way.Count > 0)
            {
                chess.OnMove(way, g.GridId);
            }
        }
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    public void AddGold(int _amount)
    {
        if (_amount > 0)
        {
            Gold += _amount;
            //FloatManager.Instance.ShowFloatStr($"+{_amount}", Map.Instance.GetGrid(CurGridId).Rect.anchoredPosition + new Vector2(0, 30), Color.green);
            onChangeGold?.Invoke(Gold);
        }
    }

    /// <summary>
    /// 金币是否足够
    /// </summary>
    public bool EnoughGold(int _amount)
    {
        return Gold >= _amount;
    }

    /// <summary>
    /// 花费金币
    /// </summary>
    public void SpendGold(int _amount)
    {
        if (_amount > 0)
        {
            Gold -= _amount;
            //FloatManager.Instance.ShowFloatStr($"-{_amount}", Map.Instance.GetGrid(CurGridId).Rect.anchoredPosition + new Vector2(0, 30), Color.red);
            onChangeGold?.Invoke(Gold);
        }
    }

    /// <summary>
    /// 下个步骤
    /// </summary>
    public void NextStep()
    {
        Step = (UserStep)((int)Step + 1);
    }

    /// <summary>
    /// 跳转到步骤
    /// </summary>
    public void ToStep(UserStep _step)
    {
        Step = _step;
    }

    /// <summary>
    /// 获取当前步骤的行为
    /// </summary>
    public Actionable GetActionable()
    {
        return actions[Step];
    }

    /// <summary>
    /// 获取指定步骤的行为
    /// </summary>
    public Actionable GetActionableByStep(UserStep _step)
    {
        actions.TryGetValue(_step, out var a);
        return a;
    }
}
