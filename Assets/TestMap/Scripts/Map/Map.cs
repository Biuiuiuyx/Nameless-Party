using Framework;
using GameProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rd = GameProject.Random;

/// <summary>
/// 地图
/// </summary>
public class Map : MonoBehaviour
{
    [SerializeField] Transform gridContainer;       // 格子容器
    [SerializeField] Grid origins;                  // 起点
	[SerializeField] Transform chessContainer;
	//[SerializeField] int goal = 3500;				// 胜利目标金额

    public int CurRound { get; private set; }       // 当前回合数
    public User CurUser { get; private set; }		// 当前回合操作的用户
	public int CurDice { get; private set; }

    private Dictionary<int, Grid> grids;            // 所有格子
	private Dictionary<Camp, User> users;           // 所有用户
	private int curId = 0;
	private int remainStep = 1;
	private IAudioPlayer bgm;
	private MainPanel panel;
	private static Transform container;

	public static Transform Container
    {
        get
        {
            if (container == null)
            {
				container = GameObject.FindGameObjectWithTag("Map").transform;
            }
			return container;
        }
    }

    private static Map instance;                    // 单例
    public static Map Instance => instance;

    public Action<int> onChangeRound;
	public Action<Camp> onCamp;

    private void Awake()
    {
        // 初始化所有的格子
        Grid[] gs = gridContainer.GetComponentsInChildren<Grid>();
        grids = new Dictionary<int, Grid>(gs.Length);
        foreach (var g in gs)
        {
            Debug.Assert(!grids.ContainsKey(g.GridId), $"{g.GridId}重复了!");
            grids.Add(g.GridId, g);
        }

        CurRound = 0;
        instance = this;

		// 初始化2个用户
		int count = 2;
		users = new Dictionary<Camp, User>(count);
		for (int i = 0; i < count; i++)
		{
			curId++;
			Camp camp = (Camp)curId;
			UserChess uc = Instantiate(Resources.Load<UserChess>($"User/{camp}"), chessContainer);
			uc.gameObject.SetActive(true);
			User u = new User(camp, origins.GridId, 3000, uc);
			users.Add(camp, u);
		}
	}

    private void Start()
    {
		panel = UIManager.Instance.Open<MainPanel>();
		StartCoroutine(StartRound());
    }

	public static void Load()
    {
		Unload();
		Instantiate(Resources.Load<Map>("Map/Map"), Container);
    }

	public static void Unload()
    {
		if (instance != null)
        {
			instance.panel.CloseSelf(true);
			instance.onChangeRound = null;
			instance.onCamp = null;
			instance = null;
			Destroy(instance.gameObject);
        }
    }

    /// <summary>
    /// 查询格子
    /// </summary>
    public Grid GetGrid(int _gid)
    {
        grids.TryGetValue(_gid, out var g);
        return g;
    }

	/// <summary>
	/// 查找上一个格子
	/// </summary>
	/// <param name="_gid"></param>
	/// <returns></returns>
	public Grid FindLast(int _gid)
    {
        foreach (var g in grids.Values)
        {
			if (g.NextId == _gid)
            {
				return g;
            }
        }
		return null;
    }

	/// <summary>
	/// 查询用户
	/// </summary>
	public User GetUser(Camp _camp)
    {
		users.TryGetValue(_camp, out var u);
		return u;
    }

	/// <summary>
	/// 摇骰子
	/// </summary>
	public int PlayDice(Camp _user)
    {
        if (CurUser.Camp != _user)
        {
			UIWarn.Instance.ShowWarn($"当前不是<{_user.Name()}>的回合");
			return 0;
        }

		CurDice = Rd.GetValue(1, 7);
		return CurDice;
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    private IEnumerator StartRound()
    {
		yield return new WaitForSeconds(.2f);
		bgm = AudioManager.Instance.Play("bg", 1, true);
		yield return new WaitForSeconds(1f);

		// 循环直到一方胜出结束
		User winner;
		while (!IsFinish(out winner))
		{
			CurRound++;
			onChangeRound?.Invoke(CurRound);
			// 每一回合遍历所有用户
			foreach (var u in users.Values)
			{
				CurUser = u;
				onCamp?.Invoke(u.Camp);
				// 当前操作方的剩余操作次数
				remainStep = 1;
				// 提示操作方
				CameraFollow.Instance.SetTarget(CurUser.Chess.transform);
				//Debug.Log($"<{u.Name}>的回合");
				while (remainStep > 0)
				{
					// 循环直到所有的步骤结束
					// 步骤是有顺序的，等待 -> 摇骰子 -> 移动 -> 完成，完成后重置为等待
					while (u.Step != UserStep.Completed)
					{
						if (u.StopRound <= 0)
                        {
							if (u.Step == UserStep.PlayDice || u.Step == UserStep.Move || u.Step == UserStep.Tip)
							{
								// 获取actionable，执行步骤
								var action = u.GetActionable();
								action.TakeTurn();
								// 等待步骤执行完成
								yield return new WaitUntil(() => action.Completed);
								// 步骤完成后重置步骤用于下一回合
								action.Reset();
							}

							// 切换下一个步骤
							u.NextStep();
                        }
                        else
                        {
							u.StopRound -= 1;
							u.ToStep(UserStep.Completed);
							yield return new WaitForSeconds(.4f);
                        }
						
					}
					// 操作完成，剩余次数减一
					remainStep--;
					if (remainStep > 0)
					{
						// 如果还有剩余次数，直接跳到等待
						u.ToStep(UserStep.Wait);
					}
				}
				// 该用户的回合结束
				// 重置到等待步骤
				u.ToStep(UserStep.Wait);

				// 检查是否结束
				if (IsFinish(out var w))
				{
					// 如果结束，这一回合后面的都不用操作了，跳出
					break;
				}
			}
		}

		yield return new WaitForSeconds(1);
		bgm.Stop();
		// 提示胜方
		UIManager.Instance.Open<GameOverPanel>().ShowWinner(winner);
	}

    /// <summary>
    /// 检查是否游戏结束
    /// </summary>
    /// <param name="winnerUserId">胜利玩家id</param>
    private bool IsFinish(out User winnerUser)
    {
        foreach (var u in users.Values)
        {
			// 暂定
            if (u.Reach)
            {
				winnerUser = u;
				return true;
            }
        }
		winnerUser = null;
        return false;
    }
}
