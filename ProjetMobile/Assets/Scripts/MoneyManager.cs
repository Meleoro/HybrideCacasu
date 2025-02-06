using System;
using TMPro;
using UnityEngine;
using Utilities;

public class MoneyManager : GenericSingletonClass<MoneyManager>
{
    [Header("Parameters")] 
    [SerializeField] private int middleChestCost;
    [SerializeField] private int chestUpgradeCost;
    [SerializeField] private int towerUpgradeCost;
    [SerializeField] private int middleChestCostAddedPerMiddleChest;
    [SerializeField] private int middleChestCostAddedPerChestUpgrade;
    [SerializeField] private int chestUpgradeCostAddedPerChestUpgrade;
    [SerializeField] private float middleChestCostMultiplierPerMiddleChest = 1f;
    [SerializeField] private float middleChestCostMultiplierPerChestUpgrade = 1f;
    [SerializeField] private float chestUpgradeCostMultiplierPerChestUpgrade = 1f;
    [SerializeField] private float towerUpgradeCostMultiplierPerTowerUpgrade = 1f;
    [SerializeField] private Coin coinPrefab;
    
    [Header("Private Infos")] 
    private int currentMoney;
    private int currentMiddleChestCost;
    private int currentRankUpgradeCost;
    private int currentTowerUpgradeCost;

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI moneyText;


    private void Start()
    {
        currentMiddleChestCost = middleChestCost;
        currentRankUpgradeCost = chestUpgradeCost;
        currentTowerUpgradeCost = towerUpgradeCost;
        
        ActualiseText();
    }
    
    private void ActualiseText()
    {
        moneyText.text = currentMoney.ToString();
    }
    
    
    #region Public Functions

    public void CreateCoin(Vector3 enemyPos, int coinValue)
    {
        Vector3 dir = Vector3.ClampMagnitude(enemyPos - CameraManager.Instance.transform.position, 8.5f);
        
        Coin newCoin = Instantiate(coinPrefab, CameraManager.Instance.transform.position + dir, Quaternion.identity, transform);
        newCoin.Initialise(moneyText.transform.position, coinValue);
    }
    
    public void AddMoney(int moneyAmount)
    {
        currentMoney += moneyAmount;
        AddMoneyFeel();
        
        ActualiseText();
        
        HUDManager.Instance.ActualiseButtonColors();
    }

    public void UseMoney(int moneyAmount)
    {
        currentMoney -= moneyAmount;
        RemoveMoneyFeel();
        
        ActualiseText();
        
        HUDManager.Instance.ActualiseButtonColors();
    }

    public bool VerifyHasEnoughMoneyMiddleChest(bool buyIfGood = false)
    {
        if (buyIfGood)
        {
            if (currentMoney >= currentMiddleChestCost)
            {
                UseMoney(currentMiddleChestCost);
                currentMiddleChestCost += middleChestCostAddedPerMiddleChest;
                currentMiddleChestCost = (int)(currentMiddleChestCost * middleChestCostMultiplierPerMiddleChest);
                
                return true;
            }

            return false;
        }
        
        return currentMoney >= currentMiddleChestCost;
    }
    
    public bool VerifyHasEnoughMoneyChestUpgrade(bool buyIfGood = false)
    {
        if (buyIfGood)
        {
            if (currentMoney >= currentRankUpgradeCost)
            {
                UseMoney(currentRankUpgradeCost);
                currentRankUpgradeCost += chestUpgradeCostAddedPerChestUpgrade;
                currentRankUpgradeCost = (int)(currentRankUpgradeCost * chestUpgradeCostMultiplierPerChestUpgrade);
                currentMiddleChestCost += middleChestCostAddedPerChestUpgrade;
                currentMiddleChestCost = (int)(currentMiddleChestCost * middleChestCostMultiplierPerChestUpgrade);
                
                return true;
            }

            return false;
        }
        
        return currentMoney >= currentRankUpgradeCost;
    }
    
    public bool VerifyHasEnoughMoneyTowerUpgrade(bool buyIfGood = false)
    {
        if (buyIfGood)
        {
            if (currentMoney >= currentTowerUpgradeCost)
            {
                UseMoney(currentTowerUpgradeCost);
                currentTowerUpgradeCost = (int)(currentTowerUpgradeCost * towerUpgradeCostMultiplierPerTowerUpgrade);
                
                return true;
            }

            return false;
        }
        
        return currentMoney >= currentTowerUpgradeCost;
    }
    
    public bool VerifyHasEnoughMoney(int moneyAmount)
    {
        return currentMoney >= moneyAmount;
    }
    
    #endregion

    
    #region Feel Functions

    private void AddMoneyFeel()
    {
        moneyText.rectTransform.UBounce(0.05f, Vector3.one * 1.2f, 0.15f, Vector3.one);
    }
    
    private void RemoveMoneyFeel()
    {
        moneyText.rectTransform.UBounce(0.05f, Vector3.one * 0.8f, 0.15f, Vector3.one);
    }

    #endregion
    
}
