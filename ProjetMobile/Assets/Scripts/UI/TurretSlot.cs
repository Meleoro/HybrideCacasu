using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class TurretSlot : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private int turretIndex;
    [SerializeField] private float fadeValueAvailableSlot;

    [Header("Public Infos")] 
    public bool bounceTurret;
    
    [Header("Private Infos")] 
    [SerializeField] private Cap currentCap;
    private bool isShowingCompatibleColor;
    private bool isCompatible;
    private bool isActive;
    private Coroutine compatibleEffectCoroutine;
    
    [Header("References")] 
    [SerializeField] private Image slotImage;
    [SerializeField] private Button slotButton;
    private TurretSlotsManager mainScript;


    public void InitialiseSlot(TurretSlotsManager mainScript, bool startEnabled)
    {
        currentCap = null;
        this.mainScript = mainScript;
        ActualiseSlot();

        if (startEnabled)
        {
            EnableSlot();
        }
        else
        {
            DisableSlot();
        }
    }

    public void EnableSlot()
    {
        isActive = true;
        slotImage.enabled = true;
        slotButton.enabled = true;
    }

    private void DisableSlot()
    {
        isActive = false;
        slotImage.enabled = false;
        slotButton.enabled = false;
    }


    #region Compatibility Functions

    public void DisplayIsCompatible(ModificatorData draggedModificator, int draggedRank, TurretSlot originSlot)
    {
        if (!isActive) return;
        
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
        if (!isActive) return;
        
        if (isCompatible)
        {
            slotImage.rectTransform.localScale = Vector3.one * 1f;
            StopCoroutine(compatibleEffectCoroutine);
            
            slotImage.UStopFadeImage();
            slotImage.UFadeImage(0.1f, 0, CurveType.EaseOutSin, true);

            if(!isPause)  
                isCompatible = false;
        }
        
        if(!isPause)  
            isShowingCompatibleColor = false;
        
        ActualiseSlot();
    }

    private IEnumerator CompatibleEffectCoroutine()
    {
        slotImage.UStopFadeImage();
        
        while (true)
        {
            slotImage.UFadeImage(0.5f, fadeValueAvailableSlot, CurveType.EaseInOutSin, true);
        
            yield return new WaitForSecondsRealtime(0.55f);
        
            slotImage.UFadeImage(0.5f, 0, CurveType.EaseInOutSin, true);
        
            yield return new WaitForSecondsRealtime(0.55f);
        }
    }

    #endregion
    
    
    public void ActualiseSlot()
    {
        if (currentCap is null)
        {
            return;
        }
        
        currentCap.ChangeWantedPos(transform.position - transform.forward * 0.3f);
        currentCap.transform.rotation = transform.rotation * Quaternion.Euler(-90, 0, 0);
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
            
            slotImage.UStopFadeImage();
            slotImage.UFadeImage(0.1f, 0.75f, CurveType.EaseOutSin, true);
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
        //if (HUDManager.Instance.modificatorChooseScript.isOpenned) return;
        
        mainScript.StartDrag(currentCap, this);
    }

    public (Cap, bool) AddModificator(Cap cap, bool isExchange = false)
    {
        // If the slot is empty
        if (currentCap == null && cap != null)
        {
            ModificatorAddFeel();
            currentCap = cap;

            return (null, true);
        }

        // If we can merge
        if (currentCap.capModificatorData.modificatorType == cap.capModificatorData.modificatorType && currentCap.capRank == cap.capRank && currentCap.capRank < 3)
        {
            Destroy(cap.gameObject);
            
            ModificatorAddFeel();
            currentCap.capRank += 1;
            currentCap.ActualiseCap();
            currentCap.FuseCaps();

            return (null, true);
        }

        // If we want to exchange two slots modificators
        if (isExchange && cap != null)
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
        bounceTurret = true;
    }
}
