using System;
using UnityEngine;


public class DontDestroyOnLoadObject : GenericSingletonClass<DontDestroyOnLoadObject>, ISaveable
{
    [Header("Public Infos")] 
    public LevelData levelData;
    public int currentLevelIndex;
    public int ownedSoftCurrency;
    public bool[] wonObjectives;
    
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void ActualiseSoftCurrency(int newSoftCurrency)
    {
        ownedSoftCurrency = newSoftCurrency;
        
        SaveManager.Instance.SaveGame();
    }
    
    public void SaveLevelFinishedObjectives(bool[] levelWonObjectives)
    {
        wonObjectives[currentLevelIndex * 3] = levelWonObjectives[0];
        wonObjectives[1 + currentLevelIndex * 3] = levelWonObjectives[1];
        wonObjectives[2 + currentLevelIndex * 3] = levelWonObjectives[2];
        
        SaveManager.Instance.SaveGame();
    }
    
    public void LoadData(GameData data)
    {
        ownedSoftCurrency = data.ownedSoftCurrency;
        wonObjectives = data.wonObjectives;
    }

    public void SaveData(ref GameData data)
    {
        data.ownedSoftCurrency = ownedSoftCurrency;
        data.wonObjectives = wonObjectives;
    }
}
