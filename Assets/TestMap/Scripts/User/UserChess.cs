using Framework;
using GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用户的棋子
/// </summary>
public class UserChess : MonoBehaviour
{
	[SerializeField] protected Camp camp;

	Rigidbody2D rig;
	Animator ani;
    SpriteRenderer render;

	private readonly Queue<IEnumerator> actionQueue = new Queue<IEnumerator>();
	private bool doing = false;

    Queue<Vector2> way;

    private Vector2 curPos;
    private Vector2 targetPos;
    private bool moveable;
    private float useTime;
    private float during = .5f;
    private bool showFinish;

    public bool ShowFinish => showFinish;
	public Camp Camp => camp;
	public bool StepCompleted => !doing && actionQueue.Count == 0 && showFinish;

    private void Start()
    {
		rig = GetComponent<Rigidbody2D>();
		ani = GetComponent<Animator>();
		render = GetComponent<SpriteRenderer>();
    }

    void Update()
	{
		if (!moveable) return;
		// 更新移动时间，线性插值移动
		useTime += Time.deltaTime;
		Vector2 pos = Vector2.Lerp(curPos, targetPos, useTime / during);
		rig.MovePosition(pos);
		if (useTime >= during)
		{
			// 当前移动到达目标点
			rig.MovePosition(targetPos);
			useTime = 0;
			moveable = false;
			// 设置下一个目标点
			MoveToNext();
		}
	}

	/// <summary>
	/// 初始化位置
	/// </summary>
	public void Init(Vector2 _pos)
    {
		transform.position = _pos;
		gameObject.SetActive(true);
    }

	/// <summary>
	/// 移动
	/// </summary>
	private void Move(Queue<Vector2> _way)
    {
		//transform.SetAsLastSibling();
		way = new Queue<Vector2>(_way);
		var list = way.ToArray();
		Vector2 cur = list[list.Length - 1];
        if (Vector2.Distance(rig.position, cur) < .1f)
        {
			return;
        }
		showFinish = false;
		useTime = 0;
		MoveToNext();
		enabled = true;
		ani.SetBool("Walk", true);
    }



	/// <summary>
	/// 把移动加入行为队列
	/// </summary>
	public void OnMove(List<Vector2> _way, int _gid)
    {
		actionQueue.Enqueue(ToMove(_way, _gid));
		ToNext();
    }

	/// <summary>
	/// 把获得起点的奖励加入行为队列
	/// </summary>
	public void GetOriginGold()
    {
		actionQueue.Enqueue(ToGetOriginGold());
		ToNext();
    }

	/// <summary>
	/// 开启移动协程
	/// </summary>
	private IEnumerator ToMove(List<Vector2> _way, int _gid)
	{
		Queue<Vector2> wayQueue = new Queue<Vector2>(_way);
		Move(wayQueue);
		Map.Instance.GetUser(Camp).MoveToGrid(_gid);
		yield return new WaitUntil(() => showFinish);
		doing = false;
		ToNext();
	}

	private IEnumerator ToGetOriginGold()
	{
		// 经过起点加1000
		//Map.Instance.GetUser(Camp).AddGold(1000);
		//UIWarn.Instance.ShowWarn($"[{camp.Name()}]经过了起点！");
		yield return null;
		doing = false;
		ToNext();
	}

	/// <summary>
	/// 移动到下一格
	/// </summary>
	private void MoveToNext()
	{
		if (way != null && way.Count > 0)
		{
			AudioManager.Instance.Play("step");
			curPos = rig.position;
			moveable = true;
			targetPos = way.Dequeue();
			// 计算当前格子和下一个的朝向
			var t = targetPos - rig.position;
			if (t.x >= 0)
            {
				render.flipX = false;
            }
            else
            {
				render.flipX = true;
            }
			//Debug.Log(Vector3.Distance(targetPos, curPos));
		}
		else
		{
			ani.SetBool("Walk", false);
			moveable = false;
			enabled = false;
			showFinish = true;
		}
	}

	/// <summary>
	/// 下个行为
	/// </summary>
	private void ToNext()
	{
		// 当前没有行为在执行并且队列里面不为空
		if (actionQueue.Count > 0 && !doing)
		{
			doing = true;
			StartCoroutine(actionQueue.Dequeue());
		}
	}
}
