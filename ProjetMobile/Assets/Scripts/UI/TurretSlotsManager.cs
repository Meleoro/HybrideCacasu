using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlotsManager : MonoBehaviour
{
    [Header("Private Infos")] 
    private TurretSlot currentOverlayedSlot;
    private TurretSlot dragSlotOrigin;
    private TurretSlot[] turretSlots;

    [Header("References")] 
    [SerializeField] private TurretSlot[] turret1Slots;
    [SerializeField] private TurretSlot[] turret2Slots;
    [SerializeField] private TurretSlot[] turret3Slots;
    [SerializeField] private Image newModificatorImage;

    
    private void Start()
    {
        dragSlotOrigin = null;
        turretSlots = new TurretSlot[9];
        int currentIndex = 0;
        
        for (int i = 0; i < turret1Slots.Length; i++)
        {
            turret1Slots[i].InitialiseSlot(this);
            turretSlots[currentIndex] = turret1Slots[i];
            currentIndex++;
        }
        for (int i = 0; i < turret2Slots.Length; i++)
        {
            turret2Slots[i].InitialiseSlot(this);
            turretSlots[currentIndex] = turret2Slots[i];
            currentIndex++;
        }
        for (int i = 0; i < turret3Slots.Length; i++)
        {
            turret3Slots[i].InitialiseSlot(this);
            turretSlots[currentIndex] = turret3Slots[i];
            currentIndex++;
        }
    }
    
    

    #region Main Functions

    public void ActualiseSlotsImages()
    {
        for (int i = 0; i < turret1Slots.Length; i++)
        {
            turret1Slots[i].ActualiseSlot();
        }
        for (int i = 0; i < turret2Slots.Length; i++)
        {
            turret2Slots[i].ActualiseSlot();
        }
        for (int i = 0; i < turret3Slots.Length; i++)
        {
            turret3Slots[i].ActualiseSlot();
        }
    }
    
    public TurretModificatorValues GetTurretModificators(int turretIndex)
    {
        TurretSlot[] slots = turret1Slots;
        switch (turretIndex)
        {
            case 1 :
                slots = turret2Slots;
                break;
            case 2 :
                slots = turret3Slots;
                break;
        }

        TurretModificatorValues returnedValues = new TurretModificatorValues(1, 1);

        for (int i = 0; i < slots.Length; i++)
        {
            (ModificatorData modificatorData, int rank) = slots[i].GetCurrentModificator();
            if (modificatorData == null) continue;

            switch (modificatorData.modificatorType)
            {
                case ModificatorType.Damage:
                    returnedValues.damageMultiplier += modificatorData.modificatorImpacts[rank];
                    break;
                
                case ModificatorType.FireRate:
                    returnedValues.fireRateMultiplier += modificatorData.modificatorImpacts[rank];
                    break;
                
                case ModificatorType.ProjectileCount:
                    returnedValues.addedProjectiles += (int)(modificatorData.modificatorImpacts[rank]);
                    break;
                
                case ModificatorType.ProjectileSpeed:
                    returnedValues.projectileSpeedMultiplier += modificatorData.modificatorImpacts[rank];
                    break;
                
                case ModificatorType.Size:
                    returnedValues.projectileSizeMultiplier += modificatorData.modificatorImpacts[rank];
                    break;
                
                case ModificatorType.Slow:
                    returnedValues.slowStrength += modificatorData.modificatorImpacts[rank] ;
                    break;
                
                case ModificatorType.Burn:
                    returnedValues.burnStrength += modificatorData.modificatorImpacts[rank];
                    break;
            }
        }

        return returnedValues;
    }

    #endregion

    
    #region Overlay Functions

    public void StartOverlaySlot(TurretSlot overlayedSlot)
    {
        currentOverlayedSlot = overlayedSlot;
    }

    public void EndOverlaySlot(TurretSlot endedOverlayedSlot)
    {
        if (currentOverlayedSlot == endedOverlayedSlot)
        {
            currentOverlayedSlot = null;
        }
    }

    #endregion
    

    #region Drag Functions

    public void StartDrag(Cap cap, TurretSlot dragOrigin)
    {
        dragSlotOrigin = dragOrigin;
        HUDManager.Instance.StartDrag(cap);
    }
    
    public bool EndDrag(Cap cap)
    {
        if (currentOverlayedSlot is null) return false;
        if (currentOverlayedSlot == dragSlotOrigin) return false;

        Cap capSave = null;
        bool succedded = false;

        if (dragSlotOrigin is not null)
        {
            (capSave, succedded) = currentOverlayedSlot.AddModificator(cap, true);
        
            dragSlotOrigin.RemoveModificator();
            if (capSave is not null)
            {
                dragSlotOrigin.AddModificator(capSave);
            }

            dragSlotOrigin = null;
        }
        else
        {
            (capSave, succedded) = currentOverlayedSlot.AddModificator(cap);
        }
        
        ActualiseSlotsImages();
        return succedded;
    }


    public void EndDragSell(Cap draggedCap)
    {
        MoneyManager.Instance.AddMoney(draggedCap.capModificatorData.sellValues[draggedCap.capRank]);
        
        if (dragSlotOrigin is not null)
        {
            dragSlotOrigin.RemoveModificator();
            dragSlotOrigin = null;
        }
        
        Destroy(draggedCap.gameObject);
        
        ActualiseSlotsImages();
    }
    
    
    public void ShowPossibleSlots(Cap draggedCap)
    {
        for (int i = 0; i < turretSlots.Length; i++)
        {
            turretSlots[i].DisplayIsCompatible(draggedCap.capModificatorData, draggedCap.capRank, dragSlotOrigin);
        }
    }

    public void HidePossibleSlots(bool isPause)
    {
        for (int i = 0; i < turretSlots.Length; i++)
        {
            turretSlots[i].HideIsCompatible(isPause);
        }
    }

    #endregion
}
