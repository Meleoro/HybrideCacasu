using UnityEngine;

[System.Serializable]
public class GameData
{
    public int ownedSoftCurrency;
    public bool[] wonObjectives;
    public int[] turretsLevels;
    
    public GameData()
    {
        ownedSoftCurrency = 0;
        wonObjectives = new bool[9000];
        turretsLevels = new int[10];
    }
}
