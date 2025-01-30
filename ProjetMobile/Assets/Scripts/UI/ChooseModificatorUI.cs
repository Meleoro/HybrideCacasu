using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChooseModificatorUI : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private ModificatorData[] possibleModificators;
    [SerializeField] private int maxRankLevel;
    [SerializeField] private AnimationCurve rankCurve;
    [SerializeField] private float[] startProgressPerRank;
    [SerializeField] private float progressPerUpgrade;
    public Color[] colorsPerRanks;
    
    [Header("Private Infos")] 
    [SerializeField] private float[] currentProgresses;
    private bool isOpenned;
    [SerializeField]private float[] currentProbabilityPerRank;
    private float currentProbabilitiesSum;
    
    [Header("References")] 
    [SerializeField] private RectTransform[] modificatorUpgradeSlotsRectTr;
    [SerializeField] private ModificatorUpgradeSlotUI[] modificatorUpgradeSlotsScripts;


    private void Start()
    {
        currentProgresses = startProgressPerRank;
        ActualiseRanks();
        
        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            StartCoroutine(modificatorUpgradeSlotsScripts[i].DoCloseAnimCoroutine(true));
        }
    }


    private (ModificatorData, int) PickAModificator(ModificatorData[] chosenModificators)
    {
        int pickedModificatorIndex = Random.Range(0, possibleModificators.Length);
        float pickedRankProba = Random.Range(0, currentProbabilitiesSum);
        
        while (true)
        {
            bool isValidated = true;
            for (int i = 0; i < chosenModificators.Length; i++)
            {
                if (chosenModificators[i] != null)
                {
                    if (chosenModificators[i] == possibleModificators[pickedModificatorIndex])
                    {
                        isValidated = false;
                    }
                }
            }

            if(isValidated) break;
            
            pickedModificatorIndex = Random.Range(0, possibleModificators.Length);
            pickedRankProba = Random.Range(0f, currentProbabilitiesSum);
        }
        
        float cumulatedProba = 0;
        for (int i = 0; i < currentProbabilityPerRank.Length; i++)
        {
            cumulatedProba += currentProbabilityPerRank[i];
            if (pickedRankProba <= cumulatedProba)
            {
                return (possibleModificators[pickedModificatorIndex], i);
            }
        }

        return (possibleModificators[pickedModificatorIndex], 0);
    }


    private void ActualiseRanks()
    {
        currentProbabilityPerRank = new float[maxRankLevel];
        currentProbabilitiesSum = 0;
        
        for (int i = 0; i < maxRankLevel; i++)
        {
            float currentProgress = Mathf.Clamp(Mathf.Sin(currentProgresses[i] * (float)Math.PI * 0.5f), 0, 1);
            currentProbabilityPerRank[i] = rankCurve.Evaluate(currentProgress);

            currentProbabilitiesSum += currentProbabilityPerRank[i];
        }
    }
    
    public void UpgradeChest()
    {
        for (int i = 0; i < maxRankLevel; i++)
        {
            currentProgresses[i] += progressPerUpgrade;
        }

        ActualiseRanks();
    }
    
    
    public void OpenChoseUpgradeUI()
    {
        ModificatorData[] chosenModificators = new ModificatorData[3];
        int[] chosenRanks = new int[3];

        if (isOpenned) return;
        isOpenned = true;
        
        for (int i = 0; i < 3; i++)
        {
            (chosenModificators[i], chosenRanks[i]) = PickAModificator(chosenModificators);
        }

        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            modificatorUpgradeSlotsScripts[i].SetCurrentData(chosenModificators[i], chosenRanks[i]);
            StartCoroutine(modificatorUpgradeSlotsScripts[i].DoOpenAnimCoroutine());
        }
    }
    
    public void CloseChoseUpgradeUI()
    {
        if (!isOpenned) return;
        isOpenned = false;
        
        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            StartCoroutine(modificatorUpgradeSlotsScripts[i].DoCloseAnimCoroutine(false));
        }
    }

    public void StartDrag(ModificatorData draggedData, int draggedRank)
    {
        HUDManager.Instance.StartDrag(draggedData, draggedRank);
    }
}
