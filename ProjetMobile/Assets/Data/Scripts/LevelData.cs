using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Global Infos")] 
    public float levelDuration;
    public EnemyMaster[] spawnableEnemies;
    
    [Header("Pacing Infos")] 
    public float startConstantEnemyDelaySpawn;
    public float endConstantEnemyDelaySpawn;
}
