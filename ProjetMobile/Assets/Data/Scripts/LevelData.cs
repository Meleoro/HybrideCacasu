using System;
using UnityEngine;

[Serializable]
public struct WaveStruct
{
    public float waveWaitDuration;
    public float waveDelayBetweenSpawn;
    public WaveEnemyStruct[] waveEnemies;
}

[Serializable]
public struct WaveEnemyStruct
{
    public EnemyMaster enemyType;
    [Range(1, 25)] public int amount;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Global Infos")] 
    public string levelName;
    public float levelDuration;
    public EnemyMaster[] spawnableEnemies;

    [Header("Waves Infos")] 
    public WaveStruct[] waves;
    
    [Header("Constant Pacing Infos")] 
    public float startConstantEnemyDelaySpawn;
    public float endConstantEnemyDelaySpawn;
    
    [Header("Money Infos")] 
    public int startMoney;
    public int moneyPerEnemy;
    public int softCurrencyWon;

    [Header("Objectives infos")] 
    public int[] durationObjectives;
}
