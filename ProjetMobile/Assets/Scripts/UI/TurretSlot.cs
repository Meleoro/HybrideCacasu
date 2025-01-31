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
    private Cap currentCap;
    private bool isShowingCompatibleColor;
    private bool isCompatible;
    private Coroutine compatibleEffectCoroutine;
    
    [Header("References")] 
    [SerializeField] private Image slotImage;
    private TurretSlotsManager mainScript;


    public void InitialiseSlot(TurretSlotsManager mainScript)
    {
        currentCap = null;
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
        
        if (currentCap == null)
        {
            isCompatible = true;
            compatibleEffectCoroutine = StartCoroutine(CompatibleEffectCoroutine());
        }
        else if (currentCap.capModificatorData == draggedModificator && currentCap.capRank == draggedRank)
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
        if (currentCap is null)
        {
            slotImage.color = noDataColor;
            return;
        }
        
        currentCap.ChangeWantedPos(transform.position - transform.forward * 0.2f);
        currentCap.ChangeWantedRot(transform.rotation * Quaternion.Euler(-90, 0, 0));

        if (isShowingCompatibleColor && !isCompatible)
        {
            slotImage.color *= incompatibleColor;
        }
    }

    
    
    public (ModificatorData, int) GetCurrentModificator()
    {
        if (currentCap is null) return (null, 0);

        return (currentCap.capModificatorData, currentCap.capRank);
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
        if (currentCap == null) return;
        
        mainScript.StartDrag(currentCap, this);
    }

    public (Cap, bool) AddModificator(Cap cap, bool isExchange = false)
    {
        // If the slot is empty
        if (currentCap == null)
        {
            ModificatorAddFeel();
            currentCap = cap;

            return (null, true);
        }

        // If we can merge
        if (currentCap.capModificatorData.modificatorType == cap.capModificatorData.modificatorType && currentCap.capRank == cap.capRank)
        {
            ModificatorAddFeel();
            currentCap.capRank += 1;
            currentCap.ActualiseCap();

            return (null, true);
        }

        // If we want to exchange two slots modificators
        if (isExchange)
        {
            ModificatorAddFeel();
            Cap saveCap = currentCap;

            currentCap = cap;
            
            return (saveCap, true);
        }
        
        // if the slot is not valid
        return (null, false);;
    }

    public void RemoveModificator()
    {
        currentCap = null;
    }

    #endregion


    private void ModificatorAddFeel()
    {

    }
}
