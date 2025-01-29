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
    [SerializeField] private Transform[] enemiesSpawns;


    public void InitialiseEnemyManager(LevelData levelData)
    {
        this.levelData = levelData;

        currentTimer = 0;
        StartCoroutine(EnemiesConstantSpawnCoroutine());
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
    }


    private IEnumerator EnemiesConstantSpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Lerp(levelData.startConstantEnemyDelaySpawn,
                levelData.endConstantEnemyDelaySpawn, currentTimer / levelData.levelDuration));

            SpawnEnemy();
        }
    }


    public Vector3 FindNearestEnemy(Vector3 turretPos)
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
    
    
    private void SpawnEnemy()
    {
        int spawnedEnemyIndex = Random.Range(0, levelData.spawnableEnemies.Length);
        Vector3 spawnPos = enemiesSpawns[Random.Range(0, enemiesSpawns.Length)].position;

        EnemyMaster spawnedEnemy = Instantiate(levelData.spawnableEnemies[spawnedEnemyIndex], spawnPos, Quaternion.identity);
        currentEnemies.Add(spawnedEnemy);
    }


    public void KillEnemy(EnemyMaster killedEnemy)
    {
        currentEnemies.Remove(killedEnemy);
        Destroy(killedEnemy.gameObject);
    }
    
}
