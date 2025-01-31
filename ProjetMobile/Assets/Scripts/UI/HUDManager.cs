using System;
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
    public Action OnModificatorDragSuccessAction;

    [Header("Parameters")] 
    public Color[] ranksColors;
    
    [Header("Private Infos")] 
    private bool isDragging;
    private bool isOverlayingSell;
    private ModificatorData currentDraggedData;
    private int currentDraggedRank;

    [Header("References")] 
    public TurretSlotsManager turretSlotsManager;
    [SerializeField] private GetNewModificatorUI modificatorChoseScript;
    [SerializeField] private LevelProgressUI proressScript;
    [SerializeField] private Image dragImage;
    [SerializeField] private RectTransform sellRectTr;
    [SerializeField] private RectTransform baseButtonsRectTr;
    [SerializeField] private RectTransform upButtonPosRefRectTr;
    [SerializeField] private RectTransform downButtonPosRefRectTr;
    private RectTransform canvasRect;


    private void Start()
    {
        proressScript.GenerateWavesMarkers(GameManager.Instance.levelData);
        canvasRect = GetComponent<RectTransform>();
    }


    private void Update()
    {
        ActualiseDragImage();
        proressScript.ActualiseFillImage(GameManager.Instance.currentTimer / GameManager.Instance.levelData.levelDuration);
    }


    #region Bottom Buttons

    public void ClickMiddleButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyMiddleChest(true)) return;
        
        modificatorChoseScript.OpenChoseUpgradeUI();
    }

    public void ClickRightButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyChestUpgrade(true)) return;
        
        modificatorChoseScript.UpgradeChest();
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
    
    public void StartDrag(ModificatorData draggedData, int draggedRank)
    {
        if (isDragging) return;
        
        DisplaySellButton();
        
        isDragging = true;
        dragImage.sprite = draggedData.modificatorSprite;
        dragImage.enabled = true;
        dragImage.color = ranksColors[draggedRank];

        currentDraggedData = draggedData;
        currentDraggedRank = draggedRank;
        
        turretSlotsManager.ShowPossibleSlots(currentDraggedData, currentDraggedRank);
        
        Vector2 dragPos = new Vector2(Mathf.Lerp(0, canvasRect.rect.width, Touchscreen.current.touches[0].position.x.value / Screen.width), 
                              Mathf.Lerp(0, canvasRect.rect.height, Touchscreen.current.touches[0].position.y.value / Screen.height)) 
                          - new Vector2(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f);
        dragImage.rectTransform.localPosition = dragPos;
    }

    public void PauseDrag()
    {
        turretSlotsManager.HidePossibleSlots(true);
    }

    public void RestartDrag()
    {
        turretSlotsManager.ShowPossibleSlots(currentDraggedData, currentDraggedRank);
    }
    
    private void ActualiseDragImage()
    {
        if (!isDragging) return;
        
        if(Touchscreen.current.touches.Count == 0)
            EndDrag();

        if(Touchscreen.current.touches[0].phase.value == UnityEngine.InputSystem.TouchPhase.Ended)
            EndDrag();

        Vector2 dragPos = new Vector2(Mathf.Lerp(0, canvasRect.rect.width, Touchscreen.current.touches[0].position.x.value / Screen.width), 
            Mathf.Lerp(0, canvasRect.rect.height, Touchscreen.current.touches[0].position.y.value / Screen.height)) 
                          - new Vector2(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f);
        
        dragImage.rectTransform.localPosition = Vector3.Lerp(dragImage.rectTransform.localPosition, new Vector3(dragPos.x, dragPos.y, 0), Time.unscaledDeltaTime * 10f);
    }

    private void EndDrag()
    {
        HideSellButton();
        
        isDragging = false;
        dragImage.enabled = false;
        
        turretSlotsManager.HidePossibleSlots(false);

        if (isOverlayingSell)
        {
            turretSlotsManager.EndDragSell(currentDraggedData, currentDraggedRank);
            modificatorChoseScript.CloseChoseUpgradeUI();
            
            OnModificatorDragEndAction.Invoke();
        }
        else
        {
            if (turretSlotsManager.EndDrag(currentDraggedData, currentDraggedRank))
            {
                modificatorChoseScript.CloseChoseUpgradeUI();
            }
            OnModificatorDragEndAction.Invoke();
        }
    }

    #endregion
}
