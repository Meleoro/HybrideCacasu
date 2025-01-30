using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TurretSlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Color noDataColor; 
    [SerializeField] private Color dataColor; 
    
    [Header("Private Infos")] 
    [SerializeField] private ModificatorData currentModificator;
    [SerializeField] private int currentRank;
    private Vector2 modificatorImageScaleSave;
    
    [Header("References")] 
    [SerializeField] private Image slotImage;
    [SerializeField] private Image modificatorImage;
    private TurretSlotsManager mainScript;


    public void InitialiseSlot(TurretSlotsManager mainScript)
    {
        modificatorImageScaleSave = modificatorImage.rectTransform.localScale;
        
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
        slotImage.color = HUDManager.Instance.ranksColors[currentRank];
        //slotImage.color = dataColor;
    }

    public (ModificatorData, int) GetCurrentModificator()
    {
        if (currentModificator == null) return (null, 0);

        return (currentModificator, currentRank);
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
        
        mainScript.StartDrag(currentModificator, currentRank, this);
    }

    public (ModificatorData, int) AddModificator(ModificatorData modificatorData, int newRank, bool isExchange = false)
    {
        // If the slot is empty
        if (currentModificator == null)
        {
            ModificatorAddFeel();
            currentModificator = modificatorData;
            currentRank = newRank;

            return (null, 0);
        }

        // If we can merge
        if (currentModificator.modificatorType == modificatorData.modificatorType && currentRank == newRank)
        {
            ModificatorAddFeel();
            currentRank = newRank + 1;

            return (null, 0);
        }

        // If we want to exchange two slots modificators
        if (isExchange)
        {
            ModificatorAddFeel();
            ModificatorData saveData = currentModificator;
            int saveRank = currentRank;
            
            currentModificator = modificatorData;
            currentRank = newRank;
            
            return (saveData, saveRank);
        }
        
        // if the slot is not valid
        return (null, -1);
    }

    public void RemoveModificator()
    {
        currentModificator = null;
        currentRank = 0;
    }

    #endregion


    private void ModificatorAddFeel()
    {
        modificatorImage.rectTransform.UBounce(0.04f, modificatorImageScaleSave * 0.5f, 0.08f, modificatorImageScaleSave);
    }
}
