using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Private Infos")] 
    private Vector3 moveDir;
    private float speed;
    private int damages;
    
    public void InitialiseBullet(Vector3 aimedPoint, float speed, int damages)
    {
        moveDir = (aimedPoint - transform.position).normalized;
        moveDir.y = 0;
        this.speed = speed;
        this.damages = damages;
    }

    private void Update()
    {
        transform.position += Time.deltaTime * speed * moveDir;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.gameObject.GetComponentInParent<EnemyMaster>().TakeDamage(damages);
    }
}
