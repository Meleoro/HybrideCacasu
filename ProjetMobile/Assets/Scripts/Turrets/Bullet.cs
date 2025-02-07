using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private AnimationCurve throwYCurve;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private LayerMask enemyLayer; 
    
    [Header("Private Infos")] 
    private Vector3 moveDir;
    private float speed;
    private int damages;
    private BulletBehavior behaviorBullet;
    private ShootBehavior behaviorShoot;

    [Header("Throw Private Infos")] 
    private float maxDist;
    private Vector3 endPoint;

    [Header("Explode Private Infos")] 
    private float explosionRange;

    [Header("Bullet Effects")] 
    private float slowStrength;
    private float burnStrength;
    
    
    public void InitialiseBullet(Vector3 aimedPoint, TurretData data, TurretModificatorValues modificatorValues, TurretModificatorValues upgradesValues)
    {
        Destroy(gameObject, 10f);
        
        moveDir = (aimedPoint - transform.position).normalized;
        moveDir.y = 0;

        transform.localScale = Vector3.one * (data.bulletSize * modificatorValues.projectileSizeMultiplier * upgradesValues.projectileSizeMultiplier);
        speed = data.bulletSpeed * modificatorValues.projectileSpeedMultiplier * upgradesValues.projectileSpeedMultiplier;
        damages = (int)(data.damages * modificatorValues.damageMultiplier * upgradesValues.damageMultiplier);
        behaviorBullet = data.bulletBehavior;
        behaviorShoot = data.shootBehavior;

        burnStrength = modificatorValues.burnStrength;
        slowStrength = modificatorValues.slowStrength;

        if (data.shootBehavior == ShootBehavior.Throw)
        {
            maxDist = -transform.position.z + aimedPoint.z;
            endPoint = aimedPoint;
        }

        if (data.bulletBehavior == BulletBehavior.Explose)
        {
            explosionRange = data.explosionRange;
        }

        transform.forward = moveDir;
    }   
    
    private void Update()
    {
        transform.position += Time.deltaTime * speed * moveDir;

        if (behaviorShoot == ShootBehavior.Throw)
        {
            ThrowBehavior();
        }
    }

    
    private void ThrowBehavior()
    {
        float progress = 1 - Mathf.Abs((endPoint.z - transform.position.z) / maxDist);
        float currentY = throwYCurve.Evaluate(progress);

        transform.position = new Vector3(transform.position.x, endPoint.y + currentY * 3f, transform.position.z);

        if (progress >= 0.99f || transform.position.z > endPoint.z)
        {
            ReachDestination();
        }
    }


    private void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRange, Vector3.one, 10f, enemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyMaster enemy = hits[i].collider.GetComponentInParent<EnemyMaster>();
            
            ApplySpecialEffects(enemy);
            enemy.TakeDamage(damages, transform.position);
        }

        Instantiate(explosionVFX, transform.position + Vector3.down * 0.15f, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
    }

    private void ApplySpecialEffects(EnemyMaster enemy)
    {
        if (burnStrength != 1)
        {
            StartCoroutine(enemy.BurnEnemyCoroutine(burnStrength, 5f));
        }

        if (slowStrength != 1)
        {
            StartCoroutine(enemy.SlowEnemyCoroutine(slowStrength, 1f));
        }
    }
    

    private void ReachDestination()
    {
        switch (behaviorBullet)
        {
            case BulletBehavior.Destroy :
                Destroy(gameObject);
                break;
            
            case BulletBehavior.Explose :
                Explode();
                break;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        EnemyMaster hitedEnemy = other.gameObject.GetComponentInParent<EnemyMaster>();
        hitedEnemy.TakeDamage(damages, transform.position);
        ApplySpecialEffects(hitedEnemy);

        switch (behaviorBullet)
        {
            case BulletBehavior.Destroy :
                Destroy(gameObject);
                break;
        }
    }
}
