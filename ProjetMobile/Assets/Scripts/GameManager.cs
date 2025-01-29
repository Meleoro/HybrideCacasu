using System;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Parameters")] 
    public LevelData levelData;
    
    private void Start()
    {
        EnemiesManager.Instance.InitialiseEnemyManager(levelData);
        MoneyManager.Instance.AddMoney(levelData.startMoney);
    }
}
