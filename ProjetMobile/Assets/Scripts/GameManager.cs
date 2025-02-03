using System;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Public Infos")] 
    public LevelData levelData;

    [Header("Public Infos")] 
    public float currentTimer;
    
        
    private void Start()
    {
        if (DontDestroyOnLoadObject.Instance != null)
        {
            levelData = DontDestroyOnLoadObject.Instance.levelData;
        }
        
        EnemiesManager.Instance.InitialiseEnemyManager(levelData);
        MoneyManager.Instance.AddMoney(levelData.startMoney);
        HUDManager.Instance.InitialiseHUD();
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
    }
}
