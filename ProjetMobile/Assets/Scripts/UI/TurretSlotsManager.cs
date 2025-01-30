using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlotsManager : MonoBehaviour
{
    [Header("Private Infos")] 
    private TurretSlot currentOverlayedSlot;
    private TurretSlot dragSlotOrigin;

    [Header("References")] 
    [SerializeField] private TurretSlot[] turret1Slots;
    [SerializeField] private TurretSlot[] turret2Slots;
    [SerializeField] private TurretSlot[] turret3Slots;
    [SerializeField] private Image newModificatorImage;

    
    private void Start()
    {
        dragSlotOrigin = null;
        
        for (int i = 0; i < turret1Slots.Length; i++)
        {
            turret1Slots[i].InitialiseSlot(this);
        }
        for (int i = 0; i < turret2Slots.Length; i++)
        {
            turret2Slots[i].InitialiseSlot(this);
        }
        for (int i = 0; i < turret3Slots.Length; i++)
        {
            turret3Slots[i].InitialiseSlot(this);
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

    public void StartDrag(ModificatorData draggedData, int draggedRank, TurretSlot dragOrigin)
    {
        dragSlotOrigin = dragOrigin;
        HUDManager.Instance.StartDrag(draggedData, draggedRank);
    }
    
    public bool EndDrag(ModificatorData draggedData, int draggedRank)
    {
        if (currentOverlayedSlot is null) return false;

        ModificatorData modificatorData = null;
        int rank = 0;

        if (dragSlotOrigin is not null)
        {
            (modificatorData, rank) = currentOverlayedSlot.AddModificator(draggedData, draggedRank, true);
        
            dragSlotOrigin.RemoveModificator();
            if (modificatorData is not null)
            {
                dragSlotOrigin.AddModificator(modificatorData, rank);
            }

            dragSlotOrigin = null;
        }
        else
        {
            (modificatorData, rank) = currentOverlayedSlot.AddModificator(draggedData, draggedRank);
        }
        
        ActualiseSlotsImages();
        
        if (rank == 0) return true;
        return false;
    }

    #endregion
}
