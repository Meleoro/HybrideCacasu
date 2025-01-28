using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : GenericSingletonClass<HUDManager>
{
    [Header("Private Infos")] 
    private bool isDragging;
    
    [Header("References")] 
    [SerializeField] private Image dragImage;


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
        

        Vector2 dragPos = Input.touches[0].position;
        Debug.Log(dragPos);
        Ray ray = CameraManager.Instance._camera.ScreenPointToRay(dragPos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100, LayerMask.NameToLayer("Ground")))
        {
            dragPos = hit.point;
        }
        
        dragImage.rectTransform.localPosition = new Vector3(dragPos.x * 0.5f - Screen.width * 0.25f, dragPos.y * 0.5f - Screen.height * 0.25f, 0);
    }

    private void EndDrag()
    {
        isDragging = false;
        dragImage.enabled = false;
    }

    #endregion
}
