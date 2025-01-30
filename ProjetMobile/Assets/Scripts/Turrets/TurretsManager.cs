using System;
using UnityEngine;

public class TurretsManager : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private TurretMaster[] turretPrefabs;

    [Header("References")] 
    [SerializeField] private Transform[] turretSpawns;


    private void Start()
    {
        InitialiseTurrets();
    }

    public void InitialiseTurrets()
    {
        for (int i = 0; i < turretSpawns.Length; i++)
        {
            TurretMaster newTurret = Instantiate(turretPrefabs[i], turretSpawns[i].transform.position, Quaternion.identity);
            newTurret.InitialiseTurret(i);
        }
    }
}
