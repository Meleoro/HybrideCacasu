using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public struct TurretModificatorValues
{
    public TurretModificatorValues(float baseDamage = 1, float baseFireRate = 1)
    {
        damageMultiplier = baseDamage;
        fireRateMultiplier = baseFireRate;
        addedProjectiles = 0;
        projectileSizeMultiplier = 1;
        projectileSpeedMultiplier = 1;
        slowStrength = 1;
        burnStrength = 1;
    }
    
    public float damageMultiplier;
    public float fireRateMultiplier;
    public int addedProjectiles;
    public float projectileSizeMultiplier;
    public float projectileSpeedMultiplier;
    public float slowStrength;
    public float burnStrength;
}


public class TurretMaster : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private TurretData turretData;
    [SerializeField][Range(0, 2)] private int turretIndex;

    [Header("Private Infos")] 
    private TurretModificatorValues modificatorValues = new TurretModificatorValues(1, 1);
    private TurretModificatorValues upgradeValues = new TurretModificatorValues(1, 1);
    private Vector3 aimedPoint;
    private float upgradeDamageMutliplier;
    private float upgradeFireRateMutliplier;
    private float upgradeProjectileSizeMutliplier;
    private float upgradeProjectileSpeedMutliplier;

    [Header("References")] 
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private ParticleSystem shootVFX;
    [SerializeField] private ParticleSystem upgradeVFX;
    [SerializeField] private Transform jointToRotate;
    [SerializeField] private Animator anim;
    

    public void InitialiseTurret(int turretIndex)
    {
        this.turretIndex = turretIndex;
        upgradeValues = new TurretModificatorValues(1);
        
        HUDManager.Instance.OnModificatorDragEndAction += ActualiseModificators;
        modificatorValues = new TurretModificatorValues(1, 1);
        StartCoroutine(ShootBehaviorCoroutine());

        HUDManager.Instance.turretScripts.Add(this);

        if (DontDestroyOnLoadObject.Instance != null)
        {
            turretData.ActualiseTurretLevel(DontDestroyOnLoadObject.Instance.turretsLevels[turretData.turretIndex]);
        }
    }
    

    private void Update()
    {
        RotateTurret();
    }

    private void RotateTurret()
    {
        aimedPoint = EnemiesManager.Instance.FindNearestEnemy();

        if (aimedPoint != Vector3.zero)
        {
            Vector3 aimedDir = aimedPoint - jointToRotate.position;
            float angle = -Mathf.Atan2(aimedDir.z, aimedDir.x) * Mathf.Rad2Deg + 90;
            jointToRotate.localRotation = Quaternion.Euler(jointToRotate.eulerAngles.x, angle, jointToRotate.eulerAngles.z);
        }
    }

    private void ActualiseModificators()
    {
        bool bounce;
        (modificatorValues, bounce) = HUDManager.Instance.turretSlotsManager.GetTurretModificators(turretIndex);

        if (bounce)
        {
            transform.USquishEffect(0.3f, 0.15f, false, CurveType.None);
        }
    }

    public void UpgradeTurret(float damage, float fireRate, float size, float speed)
    {
        upgradeValues.damageMultiplier += damage;
        upgradeValues.fireRateMultiplier += fireRate;
        upgradeValues.projectileSizeMultiplier += size;
        upgradeValues.projectileSpeedMultiplier += speed;
        
        upgradeVFX.Play();
        
        transform.UStopSquish();
        transform.localScale = Vector3.one;
        transform.USquishEffect(0.3f, 0.15f, false, CurveType.None);
    }


    #region Shoot Functions

    private IEnumerator ShootBehaviorCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(turretData.shootCooldown / (modificatorValues.fireRateMultiplier + upgradeValues.fireRateMultiplier - 1));

            if (aimedPoint == Vector3.zero) continue;
            StartCoroutine(ShootCoroutine(EnemiesManager.Instance.FindNearestEnemy()));
        }
    }

    private IEnumerator ShootCoroutine(Vector3 aimedPoint)
    {
        anim.SetTrigger("Shoot");
        
        shootVFX.Play();
        
        for (int i = 0; i < turretData.bulletCount + modificatorValues.addedProjectiles; i++)
        {
            Bullet newBullet = Instantiate(bulletPrefab, shootVFX.transform.position, Quaternion.identity);
            newBullet.InitialiseBullet(aimedPoint + new Vector3(Random.Range(-turretData.shootDispersion, turretData.shootDispersion), 0, 0), 
                turretData, modificatorValues, upgradeValues);

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
}
