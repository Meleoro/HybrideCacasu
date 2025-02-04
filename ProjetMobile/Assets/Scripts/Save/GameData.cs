using UnityEngine;

[System.Serializable]
public class GameData
{
    public int ownedSoftCurrency;
    public bool[] wonObjectives;
    
    public GameData()
    {
        this.ownedSoftCurrency = 0;
        wonObjectives = new bool[9000];
    }
}
