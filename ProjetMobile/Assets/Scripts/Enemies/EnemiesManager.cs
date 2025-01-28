using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    [Header("Private Infos")]
    [SerializeField] private LevelData levelData;
    private float currentTimer;
    
    [Header("Private Infos")] 
    private List<EnemyMaster> currentEnemies = new List<EnemyMaster>();

    [Header("References")] 
    [SerializeField] private Transform[] enemiesSpawns;


    private void InitialiseEnemyManager(LevelData levelData)
    {
        if(levelData != null)
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


    private void SpawnEnemy()
    {
        int spawnedEnemyIndex = Random.Range(0, levelData.spawnableEnemies.Length);
        Vector3 spawnPos = enemiesSpawns[Random.Range(0, enemiesSpawns.Length)].position;

        EnemyMaster spawnedEnemy = Instantiate(levelData.spawnableEnemies[spawnedEnemyIndex], spawnPos, Quaternion.identity);
        currentEnemies.Add(spawnedEnemy);
    }


    private void KillEnemy(EnemyMaster killedEnemy)
    {
        currentEnemies.Remove(killedEnemy);
        Destroy(killedEnemy);
    }
    
}
