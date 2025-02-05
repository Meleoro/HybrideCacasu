using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Private Infos")] 
    private TurretData currentData;
    
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI turretNameText;
    [SerializeField] private TextMeshProUGUI turretLvlText;
    private UpgradesMenuManager mainScript;

    
    public void ClickSlot()
    {
        mainScript.OpenTurretDetails(currentData);
    }


    public void SetupSlot(TurretData data, UpgradesMenuManager mainScript)
    {
        this.mainScript = mainScript;
        currentData = data;

        ActualiseInfos();
    }
    
    public void ActualiseInfos()
    {
        currentData.ActualiseTurretLevel(DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex]);
        
        turretNameText.text = currentData.turretName;
        
        turretLvlText.text = "Lv." + (DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1);
    }
}
