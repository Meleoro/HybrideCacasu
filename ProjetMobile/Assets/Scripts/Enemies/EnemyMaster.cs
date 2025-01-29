using System;
using System.Collections;
using UnityEngine;
using Utilities;

public class EnemyMaster : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private int enemyHealth;
    [SerializeField] private float enemySpeed;
    [SerializeField] private Vector2 moveDir;

    [Header("Private Infos")] 
    private int currentHealth;
    private Vector3 currentKnockback;

    [Header("References")] 
    [SerializeField] private Transform meshTr;
    [SerializeField] private ParticleSystem deathVFX;


    private void Start()
    {
        currentHealth = enemyHealth;
    }

    
    #region Move Functions
    
    private void Update()
    {
        transform.position += Time.deltaTime * enemySpeed * new Vector3(moveDir.x, 0, moveDir.y) + currentKnockback;
    }
    
    #endregion
    

    #region Damages Functions

    public void TakeDamage(int damages, Vector3 damageOrigin)
    {
        currentHealth -= damages;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            meshTr.UShakeLocalPosition(0.1f, 0.5f, false, true, false);
            StartCoroutine(KnockbackCoroutine(0.05f, 3f, new Vector3(0, 0, 1)));
        }
    }

    private IEnumerator KnockbackCoroutine(float duration, float strength, Vector3 direction)
    {
        float timer = 0;
        direction.y = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            currentKnockback = Time.deltaTime * Mathf.Lerp(0, strength, timer / duration) * direction;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        currentKnockback = Vector3.zero;
    }

    private void Die()
    {
        Instantiate(deathVFX, transform.position, Quaternion.Euler(0, 0, 0));
        
        EnemiesManager.Instance.KillEnemy(this);
    }

    #endregion
}
