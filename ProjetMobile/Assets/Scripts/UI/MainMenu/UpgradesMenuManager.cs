using UnityEngine;

public class UpgradesMenuManager : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private TurretData possibleTurrets;
    
    [Header("References")] 
    [SerializeField] private UpgradeSlot[] equippedSlots;
    [SerializeField] private UpgradeSlot[] notEquippedSlots;
}
