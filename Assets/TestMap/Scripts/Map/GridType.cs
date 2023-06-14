using UnityEngine;
/// <summary>
/// 格子类型
/// </summary>
public enum GridType
{
	/// <summary>
	/// 空地
	/// </summary>
	Empty = 1,

	/// <summary>
	/// 起点
	/// </summary>
	Origin = 2,

	/// <summary>
	/// 退格
	/// </summary>
	Back = 3,

	/// <summary>
	/// 小游戏1
	/// </summary>
	Game1 = 4,

	/// <summary>
	/// 小游戏2
	/// </summary>
	Game2 = 5,

	/// <summary>
	/// 小游戏3
	/// </summary>
	Game3 = 6,

	/// <summary>
	/// 小游戏4
	/// </summary>
	Game4 = 7,

	/// <summary>
	/// 终点
	/// </summary>
	Destination = 8,

	/// <summary>
	/// 随机游戏
	/// </summary>
	Game = 9,

	/// <summary>
	/// 随机道具
	/// </summary>
	Prop = 10,

	/// <summary>
	/// 随机游戏
	/// </summary>
	RandGame=11,
}

public static class GridTypeExtension
{
	private static string[] names = new string[]
	{
		"空地","启动","退格","小游戏1","小游戏2","小游戏3","小游戏4","终点","随机游戏","随机道具","随机游戏"
	};

	private static Color[] colors = new Color[]
	{
		Color.white,
		Color.green,
		Color.red,
		Color.blue,
		Color.yellow,
		Color.cyan,
		Color.cyan,
		new Color(.8f, 0, .8f, 1),
		Color.yellow,
		Color.yellow,
		Color.gray,
	};

	public static string Name(this GridType self)
    {
		return names[(int)self - 1];
    }

	public static Color GetColor(this GridType self)
    {
		return colors[(int)self - 1];
    }
}