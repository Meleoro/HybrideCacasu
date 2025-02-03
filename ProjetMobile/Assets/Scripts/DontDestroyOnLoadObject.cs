using System;
using UnityEngine;


public class DontDestroyOnLoadObject : GenericSingletonClass<DontDestroyOnLoadObject>
{
    [Header("Public Infos")] 
    public LevelData levelData;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
