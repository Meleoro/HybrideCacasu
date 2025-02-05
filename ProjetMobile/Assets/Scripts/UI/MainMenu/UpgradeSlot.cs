using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Private Infos")] 
    private TurretData currentData;
    private int currentTurretLvl;
    
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI turretNameText;
    [SerializeField] private TextMeshProUGUI turretLvlText;

    
    
    
    
    public void ActualiseInfos()
    {
        turretNameText.text = currentData.turretName;
        turretLvlText.text = "Lv." + currentTurretLvl;
    }
}
