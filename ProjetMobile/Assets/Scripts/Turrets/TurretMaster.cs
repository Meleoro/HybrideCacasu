using System;
using System.Collections;
using UnityEngine;

public struct TurretModificatorValues
{
    public float damageMultiplier;
    public float fireRateMultiplier;
}


public class TurretMaster : MonoBehaviour
{
    [SerializeField] private TurretData turretData;

    [Header("References")] 
    [SerializeField] private Bullet bulletPrefab;

    private void Start()
    {
        StartCoroutine(ShootBehaviorCoroutine());
    }

    private void Update()
    {
        RotateTurret();
    }

    private void RotateTurret()
    {
        Vector3 aimedPoint = EnemiesManager.Instance.FindNearestEnemy(transform.position);

        if (aimedPoint != Vector3.zero)
        {
            Vector3 aimedDir = aimedPoint - transform.position;
            float angle = -Mathf.Atan2(aimedDir.z, aimedDir.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
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
