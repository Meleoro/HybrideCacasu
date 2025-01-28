using System;
using System.Collections;
using UnityEngine;

public class TurretMaster : MonoBehaviour
{
    [SerializeField] private TurretData turretData;

    [Header("References")] 
    [SerializeField] private GameObject bulletPrefab;

    private void Start()
    {
        
    }

    private IEnumerator ShootBehaviorCoroutine()
    {
        yield return new WaitForSeconds(turretData.fireRate);
        
        
    }

    private void Shoot()
    {
        
    }
}
