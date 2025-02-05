using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class SaveManager : GenericSingletonClass<SaveManager>
{
    [SerializeField] private string fileName;
    [SerializeField] private bool newGame;
    
    private GameData gameData;
    private List<ISaveable> saveableObjects = new();
    private SaveFileWriter saveFileWriter;


    public override void Awake()
    {
        base.Awake();
        
        saveFileWriter = new SaveFileWriter(Application.persistentDataPath, fileName);
        saveableObjects = GetAllSaveableObjects();
        
        if(newGame)
            NewGame();
        
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
        saveFileWriter.Save(gameData);
        
        LoadGame();
    }
    
    public void SaveGame()
    {
        foreach (var saveable in saveableObjects)
        {
            saveable.SaveData(ref gameData);
        }
        
        saveFileWriter.Save(gameData);
    }

    public void LoadGame()
    {
        gameData = saveFileWriter.Load();
        
        if (gameData == null)
        {
            NewGame();
        }

        foreach (var saveable in saveableObjects)
        {
            saveable.LoadData(gameData);
        }
    }

    private List<ISaveable> GetAllSaveableObjects()
    {
        IEnumerable<ISaveable> saveableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        return new List<ISaveable>(saveableObjects);
    }
}
