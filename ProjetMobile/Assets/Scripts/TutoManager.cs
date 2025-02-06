using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TutoManager : MonoBehaviour
{
    [Header("Private Infos")] 
    private bool upgradedChest;
    private bool upgradedTurret;
    
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
    
    
    public IEnumerator PlayUpgradeChestTutoCoroutine()
    {
        cursorImage.enabled = true;
        openChestButton.enabled = false;

        Transform originalParent = upgradeChestButton.transform.parent;
        upgradeChestButton.transform.parent = backImage.transform;
        
        Time.timeScale = 0.02f;
        backImage.UFadeImage(0.5f, 0.7f, CurveType.EaseOutSin, true);
        
        while (true)
        {
            if (upgradedChest) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        backImage.UFadeImage(0.5f, 0f, CurveType.EaseOutSin, true);
        Time.timeScale = 1f;

        upgradeChestButton.transform.parent = originalParent;
        openChestButton.enabled = true;
        cursorImage.enabled = false;
    }

    public IEnumerator PlayUpgradeTurretTutoCoroutine()
    {
        cursorImage.enabled = true;
        openChestButton.enabled = false;
        upgradeChestButton.enabled = false;

        Time.timeScale = 0.02f;
        
        while (true)
        {
            if (upgradedTurret) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        Time.timeScale = 1f;
        
        upgradeChestButton.enabled = true;
        openChestButton.enabled = true;
        cursorImage.enabled = false;
    }
}
