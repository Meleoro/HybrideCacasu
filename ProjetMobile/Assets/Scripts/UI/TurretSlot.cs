using UnityEngine;
using UnityEngine.UI;

public class TurretSlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Color noDataColor; 
    [SerializeField] private Color dataColor; 
    
    [Header("Private Infos")] 
    [SerializeField] private ModificatorData currentModificator;
    [SerializeField] private int currentStackAmount;
    
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
        if (currentModificator == null)
        {
            modificatorImage.enabled = false;
            modificatorImage.sprite = null;
            slotImage.color = noDataColor;
            return;
        }

        modificatorImage.enabled = true;
        modificatorImage.sprite = currentModificator.modificatorSprite;
        slotImage.color = dataColor;
    }

    public (ModificatorData, int) GetCurrentModificator()
    {
        if (currentModificator == null) return (null, 0);

        return (currentModificator, currentStackAmount);
    }


    public void StartOverlayButton()
    {
        mainScript.StartOverlaySlot(this);
    }

    public void EndOverlayButton()
    {
        mainScript.EndOverlaySlot(this);
    }
    
    
    #region Drag Functions

    public void StartDrag()
    {
        if (currentModificator == null) return;
        
        mainScript.StartDrag(currentModificator.modificatorSprite, this);
    }

    public (ModificatorData, int) AddModificator(ModificatorData modificatorData, int modificatorStackAmount, bool isExchange = false)
    {
        // If the slot is empty
        if (currentModificator == null)
        {
            currentModificator = modificatorData;
            currentStackAmount = 1;

            return (null, 0);
        }

        // If we can merge
        if (currentModificator.modificatorType == modificatorData.modificatorType)
        {
            currentStackAmount = modificatorStackAmount + currentStackAmount;

            return (null, 0);
        }

        // If we want to exchange two slots modificators
        if (isExchange)
        {
            ModificatorData saveData = currentModificator;
            int saveStack = currentStackAmount;
            
            currentModificator = modificatorData;
            currentStackAmount = modificatorStackAmount;
            
            return (saveData, saveStack);
        }
        
        // if the slot is not valid
        return (null, 0);
    }

    public void RemoveModificator()
    {
        currentModificator = null;
        currentStackAmount = 0;
    }

    #endregion
}
