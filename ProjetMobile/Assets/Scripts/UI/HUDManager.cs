using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using Utilities;
using TouchPhase = UnityEngine.TouchPhase;

public class HUDManager : GenericSingletonClass<HUDManager>
{
    [Header("Actions")] 
    public Action OnModificatorDragEndAction;

    [Header("Parameters")] 
    public Color[] ranksColors;
    [SerializeField][Range(1f, 2f)] private float turretDamageMultiplierPerUpgrade;
    [SerializeField][Range(1f, 2f)] private float turretFireRateMultiplierPerUpgrade;
    [SerializeField][Range(1f, 2f)] private float turretBulletSpeedMultiplierPerUpgrade;
    [SerializeField][Range(1f, 2f)] private float turretBulletSizeMultiplierPerUpgrade;
    [SerializeField] private Color availableButtonColor = new Color(1, 1, 1);
    [SerializeField] private Color notAvailableButtonColor = new Color(0.5f, 0.5f, 0.5f);
    
    [Header("Private Infos")] 
    private bool isDragging;
    private bool isOverlayingSell;
    private Cap currentDraggedCap;
    private bool isDraggingNewCap;

    [Header("References")] 
    public TurretSlotsManager turretSlotsManager;
    public GetNewModificatorUI modificatorChooseScript;
    public EndScreen endScreen;
    [SerializeField] private LevelProgressUI progressScript;
    [SerializeField] private ParticleSystem upgradeChestVFX;
    [SerializeField] private LevelTransition transitionScript;
    public List<TurretMaster> turretScripts = new();
    private RectTransform canvasRect;
    
    [Header("Bottom Buttons References")]
    [SerializeField] private RectTransform sellRectTr;
    [SerializeField] private RectTransform baseButtonsRectTr;
    [SerializeField] private RectTransform upButtonPosRefRectTr;
    [SerializeField] private RectTransform downButtonPosRefRectTr;
    [SerializeField] private RectTransform openChestButtonRectTr;
    [SerializeField] private Image upgradeTowerButtonImage;
    [SerializeField] private Image openChestButtonImage;
    [SerializeField] private Image upgradeChestButtonImage;


    public void InitialiseHUD()
    {
        progressScript.GenerateWavesMarkers(GameManager.Instance.levelData);
        canvasRect = GetComponent<RectTransform>();
        transitionScript.ExitTransitionCoroutine();
    }

    private void Update()
    {
        ActualiseDrag();
        progressScript.ActualiseFillImage(GameManager.Instance.currentTimer / GameManager.Instance.levelData.levelDuration);
    }


    #region Bottom Buttons

    public void ActualiseButtonColors()
    {
        upgradeTowerButtonImage.color = MoneyManager.Instance.VerifyHasEnoughMoneyTowerUpgrade()
            ? availableButtonColor
            : notAvailableButtonColor;
        
        openChestButtonImage.color = MoneyManager.Instance.VerifyHasEnoughMoneyMiddleChest()
            ? availableButtonColor
            : notAvailableButtonColor;
        
        upgradeChestButtonImage.color = MoneyManager.Instance.VerifyHasEnoughMoneyChestUpgrade()
            ? availableButtonColor
            : notAvailableButtonColor;
    }
    
    public void ClickMiddleButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyMiddleChest(true)) return;
        
        StartCoroutine(modificatorChooseScript.OpenChoseUpgradeUICoroutine());
    }

    public void ClickRightButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyChestUpgrade(true)) return;
        
        upgradeChestVFX.Play();
        openChestButtonRectTr.UBounce(0.1f, Vector3.one * 1.4f, 0.3f, Vector3.one, CurveType.None, true);
        
        modificatorChooseScript.UpgradeChest();
    }

    public void ClickLeftButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyTowerUpgrade(true)) return;
        
        for (int i = 0; i < turretScripts.Count; i++)
        {
            turretScripts[i].UpgradeTurret(turretDamageMultiplierPerUpgrade-1, turretFireRateMultiplierPerUpgrade-1, 
                turretBulletSizeMultiplierPerUpgrade-1, turretBulletSpeedMultiplierPerUpgrade-1);
        }
    }

    #endregion


    #region Sell Button

    private void DisplaySellButton()
    {
        sellRectTr.UChangePosition(0.45f, upButtonPosRefRectTr.position, CurveType.EaseInCubic, true);
        baseButtonsRectTr.UChangePosition(0.45f, downButtonPosRefRectTr.position, CurveType.EaseInCubic, true);
    }

    private void HideSellButton()
    {
        sellRectTr.UChangePosition(0.45f, downButtonPosRefRectTr.position, CurveType.EaseInCubic, true);
        baseButtonsRectTr.UChangePosition(0.45f, upButtonPosRefRectTr.position, CurveType.EaseInCubic, true);
    }

    public void OverlaySellButton()
    {
        isOverlayingSell = true;
    }

    public void QuitOverlaySell()
    {
        isOverlayingSell = false;
    }

    #endregion
    

    #region Drag Fonctions
    
    public void StartDrag(Cap draggedCap, bool isNewCap)
    {
        if (isDragging) return;
        
        DisplaySellButton();

        isDraggingNewCap = isNewCap;
        currentDraggedCap = draggedCap;
        
        isDragging = true;
        turretSlotsManager.ShowPossibleSlots(currentDraggedCap);
    }

    public void PauseDrag()
    {
        turretSlotsManager.HidePossibleSlots(true);
    }

    public void RestartDrag()
    {
        turretSlotsManager.ShowPossibleSlots(currentDraggedCap);
    }
    
    private void ActualiseDrag()
    {
        if (!isDragging) return;

        if (Touchscreen.current.touches.Count == 0)
        {
            EndDrag();
            return;
        }

        if (Touchscreen.current.touches[0].phase.value == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            EndDrag();
            return;
        }

        Vector2 dragPos = new Vector2(Mathf.Lerp(0, canvasRect.rect.width, Touchscreen.current.touches[0].position.x.value / Screen.width), 
            Mathf.Lerp(0, canvasRect.rect.height, Touchscreen.current.touches[0].position.y.value / Screen.height)) 
                          - new Vector2(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f);
        //dragImage.rectTransform.localPosition = Vector3.Lerp(dragImage.rectTransform.localPosition, new Vector3(dragPos.x, dragPos.y, 0), Time.unscaledDeltaTime * 10f);
        currentDraggedCap.ChangeWantedPos(canvasRect.TransformPoint(dragPos));
    }

    private void EndDrag()
    {
        HideSellButton();
        
        isDragging = false;
        
        turretSlotsManager.HidePossibleSlots(false);

        if (isOverlayingSell)
        {
            turretSlotsManager.EndDragSell(currentDraggedCap);
            
            if(isDraggingNewCap)
                modificatorChooseScript.CloseChoseUpgradeUI();
            
            OnModificatorDragEndAction.Invoke();
        }
        else
        {
            if (turretSlotsManager.EndDrag(currentDraggedCap) && isDraggingNewCap)
            {
                modificatorChooseScript.CloseChoseUpgradeUI();
            }
            else
            {
                modificatorChooseScript.EndDrag();
            }
            
            OnModificatorDragEndAction.Invoke();
        }
    }

    #endregion
}
