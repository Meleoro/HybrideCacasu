using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TurretSlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Color noDataColor; 
    [SerializeField] private Color dataColor; 
    [SerializeField] private Color compatibleColor; 
    [SerializeField] private Color incompatibleColor; 
    
    [Header("Private Infos")] 
    [SerializeField] private ModificatorData currentModificator;
    [SerializeField] private int currentRank;
    private Vector2 modificatorImageScaleSave;
    private bool isShowingCompatibleColor;
    private bool isCompatible;
    private Coroutine compatibleEffectCoroutine;
    
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


    #region Compatibility Functions

    public void DisplayIsCompatible(ModificatorData draggedModificator, int draggedRank, TurretSlot originSlot)
    {
        if (originSlot == this)
        {
            isShowingCompatibleColor = true;
            isCompatible = false;
                    
            ActualiseSlot();
            return;
        }
        
        isShowingCompatibleColor = true;
        isCompatible = false;
        
        if (currentModificator == null)
        {
            isCompatible = true;
            compatibleEffectCoroutine = StartCoroutine(CompatibleEffectCoroutine());
        }
        else if (currentModificator == draggedModificator && currentRank == draggedRank)
        {
            isCompatible = true;
            compatibleEffectCoroutine = StartCoroutine(CompatibleEffectCoroutine());
        }
        
        ActualiseSlot();
    }

    public void HideIsCompatible(bool isPause)
    {
        if (isCompatible)
        {
            slotImage.UStopLerpImageColor();

            
            slotImage.rectTransform.localScale = Vector3.one * 1f;
            StopCoroutine(compatibleEffectCoroutine);

            if(!isPause)  
                isCompatible = false;
        }
        
        if(!isPause)  
            isShowingCompatibleColor = false;
        
        ActualiseSlot();
    }

    private IEnumerator CompatibleEffectCoroutine()
    {
        Color saveColor = slotImage.color;
        
        while (true)
        {
            slotImage.rectTransform.UChangeScale(0.5f, Vector3.one * 1.05f, CurveType.EaseInOutSin, true);
            slotImage.ULerpImageColor(0.5f, saveColor * new Color(1.15f, 1.15f, 1.15f), CurveType.EaseInOutSin, true);
        
            yield return new WaitForSecondsRealtime(0.55f);
        
            slotImage.rectTransform.UChangeScale(0.5f, Vector3.one * 1f, CurveType.EaseInOutSin, true);
            slotImage.ULerpImageColor(0.5f, saveColor, CurveType.EaseInOutSin, true);
        
            yield return new WaitForSecondsRealtime(0.55f);
        }
    }

    #endregion
    
    
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

        if (isShowingCompatibleColor && !isCompatible)
        {
            slotImage.color *= incompatibleColor;
        }
    }

    
    
    public (ModificatorData, int) GetCurrentModificator()
    {
        if (currentModificator == null) return (null, 0);

        return (currentModificator, currentRank);
    }


    public void StartOverlayButton()
    {
        mainScript.StartOverlaySlot(this);

        if (isCompatible)
        {
            HUDManager.Instance.PauseDrag();
            
            slotImage.rectTransform.UChangeScale(0.1f, Vector3.one * 1.1f, CurveType.EaseOutBack, true);
            slotImage.ULerpImageColor(0.1f, slotImage.color * new Color(1.25f, 1.25f, 1.25f), CurveType.EaseOutBack, true);
        }
    }

    public void EndOverlayButton()
    {
        mainScript.EndOverlaySlot(this);
        
        if (isCompatible)
        {
            HUDManager.Instance.RestartDrag();
        }
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
            currentRank += 1;

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
