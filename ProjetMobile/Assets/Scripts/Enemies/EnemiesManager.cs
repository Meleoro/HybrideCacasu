 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : GenericSingletonClass<EnemiesManager>
{
    [Header("Parameters")] 
    [SerializeField] private float deadZoneHeight;
    
    [Header("Private Infos")]
    private LevelData levelData;
    private List<EnemyMaster> currentEnemies = new List<EnemyMaster>();
    private Coroutine constantSpawnCoroutine;
    private bool gameNeedsToEnd;

    [Header("References")] 
    [SerializeField] private Transform enemySpawnXMinRef;
    [SerializeField] private Transform enemySpawnXMaxRef;


    public void InitialiseEnemyManager(LevelData levelData)
    {
        this.levelData = levelData;
        
        StartCoroutine(ManageEnemyWavesCoroutine());
        constantSpawnCoroutine = StartCoroutine(EnemiesConstantSpawnCoroutine());
    }
    

    #region Waves Functions

    private IEnumerator ManageEnemyWavesCoroutine()
    {
        int currentWaveIndex = 0;
        float timerSpent = 0;

        while (currentWaveIndex < levelData.waves.Length)
        {
            if(gameNeedsToEnd) yield break;

            yield return new WaitForSeconds(levelData.waves[currentWaveIndex].waveWaitDuration - timerSpent);
 
            StartCoroutine(SpawnEnemyWaveCoroutine(levelData.waves[currentWaveIndex]));
            timerSpent = levelData.waves[currentWaveIndex].waveWaitDuration;
            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnEnemyWaveCoroutine(WaveStruct currentWave)
    {
        for (int i = 0; i < currentWave.waveEnemies.Length; i++)
        {
            for (int j = 0; j < currentWave.waveEnemies[i].amount; j++)
            {
                if(gameNeedsToEnd) yield break;
                
                SpawnEnemy(currentWave.waveEnemies[i].enemyType);

                yield return new WaitForSeconds(currentWave.waveDelayBetweenSpawn);
            }
        }
    }

    #endregion


    #region Constant Spawn

    private IEnumerator EnemiesConstantSpawnCoroutine()
    {
        if(levelData.spawnableEnemies.Length == 0) yield break;
        
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Lerp(levelData.startConstantEnemyDelaySpawn,
                levelData.endConstantEnemyDelaySpawn, GameManager.Instance.currentTimer / levelData.levelDuration));

            int spawnedEnemyIndex = Random.Range(0, levelData.spawnableEnemies.Length);
            
            SpawnEnemy(levelData.spawnableEnemies[spawnedEnemyIndex]);
        }
    }
    
    private void SpawnEnemy(EnemyMaster enemy)
    {
        Vector3 spawnPos = enemySpawnXMinRef.position + new Vector3(Random.Range(0, Mathf.Abs(enemySpawnXMinRef.position.x - enemySpawnXMaxRef.position.x)), 0, 0);

        EnemyMaster spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        currentEnemies.Add(spawnedEnemy);
    }

    #endregion


    #region Others

    public void EndGame()
    {
        gameNeedsToEnd = true;
        
        StopCoroutine(constantSpawnCoroutine);
        StartCoroutine(VerifyGameEndCoroutine());
    }

    private IEnumerator VerifyGameEndCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (currentEnemies.Count == 0)
            {
                StartCoroutine(HUDManager.Instance.endScreen.DisplayWinCoroutine());
                
                yield break;
            }
        }
    }
    
    public Vector3 FindNearestEnemy()
    {
        if (currentEnemies.Count == 0) return new Vector3(0, 0, 0);
        
        float bestDist = deadZoneHeight;
        int bestIndex = -1;
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            float dist = currentEnemies[i].transform.position.z;
            
            if (dist < bestDist)
            {
                bestDist = dist;
                bestIndex = i;
            }
        }
        
        if(bestIndex == -1) return new Vector3(0, 0, 0);

        return currentEnemies[bestIndex].transform.position;
    }


    public void KillEnemy(EnemyMaster killedEnemy)
    {
        currentEnemies.Remove(killedEnemy);
        Destroy(killedEnemy.gameObject);
    }

    #endregion
}
