using System;
using System.Collections;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Public Infos")] 
    public LevelData levelData;

    [Header("Public Infos")] 
    public float currentTimer;
    
        
    private void Start()
    {
        Time.timeScale = 1;
        
        if (DontDestroyOnLoadObject.Instance != null)
        {
            levelData = DontDestroyOnLoadObject.Instance.levelData;
        }
        
        EnemiesManager.Instance.InitialiseEnemyManager(levelData);
        MoneyManager.Instance.AddMoney(levelData.startMoney);
        HUDManager.Instance.InitialiseHUD();

        StartCoroutine(ActualiseTimerCoroutine());
    }
    
    
    private IEnumerator ActualiseTimerCoroutine(){

        while (currentTimer < levelData.levelDuration)
        {
            currentTimer += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }

        EnemiesManager.Instance.EndGame();
    }
}
