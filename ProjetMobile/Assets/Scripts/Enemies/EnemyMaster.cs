using System;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private int enemyHealth;
    [SerializeField] private float enemySpeed;
    [SerializeField] private Vector2 moveDir;

    [Header("Private Infos")] 
    private int currentHealth;


    private void Start()
    {
        currentHealth = enemyHealth;
    }

    
    #region Move Functions
    
    private void Update()
    {
        transform.position += Time.deltaTime * enemySpeed * new Vector3(moveDir.x, 0, moveDir.y);
    }
    
    #endregion
    

    #region Damages Functions

    public void TakeDamage(int damages)
    {
        currentHealth -= damages;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        EnemiesManager.Instance.KillEnemy(this);
    }

    #endregion
}
