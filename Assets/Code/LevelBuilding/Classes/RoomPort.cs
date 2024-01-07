
public class RoomPort
{
    public SerializeableVector3 RelativePosition;
    public int SiblingIndex;
    public Enums.LinePoints LinePoint;
    public Enums.RoomSides SideOfRoom;

    public RoomPort(SerializeableVector3 relativePosition, int siblingIndex, Enums.LinePoints linePoint, Enums.RoomSides sideOfRoom)
    {
        RelativePosition = relativePosition;
        SiblingIndex = siblingIndex;
        LinePoint = linePoint;
        SideOfRoom = sideOfRoom;
    }

    public static bool operator ==(RoomPort portA, RoomPort portB)
    {
        if(portA is null)
        {
            return portB is null;
        }

        return portA.RelativePosition == portB.RelativePosition &&
            portA.SiblingIndex == portB.SiblingIndex &&
            portA.LinePoint == portB.LinePoint &&
            portA.SideOfRoom == portB.SideOfRoom;
    }

    public static bool operator !=(RoomPort portA, RoomPort portB)
    {
        if(portB is null)
        {
            return portA is not null;
        }
        return portA.RelativePosition != portB.RelativePosition ||
            portA.SiblingIndex != portB.SiblingIndex ||
            portA.LinePoint != portB.LinePoint ||
            portA.SideOfRoom != portB.SideOfRoom;
    }

    public override bool Equals(object obj)
    {
        try
        {
            return (RoomPort)obj == this;
        }
        catch(System.Exception e)
        {
            return false;
        }
    }
}
