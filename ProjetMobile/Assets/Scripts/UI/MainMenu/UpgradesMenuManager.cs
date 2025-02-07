using System;
using TMPro;
using UnityEngine;
using Utilities;

public class UpgradesMenuManager : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private TurretData[] possibleTurrets;

    [Header("Private Infos")] 
    private TurretData currentData;
    
    [Header("References")] 
    [SerializeField] private UpgradeSlot[] equippedSlots;
    [SerializeField] private UpgradeSlot[] notEquippedSlots;

    [Header("Details References")] 
    [SerializeField] private TextMeshProUGUI detailsTurretNameText;
    [SerializeField] private TextMeshProUGUI detailsTurretDamagesText;
    [SerializeField] private TextMeshProUGUI detailsTurretReloadText;
    [SerializeField] private TextMeshProUGUI detailsTurretSpeedText;
    [SerializeField] private TextMeshProUGUI detailsUpgradeCostText;
    [SerializeField] private TextMeshProUGUI softCurrencyText;
    [SerializeField] private RectTransform detailsParentTr;


    private void Start()
    {
        DontDestroyOnLoadObject.Instance.OnSaveLoad += ActualiseUpgradeSlots;
        
        for (int i = 0; i < equippedSlots.Length; i++)
        {
            equippedSlots[i].SetupSlot(possibleTurrets[i], this);
        }
    }


    public void OpenTurretDetails(TurretData slotData)
    {
        currentData = slotData;
        
        detailsParentTr.UChangeLocalPosition(0.25f, Vector3.zero, CurveType.EaseOutSin);
        
        detailsTurretNameText.text = slotData.turretName;
        detailsTurretDamagesText.text = slotData.damages.ToString();
        detailsTurretReloadText.text = slotData.shootCooldown.ToString();
        detailsTurretSpeedText.text = slotData.bulletSpeed.ToString();

        if (DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1 < slotData.turretLevels.Length)
            detailsUpgradeCostText.text = slotData
                .turretLevels[DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1].upgradeCost
                .ToString();

        else
            detailsUpgradeCostText.text = "MAX";
    }

    private void ActualiseDetails()
    {
        if (currentData == null) return;
        
        detailsTurretNameText.text = currentData.turretName;
        detailsTurretDamagesText.text = currentData.damages.ToString();
        detailsTurretReloadText.text = currentData.shootCooldown.ToString();
        detailsTurretSpeedText.text = currentData.bulletSpeed.ToString();

        if (DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1 < currentData.turretLevels.Length)
            detailsUpgradeCostText.text = (1 + currentData
                .turretLevels[DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1].upgradeCost)
                .ToString();

        else
            detailsUpgradeCostText.text = "MAX";
    }

    public void UpgradeTurret()
    {
        if (DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1 >=
            currentData.turretLevels.Length)
        {
            return;
        }
        
        if (DontDestroyOnLoadObject.Instance.ownedSoftCurrency < currentData
                  .turretLevels[DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1].upgradeCost)
        {
            return;
        }

        DontDestroyOnLoadObject.Instance.ownedSoftCurrency -= currentData
            .turretLevels[DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex]].upgradeCost;
        softCurrencyText.text = DontDestroyOnLoadObject.Instance.ownedSoftCurrency.ToString();
        
        DontDestroyOnLoadObject.Instance.ChangeTurretLevel(currentData.turretIndex, DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex] + 1);
        currentData.ActualiseTurretLevel(DontDestroyOnLoadObject.Instance.turretsLevels[currentData.turretIndex]);

        ActualiseUpgradeSlots();
    }

    public void ActualiseUpgradeSlots()
    {
        for (int i = 0; i < equippedSlots.Length; i++)
        {
            equippedSlots[i].ActualiseInfos();
        }

        ActualiseDetails();
    }
    
    public void CloseTurretDetails()
    {
        detailsParentTr.UChangeLocalPosition(0.25f, new Vector3(500, 0, 0), CurveType.EaseOutSin);
    }
}
