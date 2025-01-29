using UnityEngine;

public class ChestManager : GenericSingletonClass<ChestManager>
{
    [Header("Parameters")] 
    [SerializeField] private int maxChessLevel;
    [SerializeField] private int middleChestCost;
    [SerializeField] private int chessUpgradeCost;

    [Header("Private Infos")] 
    private float[] probaPerTiers = new float[4];
    private int chessLevel;
    
    [Header("References")] 
    [SerializeField] private RectTransform leftButton;
    [SerializeField] private RectTransform middleButton;
    [SerializeField] private RectTransform rightButton;


    #region Click Functions

    public void ClickRightButton()
    {
        
    }
    
    public void ClickMiddleButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoney(middleChestCost)) return;
        
        
    }

    public void ClickLeftButton()
    {
        
    }

    #endregion
}
