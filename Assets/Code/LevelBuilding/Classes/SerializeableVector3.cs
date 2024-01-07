

public class SerializeableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializeableVector3(UnityEngine.Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public UnityEngine.Vector3 ConvertToVector3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }

    public static bool operator ==(SerializeableVector3 vectorA, SerializeableVector3 vectorB)
    {
        return vectorA.x == vectorB.x &&
            vectorA.y == vectorB.y &&
            vectorA.z == vectorB.z;
    }

    public static bool operator !=(SerializeableVector3 vectorA, SerializeableVector3 vectorB)
    {
        return vectorA.x != vectorB.x ||
            vectorA.y != vectorB.y |
            vectorA.z != vectorB.z;
    }

    public override bool Equals(object obj)
    {
        try
        {
            return (SerializeableVector3)obj == this;
        }
        catch (System.Exception e)
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"{{{x}, {y}, {z}}}";
    }
}
