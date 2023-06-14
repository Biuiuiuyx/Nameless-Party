using Framework;
using GameProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rd = GameProject.Random;

/// <summary>
/// ��ͼ
/// </summary>
public class Map : MonoBehaviour
{
    [SerializeField] Transform gridContainer;       // ��������
    [SerializeField] Grid origins;                  // ���
	[SerializeField] Transform chessContainer;
	//[SerializeField] int goal = 3500;				// ʤ��Ŀ����

    public int CurRound { get; private set; }       // ��ǰ�غ���
    public User CurUser { get; private set; }		// ��ǰ�غϲ������û�
	public int CurDice { get; private set; }

    private Dictionary<int, Grid> grids;            // ���и���
	private Dictionary<Camp, User> users;           // �����û�
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

    private static Map instance;                    // ����
    public static Map Instance => instance;

    public Action<int> onChangeRound;
	public Action<Camp> onCamp;

    private void Awake()
    {
        // ��ʼ�����еĸ���
        Grid[] gs = gridContainer.GetComponentsInChildren<Grid>();
        grids = new Dictionary<int, Grid>(gs.Length);
        foreach (var g in gs)
        {
            Debug.Assert(!grids.ContainsKey(g.GridId), $"{g.GridId}�ظ���!");
            grids.Add(g.GridId, g);
        }

        CurRound = 0;
        instance = this;

		// ��ʼ��2���û�
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
		CameraFollow.Instance.SetTarget(users[Camp.Player1].Chess.transform);
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
			Destroy(instance.gameObject);
			instance = null;
        }
    }

    /// <summary>
    /// ��ѯ����
    /// </summary>
    public Grid GetGrid(int _gid)
    {
        grids.TryGetValue(_gid, out var g);
        return g;
    }

	/// <summary>
	/// ������һ������
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
	/// ��ѯ�û�
	/// </summary>
	public User GetUser(Camp _camp)
    {
		users.TryGetValue(_camp, out var u);
		return u;
    }

	/// <summary>
	/// ҡ����
	/// </summary>
	public int PlayDice(Camp _user)
    {
        if (CurUser.Camp != _user)
        {
			UIWarn.Instance.ShowWarn($"��ǰ����<{_user.Name()}>�Ļغ�");
			return 0;
        }

		CurDice = Rd.GetValue(1, 7);
		return CurDice;
    }

    /// <summary>
    /// �غϿ�ʼ
    /// </summary>
    private IEnumerator StartRound()
    {
		yield return new WaitForSeconds(.2f);
		bgm = AudioManager.Instance.Play("bg", 1, true);
		yield return new WaitForSeconds(1f);

		// ѭ��ֱ��һ��ʤ������
		User winner;
		while (!IsFinish(out winner))
		{
			CurRound++;
			onChangeRound?.Invoke(CurRound);
			// ÿһ�غϱ��������û�
			foreach (var u in users.Values)
			{
				CurUser = u;
				onCamp?.Invoke(u.Camp);
				// ��ǰ��������ʣ���������
				remainStep = 1;
				// ��ʾ������
				CameraFollow.Instance.SetTarget(CurUser.Chess.transform);
				//Debug.Log($"<{u.Name}>�Ļغ�");
				while (remainStep > 0)
				{
					// ѭ��ֱ�����еĲ������
					// ��������˳��ģ��ȴ� -> ҡ���� -> �ƶ� -> ��ɣ���ɺ�����Ϊ�ȴ�
					while (u.Step != UserStep.Completed)
					{
						if (u.StopRound <= 0)
                        {
							if (u.Step == UserStep.PlayDice || u.Step == UserStep.Move || u.Step == UserStep.Tip)
							{
								// ��ȡactionable��ִ�в���
								var action = u.GetActionable();
								action.TakeTurn();
								// �ȴ�����ִ�����
								yield return new WaitUntil(() => action.Completed);
								// ������ɺ����ò���������һ�غ�
								action.Reset();
							}

							// �л���һ������
							u.NextStep();
                        }
                        else
                        {
							u.StopRound -= 1;
							u.ToStep(UserStep.Completed);
							yield return new WaitForSeconds(.4f);
                        }
						
					}
					// ������ɣ�ʣ�������һ
					remainStep--;
					if (remainStep > 0)
					{
						// �������ʣ�������ֱ�������ȴ�
						u.ToStep(UserStep.Wait);
					}
				}
				// ���û��ĻغϽ���
				// ���õ��ȴ�����
				u.ToStep(UserStep.Wait);

				// ����Ƿ����
				if (IsFinish(out var w))
				{
					// �����������һ�غϺ���Ķ����ò����ˣ�����
					break;
				}
			}
		}

		yield return new WaitForSeconds(1);
		bgm.Stop();
		// ��ʾʤ��
		UIManager.Instance.Open<GameOverPanel>().ShowWinner(winner);
	}

    /// <summary>
    /// ����Ƿ���Ϸ����
    /// </summary>
    /// <param name="winnerUserId">ʤ�����id</param>
    private bool IsFinish(out User winnerUser)
    {
        foreach (var u in users.Values)
        {
			// �ݶ�
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
