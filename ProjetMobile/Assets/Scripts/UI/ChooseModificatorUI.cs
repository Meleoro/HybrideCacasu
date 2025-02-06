using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Random = UnityEngine.Random;

public class GetNewModificatorUI : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private ModificatorData[] possibleModificators;
    [SerializeField] private int maxRankUpAmount;
    [SerializeField] private AnimationCurve rankCurve;
    [SerializeField] private float[] startProgressPerRank;
    [SerializeField] private float progressPerUpgrade;
    
    [Header("Public Infos")]
    public bool isOpenned;
    
    [Header("Private Infos")] 
    [SerializeField] private float[] currentProgresses;
    [SerializeField]private float[] currentProbabilityPerRank;
    private float currentProbabilitiesSum;
    private int currentRankUpAmount;
    
    [Header("References")] 
    [SerializeField] private RectTransform[] modificatorUpgradeSlotsRectTr;
    [SerializeField] private GetNewModificatorUISlot[] modificatorUpgradeSlotsScripts;
    [SerializeField] private Image backImage;


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

            if (!isValidated) continue;
            
            pickedModificatorIndex = Random.Range(0, possibleModificators.Length);
            pickedRankProba = Random.Range(0f, currentProbabilitiesSum);
            
            float cumulatedProba = 0;
            for (int i = 0; i < currentProbabilityPerRank.Length; i++)
            {
                cumulatedProba += currentProbabilityPerRank[i];
                if (pickedRankProba <= cumulatedProba)
                {
                    if (i >= possibleModificators[pickedModificatorIndex].minimumRank && isValidated)
                    {
                        return (possibleModificators[pickedModificatorIndex], i);
                    }
                    if(i < possibleModificators[pickedModificatorIndex].minimumRank)
                    {
                        isValidated = false;
                    }
                }
            }
        }

        return (possibleModificators[pickedModificatorIndex], 0);
    }


    private void ActualiseRanks()
    {
        currentProbabilityPerRank = new float[startProgressPerRank.Length];
        currentProbabilitiesSum = 0;
        
        for (int i = 0; i < startProgressPerRank.Length; i++)
        {
            float currentProgress = Mathf.Clamp(Mathf.Sin(currentProgresses[i] * (float)Math.PI * 0.5f), 0, 1);
            currentProbabilityPerRank[i] = rankCurve.Evaluate(currentProgress);

            currentProbabilitiesSum += currentProbabilityPerRank[i];
        }
    }
    
    public void UpgradeChest()
    {
        if (currentRankUpAmount > maxRankUpAmount) return;
        
        for (int i = 0; i < startProgressPerRank.Length; i++)
        {
            currentProgresses[i] += progressPerUpgrade;
        }

        currentRankUpAmount++;
        ActualiseRanks();
    }
    
    
    public IEnumerator OpenChoseUpgradeUICoroutine()
    {
        ModificatorData[] chosenModificators = new ModificatorData[3];
        int[] chosenRanks = new int[3];
        
        if (isOpenned) yield break;
        isOpenned = true;
        
        Time.timeScale = 0.05f;
        backImage.UFadeImage(0.5f, 0.5f, CurveType.None, true);
        
        for (int i = 0; i < 3; i++)
        {
            (chosenModificators[i], chosenRanks[i]) = PickAModificator(chosenModificators);
        }

        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            modificatorUpgradeSlotsScripts[i].SetCurrentData(chosenModificators[i], chosenRanks[i]);
            StartCoroutine(modificatorUpgradeSlotsScripts[i].DoOpenAnimCoroutine());

            yield return new WaitForSecondsRealtime(0.4f);
        }
    }
    
    public void CloseChoseUpgradeUI()
    {
        if (!isOpenned) return;
        isOpenned = false;
        
        backImage.UFadeImage(0.5f, 0f, CurveType.None, true);
        
        Time.timeScale = 1f;
        
        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            StartCoroutine(modificatorUpgradeSlotsScripts[i].DoCloseAnimCoroutine(false));
        }
    }

    public void StartDrag(Cap draggedCap)
    {
        HUDManager.Instance.StartDrag(draggedCap , true);
    }

    public void EndDrag()
    {
        if (!isOpenned) return;
        
        for (int i = 0; i < modificatorUpgradeSlotsScripts.Length; i++)
        {
            modificatorUpgradeSlotsScripts[i].StopDrag();
        }
    }
}
