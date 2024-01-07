
public class RoomConnection
{
    public RoomPort FromPort;
    public string ToLevelName;
    public RoomPort ToPort;

    public RoomConnection(RoomPort fromPort, string toLevelName, RoomPort toPort)
    {
        FromPort = fromPort;
        ToLevelName = toLevelName;
        ToPort = toPort;
    }
}
