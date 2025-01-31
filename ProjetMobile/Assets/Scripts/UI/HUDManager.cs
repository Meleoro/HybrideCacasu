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
    private Cap currentDraggedCap;

    [Header("References")] 
    public TurretSlotsManager turretSlotsManager;
    public GetNewModificatorUI modificatorChooseScript;
    [SerializeField] private LevelProgressUI proressScript;
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
        ActualiseDrag();
        proressScript.ActualiseFillImage(GameManager.Instance.currentTimer / GameManager.Instance.levelData.levelDuration);
    }


    #region Bottom Buttons

    public void ClickMiddleButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyMiddleChest(true)) return;
        
        modificatorChooseScript.OpenChoseUpgradeUI();
    }

    public void ClickRightButton()
    {
        if (!MoneyManager.Instance.VerifyHasEnoughMoneyChestUpgrade(true)) return;
        
        modificatorChooseScript.UpgradeChest();
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
    
    public void StartDrag(Cap draggedCap)
    {
        if (isDragging) return;
        
        DisplaySellButton();

        currentDraggedCap = draggedCap;
        
        isDragging = true;
        turretSlotsManager.ShowPossibleSlots(currentDraggedCap);
        
        Vector2 dragPos = new Vector2(Mathf.Lerp(0, canvasRect.rect.width, Touchscreen.current.touches[0].position.x.value / Screen.width), 
                              Mathf.Lerp(0, canvasRect.rect.height, Touchscreen.current.touches[0].position.y.value / Screen.height)) 
                          - new Vector2(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f);
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
            modificatorChooseScript.CloseChoseUpgradeUI();
            
            OnModificatorDragEndAction.Invoke();
        }
        else
        {
            if (turretSlotsManager.EndDrag(currentDraggedCap))
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
