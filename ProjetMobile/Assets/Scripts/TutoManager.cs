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
        
        upgradeChestButton.image.UBounceImageColor(0.8f, Color.white * 0.8f, 0.8f, Color.white, CurveType.EaseInOutSin, true, true);
        
        while (true)
        {
            if (upgradedChest) break;
            
            yield return new WaitForEndOfFrame();
        }
        
        backImage.UFadeImage(0.5f, 0f, CurveType.EaseOutSin, true);
        Time.timeScale = 1f;
        
        upgradeChestButton.image.UStopBounceImageColor();
        upgradeChestButton.image.ULerpImageColor(0.5f, Color.white);

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
        upgradeTurretButton.image.ULerpImageColor(0.5f, Color.white);
        
        upgradeTurretButton.transform.parent = originalParent;
        upgradeChestButton.enabled = true;
        openChestButton.enabled = true;
        cursorImage.enabled = false;
    }
}
