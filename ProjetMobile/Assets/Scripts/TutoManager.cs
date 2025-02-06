using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

[Serializable]
public struct ForcedPool
{
    public ModificatorData[] forcedModificators;
    public int[] forcedRanks;
}

public class TutoManager : MonoBehaviour
{
    [Header("Parameters")] 
    public ForcedPool[] level1ForcedPools;
    public ForcedPool[] level2ForcedPools;
    
    [Header("Private Infos")] 
    private bool upgradedChest;
    private bool upgradedTurret;
    private int poolIndex;
    
    [Header("References")] 
    [SerializeField] private Image cursorImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Button openChestButton;
    [SerializeField] private Button upgradeChestButton;
    [SerializeField] private Button upgradeTurretButton;


    public void UpgradeChest()
    {
        upgradedChest = true;
    }

    public void UpgradeTurret()
    {
        upgradedTurret = true;
    }


    public (ModificatorData[], int[]) GetForcedPool(int levelIndex)
    {
        ForcedPool[] pickedForcedPools = level1ForcedPools;
        if (levelIndex == 2) pickedForcedPools = level2ForcedPools;
        
        if (poolIndex >= pickedForcedPools.Length) return (new ModificatorData[3], new int[3]);
        
        poolIndex++;

        return (pickedForcedPools[poolIndex - 1].forcedModificators, pickedForcedPools[poolIndex - 1].forcedRanks);
    }
    
    
    public IEnumerator PlayUpgradeChestTutoCoroutine()
    {
        cursorImage.enabled = true;
        openChestButton.enabled = false;

        Transform originalParent = upgradeChestButton.transform.parent;
        upgradeChestButton.transform.parent = backImage.transform;
        
        Time.timeScale = 0.02f;
        backImage.UFadeImage(0.5f, 0.7f, CurveType.EaseOutSin, true);
        
        upgradeChestButton.image.UBounceImageColor(0.8f, Color.white * 0.8f, 0.8f, Color.white, CurveType.EaseInOutSin, true, true);
        
        while (true)
        {
            if (upgradedChest) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        backImage.UFadeImage(0.5f, 0f, CurveType.EaseOutSin, true);
        Time.timeScale = 1f;
        
        upgradeChestButton.image.UStopBounceImageColor();
        HUDManager.Instance.ActualiseButtonColors();

        upgradeChestButton.transform.parent = originalParent;
        openChestButton.enabled = true;
        cursorImage.enabled = false;
    }

    public IEnumerator PlayUpgradeTurretTutoCoroutine()
    {
        cursorImage.enabled = true;
        openChestButton.enabled = false;
        upgradeChestButton.enabled = false;

        Transform originalParent = upgradeTurretButton.transform.parent;
        upgradeTurretButton.transform.parent = backImage.transform;
        
        Time.timeScale = 0.02f;
        backImage.UFadeImage(0.5f, 0.7f, CurveType.EaseOutSin, true);
        
        upgradeTurretButton.image.UBounceImageColor(0.8f, Color.white * 0.8f, 0.8f, Color.white, CurveType.EaseInOutSin, true, true);
        
        while (true)
        {
            if (upgradedTurret) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        backImage.UFadeImage(0.5f, 0f, CurveType.EaseOutSin, true);
        Time.timeScale = 1f;
        
        upgradeTurretButton.image.UStopBounceImageColor();
        HUDManager.Instance.ActualiseButtonColors();
        
        upgradeTurretButton.transform.parent = originalParent;
        upgradeChestButton.enabled = true;
        openChestButton.enabled = true;
        cursorImage.enabled = false;
    }
}
