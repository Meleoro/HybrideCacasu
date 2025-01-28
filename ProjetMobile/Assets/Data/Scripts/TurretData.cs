using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    [Header("Main Parameters")] 
    public int bulletsDamages;
    public float fireRate;

    [Header("Bullets Infos")] 
    public float bulletSpeed;
}
