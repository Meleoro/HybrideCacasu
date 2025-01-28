using System;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Parameters")] 
    [SerializeField] private LevelData levelData;
    
    
    private void Start()
    {
        EnemiesManager.Instance.InitialiseEnemyManager(levelData);
    }
}
