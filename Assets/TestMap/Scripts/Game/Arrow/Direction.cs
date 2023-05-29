public enum Direction
{
    Left = 0,
    Up = 1,
    Right = 2,
    Down = 3
}

public static class DirectionExtension
{
    private static string[] descs = new string[]
    {
        "←","↑","→","↓"
    };

    public static string GetDesc(this Direction self)
    {
        return descs[(int)self];
    }
}
