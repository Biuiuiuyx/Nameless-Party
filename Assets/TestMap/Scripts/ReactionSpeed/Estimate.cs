public enum Estimate
{
    Perfect = 0,
    Great,
    Good,
    Bad,
    Miss
}

public static class EstimateExtion
{
    private static float[] rates = new float[]
    {
        .1f,
        .2f,
        .3f,
        .4f,
        .5f,
    };

    public static float GetRate(this Estimate self)
    {
        return rates[(int)self];
    }
}
