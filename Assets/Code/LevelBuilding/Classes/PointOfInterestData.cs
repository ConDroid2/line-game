
[System.Serializable]
public class PointOfInterestData
{
    public int Type;
    public string FlagName;

    public PointOfInterestData(PointOfInterest pointOfInterest)
    {
        Type = (int)pointOfInterest.Type;
        FlagName = pointOfInterest.FlagName;
    }

    [Newtonsoft.Json.JsonConstructor]
    public PointOfInterestData(int type, string flagName)
    {
        Type = type;
        FlagName = flagName;
    }
}
