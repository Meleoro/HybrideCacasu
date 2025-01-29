using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    [Header("Main Parameters")] 
    public string turretName;
    public string turretDescription;
    public int damages;
    [Range(0.1f, 5f)] public float shootCooldown;
    [Range(0f, 3f)] public float shootDispersion;

    [Header("Bullets Infos")] 
    public float bulletSpeed;
    [Range(1, 10)] public int bulletCount;
    [Range(0.1f, 3f)] public float bulletSize;
    [Range(0.1f, 3f)] public float explosionRange;

    [Header("Others")] 
    public ShootBehavior shootBehavior;
    public BulletBehavior bulletBehavior;
}

public enum ShootBehavior
{
    StraightLine,
    Throw,
}

public enum BulletBehavior
{
    Destroy,
    GoThrough,
    Explose
}
