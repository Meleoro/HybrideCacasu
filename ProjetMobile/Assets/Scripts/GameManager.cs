using System;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Parameters")] 
    public LevelData levelData;

    [Header("Public Infos")] 
    public float currentTimer;
    
        
    private void Start()
    {
        EnemiesManager.Instance.InitialiseEnemyManager(levelData);
        MoneyManager.Instance.AddMoney(levelData.startMoney);
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
    }
}
