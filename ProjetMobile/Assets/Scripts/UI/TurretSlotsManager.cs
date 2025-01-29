using System;
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

    
    private void Start()
    {
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
            (ModificatorData modificatorData, int stackAmount) = slots[i].GetCurrentModificator();
            if (modificatorData == null) continue;

            switch (modificatorData.modificatorType)
            {
                case ModificatorType.Damage:
                    returnedValues.damageMultiplier += modificatorData.modificatorImpact * stackAmount;
                    break;
                
                case ModificatorType.FireRate:
                    returnedValues.fireRateMultiplier += modificatorData.modificatorImpact * stackAmount;
                    break;
            }
        }

        return returnedValues;
    }

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

    public void StartDrag(Sprite draggedSprite, TurretSlot dragOrigin)
    {
        dragSlotOrigin = dragOrigin;
        HUDManager.Instance.StartDrag(draggedSprite);
    }
    
    public void EndDrag()
    {
        if (currentOverlayedSlot is null) return;
        
        (ModificatorData modificatorData, int stack) = dragSlotOrigin.GetCurrentModificator();
        (modificatorData, stack) = currentOverlayedSlot.AddModificator(modificatorData, stack, true);
        
        dragSlotOrigin.RemoveModificator();
        if (modificatorData is not null)
        {
            dragSlotOrigin.AddModificator(modificatorData, stack);
        }
        
        ActualiseSlotsImages();
    }

    #endregion
}
