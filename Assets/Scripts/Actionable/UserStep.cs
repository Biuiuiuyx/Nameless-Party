/// <summary>
/// 用户行为步骤
/// </summary>
public enum UserStep
{
	/// <summary>
	/// 等待
	/// </summary>
	Wait = 0,

	/// <summary>
	/// 摇骰子
	/// </summary>
	PlayDice = 1,

	/// <summary>
	/// 行走
	/// </summary>
	Move = 2,

	/// <summary>
	/// 提示
	/// </summary>
	Tip = 3,

	/// <summary>
	/// 所有行为步骤完成
	/// </summary>
	Completed = 4
}
