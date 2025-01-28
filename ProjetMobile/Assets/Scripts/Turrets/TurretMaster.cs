using System;
using System.Collections;
using UnityEngine;

public class TurretMaster : MonoBehaviour
{
    [SerializeField] private TurretData turretData;

    [Header("References")] 
    [SerializeField] private Bullet bulletPrefab;

    private void Start()
    {
        StartCoroutine(ShootBehaviorCoroutine());
    }

    private IEnumerator ShootBehaviorCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(turretData.fireRate);

            Shoot(EnemiesManager.Instance.FindNearestEnemy(transform.position));
        }
    }

    private void Shoot(Vector3 aimedPoint)
    {
        Bullet newBullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        newBullet.InitialiseBullet(aimedPoint, turretData.bulletSpeed, turretData.bulletsDamages);
    }
}
