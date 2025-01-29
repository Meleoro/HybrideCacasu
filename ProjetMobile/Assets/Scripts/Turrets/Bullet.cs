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
    
    
    public void InitialiseBullet(Vector3 aimedPoint, TurretData data)
    {
        Destroy(gameObject, 10f);
        
        moveDir = (aimedPoint - transform.position).normalized;
        moveDir.y = 0;

        transform.localScale = Vector3.one * data.bulletSize;
        speed = data.bulletSpeed;
        damages = data.damages;
        behaviorBullet = data.bulletBehavior;
        behaviorShoot = data.shootBehavior;

        if (data.shootBehavior == ShootBehavior.Throw)
        {
            maxDist = -transform.position.z + aimedPoint.z;

            endPoint = aimedPoint;
        }

        if (data.bulletBehavior == BulletBehavior.Explose)
        {
            explosionRange = data.explosionRange;
        }
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

        if (progress >= 0.99f)
        {
            ReachDestination();
        }
    }


    private void Explode()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, explosionRange, Vector3.one, 10f, enemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].collider.GetComponentInParent<EnemyMaster>().TakeDamage(damages, transform.position);
        }

        Instantiate(explosionVFX, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(gameObject);
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
        other.gameObject.GetComponentInParent<EnemyMaster>().TakeDamage(damages, transform.position);

        switch (behaviorBullet)
        {
            case BulletBehavior.Destroy :
                Destroy(gameObject);
                break;
        }
    }
}
