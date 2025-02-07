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

    [Header("Public Infos")] 
    public bool forceMiddleChoice;
    
    [Header("Private Infos")] 
    private bool opennedChest;
    private bool upgradedChest;
    private bool upgradedTurret;
    private bool dragAndDropped;
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

    public void OpenChest()
    {
        opennedChest = true;
    }

    public void DragAndDrop()
    {
        dragAndDropped = true;
    }
    

    
    public (ModificatorData[], int[]) GetForcedPool(int levelIndex)
    {
        ForcedPool[] pickedForcedPools = level1ForcedPools;
        if (levelIndex == 2) pickedForcedPools = level2ForcedPools;
        
        if (levelIndex == 1 && poolIndex == 1) StartCoroutine(PlayDragAndDropTuto());
        if (poolIndex >= pickedForcedPools.Length) return (new ModificatorData[3], new int[3]);
            
        poolIndex++;
        
        return (pickedForcedPools[poolIndex - 1].forcedModificators, pickedForcedPools[poolIndex - 1].forcedRanks);
    }


    public IEnumerator PlayOpenChestTutoCoroutine(bool dragAndDropAnimNext)
    {
        Transform originalParent = openChestButton.transform.parent;
        openChestButton.transform.parent = backImage.transform;
        
        Time.timeScale = 0.02f;
        backImage.UFadeImage(0.5f, 0.7f, CurveType.EaseOutSin, true);
        
        openChestButton.image.UBounceImageColor(0.8f, Color.white * 0.8f, 0.8f, Color.white, CurveType.EaseInOutSin, true, true);
        
        while (true)
        {
            if (opennedChest) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        backImage.UFadeImage(0.5f, 0f, CurveType.EaseOutSin, true);
        Time.timeScale = 1f;
        
        openChestButton.image.UStopBounceImageColor();
        HUDManager.Instance.ActualiseButtonColors();

        openChestButton.transform.parent = originalParent;

        if (dragAndDropAnimNext)
        {
            StartCoroutine(PlayDragAndDropTuto());
        }
    }

    private IEnumerator PlayDragAndDropTuto()
    {
        yield return new WaitForSecondsRealtime(1f);

        forceMiddleChoice = true;
        cursorImage.enabled = true;
        
        while (true)
        {
            if (dragAndDropped) break;
            
            yield return new WaitForEndOfFrame();
        }

        forceMiddleChoice = false;
        dragAndDropped = false;
        cursorImage.enabled = false;
    }
    
    
    public IEnumerator PlayUpgradeChestTutoCoroutine()
    {
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
    }

    public IEnumerator PlayUpgradeTurretTutoCoroutine()
    {
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
    }
}
