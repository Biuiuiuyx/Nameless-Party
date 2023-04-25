/// <summary>
/// 抽象行为
/// </summary>
public abstract class Actionable
{
	#region ----属性----
	/// <summary>
	/// 是否已完成
	/// </summary>
	public bool Completed { get; protected set; }
	/// <summary>
	/// 步骤类型
	/// </summary>
	public UserStep Step { get; protected set; }
	#endregion

	#region ----构造方法----
	public Actionable(UserStep _step)
	{
		Step = _step;
		Completed = false;
	}
	#endregion

	#region ----公有方法----
	// 轮到该行为步骤
	public void TakeTurn()
	{
		if (Completed)
		{
			return;
		}
		ToDoAction();
	}

	/// <summary>
	/// 重置
	/// </summary>
	public virtual void Reset()
	{
		Completed = false;
	}

	/// <summary>
	/// 设置为已完成
	/// </summary>
	public virtual void Finish()
	{
		Completed = true;
	}
	#endregion

	#region ----私有方法----
	/// <summary>
	/// 执行行为
	/// </summary>
	protected abstract void ToDoAction();
	#endregion

}
