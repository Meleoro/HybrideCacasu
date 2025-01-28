using UnityEngine;

[CreateAssetMenu(fileName = "ModificatorData", menuName = "Scriptable Objects/ModificatorData")]
public class ModificatorData : ScriptableObject
{
    [Header("Main Infos")] 
    public ModificatorType modificatorType;
    public Sprite modificatorSprite;
    public float modificatorImpact;
    
    [Header("Infos")]
    public string modificatorName;
    public string modificatorDescription;
}

public enum ModificatorType
{
    Damage,
    FireRate
}