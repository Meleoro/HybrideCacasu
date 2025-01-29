using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : GenericSingletonClass<HUDManager>
{
    [Header("Actions")] 
    public Action OnModificatorDragEndAction;
    
    [Header("Private Infos")] 
    private bool isDragging;

    [Header("References")] 
    public TurretSlotsManager turretSlotsManager;
    [SerializeField] private Image dragImage;
    private RectTransform canvasRect;


    private void Start()
    {
        canvasRect = GetComponent<RectTransform>();
    }


    private void Update()
    {
        ActualiseDragImage();
    }


    #region Drag Fonctions
    
    public void StartDrag(Sprite draggedSprite)
    {
        if (isDragging) return;
        
        isDragging = true;
        dragImage.sprite = draggedSprite;
        dragImage.enabled = true;
    }

    private void ActualiseDragImage()
    {
        if (!isDragging) return;
        
        if(Input.touchCount == 0)
            EndDrag();

        if(Input.touches[0].phase == TouchPhase.Ended)
            EndDrag();

        Vector2 dragPos = new Vector2(Mathf.Lerp(0, canvasRect.rect.width, Input.GetTouch(0).position.x / Screen.width), 
            Mathf.Lerp(0, canvasRect.rect.height, Input.GetTouch(0).position.y / Screen.height)) 
                          - new Vector2(canvasRect.rect.width * 0.5f, canvasRect.rect.height * 0.5f);
        
        dragImage.rectTransform.localPosition = new Vector3(dragPos.x, dragPos.y, 0);
    }

    private void EndDrag()
    {
        isDragging = false;
        dragImage.enabled = false;
        
        turretSlotsManager.EndDrag();
        OnModificatorDragEndAction.Invoke();
    }

    #endregion
}
