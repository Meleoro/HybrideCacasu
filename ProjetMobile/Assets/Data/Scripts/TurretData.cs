using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    [Header("Main Parameters")] 
    public string turretName;
    public int turretIndex;
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
    
    [Header("Upgrade Infos")] 
    public TurretLevel[] turretLevels;

    public void ActualiseTurretLevel(int currentLevel)
    {
        if (turretLevels.Length == 0) return;
        
        int clampedIndex = Mathf.Clamp(currentLevel, 0, turretLevels.Length - 1);

        damages = turretLevels[clampedIndex].newDamages;
        shootCooldown = turretLevels[clampedIndex].newShootCooldown;
        bulletSpeed = turretLevels[clampedIndex].newBulletSpeed;
        bulletSize = turretLevels[clampedIndex].newBulletSize;
        explosionRange = turretLevels[clampedIndex].newExplosionRange;
    }
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

[Serializable]
public struct TurretLevel
{
    public int upgradeCost;
    public int newDamages;
    [Range(0.1f, 5f)] public float newShootCooldown;
    public float newBulletSpeed;
    [Range(0.1f, 3f)] public float newBulletSize;
    [Range(0.1f, 3f)] public float newExplosionRange;
}
