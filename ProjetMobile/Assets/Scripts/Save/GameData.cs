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

        for (int i = 0; i < wonObjectives.Length; i++)
        {
            wonObjectives[i] = false;
        }
        
        for (int i = 0; i < turretsLevels.Length; i++)
        {
            turretsLevels[i] = 0;
        }
    }
}
