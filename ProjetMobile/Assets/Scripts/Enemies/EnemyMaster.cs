using System;
using System.Collections;
using UnityEngine;
using Utilities;

public class EnemyMaster : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private int enemyHealth;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float cooldownBetweenAttacks = 0.1f;
    [SerializeField] private int attackDamages = 1;
    [SerializeField] private Vector2 moveDir;
    [SerializeField] private float moneyDropMultiplicator = 1f;

    [Header("Private Infos")] 
    private float currentHealth;
    private float currentSpeed;
    private Vector3 currentKnockback;
    private bool stopMove;

    [Header("References")] 
    [SerializeField] private Transform meshTr;
    [SerializeField] private ParticleSystem deathVFX;


    private void Start()
    {
        currentHealth = enemyHealth;
        currentSpeed = enemySpeed;
    }

    
    #region Move Functions
    
    private void Update()
    {
        ManageMovement();
    }
    
    public void StopMovement()
    {
        stopMove = true;
        StartCoroutine(StartAttackCoroutine());
    }
    
    private void ManageMovement()
    {
        if (stopMove) return;
        
        transform.position += Time.deltaTime * currentSpeed * (new Vector3(moveDir.x, 0, moveDir.y) + currentKnockback);
    }
    
    #endregion
    

    #region Damages Functions

    public IEnumerator BurnEnemyCoroutine(float multiplicator, float duration)
    {
        float timer = 0;
        float healthLostPerSecond = enemyHealth * multiplicator / duration;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;

            currentHealth -= healthLostPerSecond * Time.deltaTime;
            
            yield return new WaitForEndOfFrame();
        }
    }
    
    public IEnumerator SlowEnemyCoroutine(float multiplicator, float duration)
    {
        currentSpeed = enemySpeed - enemySpeed * (multiplicator - 1);

        yield return new WaitForSeconds(duration);

        currentSpeed = enemySpeed;
    }
    
    public void TakeDamage(int damages, Vector3 damageOrigin)
    {
        currentHealth -= damages;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            meshTr.UShakeLocalPosition(0.1f, 0.2f, 0.02f);
            StartCoroutine(KnockbackCoroutine(0.1f, 4f, new Vector3(0, 0, 1)));
        }
    }

    private IEnumerator KnockbackCoroutine(float duration, float strength, Vector3 direction)
    {
        float timer = 0;
        direction.y = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            currentKnockback = Mathf.Lerp(strength, 0, timer / duration) * direction;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        currentKnockback = Vector3.zero;
    }

    private void Die()
    {
        MoneyManager.Instance.CreateCoin(transform.position, (int)(GameManager.Instance.levelData.moneyPerEnemy * moneyDropMultiplicator));
        Instantiate(deathVFX, transform.position, Quaternion.Euler(0, 0, 0));
        
        EnemiesManager.Instance.KillEnemy(this);
    }

    #endregion


    #region Attack Functions

    private IEnumerator StartAttackCoroutine()
    {
        while (true)
        {
            HealthManager.Instance.LoseHealth(attackDamages);
        
            yield return new WaitForSeconds(cooldownBetweenAttacks);
        }
    }

    #endregion
}
