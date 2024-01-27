[System.Serializable]
public class ProjectileData
{
    public float Speed;
    public float LifeSpan;
    public UnityEngine.Vector3 Direction;

    public ProjectileData(float speed, float lifeSpan, UnityEngine.Vector3 direction)
    {
        Speed = speed;
        LifeSpan = lifeSpan;
        Direction = direction;
    }

    public ProjectileData(ProjectileData data)
    {
        Speed = data.Speed;
        LifeSpan = data.LifeSpan;
        Direction = data.Direction;
    }
}
