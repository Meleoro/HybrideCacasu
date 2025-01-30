using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : GenericSingletonClass<EnemiesManager>
{
    [Header("Private Infos")]
    private LevelData levelData;
    private float currentTimer;
    
    [Header("Private Infos")] 
    private List<EnemyMaster> currentEnemies = new List<EnemyMaster>();

    [Header("References")] 
    [SerializeField] private Transform enemySpawnXMinRef;
    [SerializeField] private Transform enemySpawnXMaxRef;


    public void InitialiseEnemyManager(LevelData levelData)
    {
        this.levelData = levelData;

        currentTimer = 0;
        StartCoroutine(EnemiesConstantSpawnCoroutine());
        StartCoroutine(ManageEnemyWavesCoroutine());
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
    }


    #region Waves Functions

    private IEnumerator ManageEnemyWavesCoroutine()
    {
        int currentWaveIndex = 0;

        while (currentWaveIndex < levelData.waves.Length)
        {
            yield return new WaitForSeconds(levelData.waves[currentWaveIndex].waveWaitDuration);

            StartCoroutine(SpawnEnemyWaveCoroutine(levelData.waves[currentWaveIndex]));
        }
    }

    private IEnumerator SpawnEnemyWaveCoroutine(WaveStruct currentWave)
    {
        for (int i = 0; i < currentWave.waveEnemies.Length; i++)
        {
            for (int j = 0; j < currentWave.waveEnemies[i].amount; j++)
            {
                SpawnEnemy(currentWave.waveEnemies[i].enemyType);

                yield return new WaitForSeconds(currentWave.waveDelayBetweenSpawn);
            }
        }
    }

    #endregion


    #region Constant Spawn

    private IEnumerator EnemiesConstantSpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Lerp(levelData.startConstantEnemyDelaySpawn,
                levelData.endConstantEnemyDelaySpawn, currentTimer / levelData.levelDuration));

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

    public Vector3 FindNearestEnemy()
    {
        if (currentEnemies.Count == 0) return new Vector3(0, 0, 0);
        
        float bestDist = Mathf.Infinity;
        int bestIndex = 0;
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            float dist = currentEnemies[i].transform.position.z;
            
            if (dist < bestDist)
            {
                bestDist = dist;
                bestIndex = i;
            }
        }

        return currentEnemies[bestIndex].transform.position;
    }


    public void KillEnemy(EnemyMaster killedEnemy)
    {
        currentEnemies.Remove(killedEnemy);
        Destroy(killedEnemy.gameObject);
    }

    #endregion
}
