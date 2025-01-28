using UnityEngine;
using UnityEngine.UI;

public class TurretSlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Color noDataColor; 
    [SerializeField] private Color dataColor; 
    
    [Header("Private Infos")] 
    [SerializeField] private ModificatorData currentData;
    [SerializeField] private int modificatorStackAmount;
    
    [Header("References")] 
    [SerializeField] private Image slotImage;
    [SerializeField] private Image modificatorImage;
    private TurretSlotsManager mainScript;


    public void InitialiseSlot(TurretSlotsManager mainScript)
    {
        this.mainScript = mainScript;
        ActualiseSlot();
    }
    
    public void ActualiseSlot()
    {
        if (currentData == null)
        {
            modificatorImage.enabled = false;
            modificatorImage.sprite = null;
            slotImage.color = noDataColor;
            return;
        }

        modificatorImage.enabled = true;
        modificatorImage.sprite = currentData.modificatorSprite;
        slotImage.color = dataColor;
    }

    public (ModificatorData, int) GetCurrentModificator()
    {
        if (currentData == null) return (null, 0);

        return (currentData, modificatorStackAmount);
    }
    

    #region Drag Functions

    public void StartDrag()
    {
        if (currentData == null) return;
        
        mainScript.StartDrag(currentData.modificatorSprite);
    }

    public void EndDrag()
    {
        
    }

    #endregion
}
