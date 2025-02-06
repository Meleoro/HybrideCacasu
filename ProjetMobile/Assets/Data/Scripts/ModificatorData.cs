using UnityEngine;

[CreateAssetMenu(fileName = "ModificatorData", menuName = "Scriptable Objects/ModificatorData")]
public class ModificatorData : ScriptableObject
{
    [Header("Main Infos")] 
    public ModificatorType modificatorType;
    public Sprite modificatorSprite;
    public float[] modificatorImpacts;
    public int[] sellValues;
    
    [Header("Infos")]
    public string modificatorName;
    public string modificatorDescription;
    public bool isPercentDisplay;

    [Header("Others")]
    [Range(0, 3)] public int minimumRank;
}

public enum ModificatorType
{
    Damage,
    FireRate,
    ProjectileCount,
    ProjectileSpeed,
    Size,
    Slow,
    Burn
}