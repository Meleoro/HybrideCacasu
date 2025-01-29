using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public struct TurretModificatorValues
{
    public TurretModificatorValues(float baseDamage = 1, float baseFireRate = 1)
    {
        damageMultiplier = baseDamage;
        fireRateMultiplier = baseFireRate;
    }
    
    public float damageMultiplier;
    public float fireRateMultiplier;
}


public class TurretMaster : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private TurretData turretData;
    [SerializeField][Range(0, 2)] private int turretIndex;

    [Header("Private Infos")] 
    private TurretModificatorValues modificatorValues = new TurretModificatorValues(1, 1);

    [Header("References")] 
    [SerializeField] private Bullet bulletPrefab;

    
    private void Start()
    {
        HUDManager.Instance.OnModificatorDragEndAction += ActualiseModificators;
        
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

    private void ActualiseModificators()
    {
        modificatorValues = HUDManager.Instance.turretSlotsManager.GetTurretModificators(turretIndex);
    }


    #region Shoot Functions

    private IEnumerator ShootBehaviorCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(turretData.shootCooldown / modificatorValues.fireRateMultiplier);
            
            Shoot(EnemiesManager.Instance.FindNearestEnemy(transform.position));
        }
    }

    private void Shoot(Vector3 aimedPoint)
    {
        for (int i = 0; i < turretData.bulletCount; i++)
        {
            Bullet newBullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            newBullet.InitialiseBullet(aimedPoint + new Vector3(Random.Range(-turretData.shootDispersion, turretData.shootDispersion), 0, 0), turretData);
        }
    }

    #endregion
}
