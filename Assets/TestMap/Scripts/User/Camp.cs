using UnityEngine;

/// <summary>
/// 阵营
/// </summary>
public enum Camp
{
    None = 0,
    Player1 = 1,
    Player2 = 2,
}

public static class CampExtension
{
    private static string[] names = new string[] { "无", "Player1", "Player2"};
    private static Color[] colors = new Color[] { Color.white, Color.yellow, Color.cyan};

    public static string Name(this Camp self)
    {
        return names[(int)self];
    }

    public static Color GetColor(this Camp self)
    {
        return colors[(int)self];
    }
}
