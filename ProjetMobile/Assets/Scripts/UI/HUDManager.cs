using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
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
    private ModificatorData currentDraggedData;
    private int currentDraggedRank;

    [Header("References")] 
    public TurretSlotsManager turretSlotsManager;
    [SerializeField] private GetNewModificatorUI modificatorChoseScript;
    [SerializeField] private LevelProgressUI proressScript;
    [SerializeField] private Image dragImage;
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
    

    #region Drag Fonctions
    
    public void StartDrag(ModificatorData draggedData, int draggedRank)
    {
        if (isDragging) return;
        
        isDragging = true;
        dragImage.sprite = draggedData.modificatorSprite;
        dragImage.enabled = true;
        dragImage.color = ranksColors[draggedRank];

        currentDraggedData = draggedData;
        currentDraggedRank = draggedRank;
        
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
        
        dragImage.rectTransform.localPosition = new Vector3(dragPos.x, dragPos.y, 0);
    }

    private void EndDrag()
    {
        isDragging = false;
        dragImage.enabled = false;
        
        turretSlotsManager.HidePossibleSlots();

        if (turretSlotsManager.EndDrag(currentDraggedData, currentDraggedRank))
        {
            modificatorChoseScript.CloseChoseUpgradeUI();
        }
        OnModificatorDragEndAction.Invoke();
    }

    #endregion
}
