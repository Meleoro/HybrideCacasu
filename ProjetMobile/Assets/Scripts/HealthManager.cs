using System;
using TMPro;
using UnityEngine;

public class HealthManager : GenericSingletonClass<HealthManager>
{
    [Header("Parameters")] 
    [SerializeField] private int startHealth;

    [Header("Private Infos")] 
    private int currentHealth;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Wall cheeseWallScript;


    private void Start()
    {
        currentHealth = startHealth;
        ActualiseHealthText();
    }
    
    private void ActualiseHealthText()
    {
        healthText.text = currentHealth.ToString();
        
        cheeseWallScript.ActualiseWallMeshes(currentHealth, startHealth);
    }


    public void LoseHealth(int damages)
    {
        currentHealth -= damages;
        ActualiseHealthText();
        
        CameraManager.Instance.DoHurtEffect();

        if (currentHealth <= 0)
        {
            StartCoroutine(HUDManager.Instance.endScreen.DisplayLoseCoroutine());
            Time.timeScale = 0;
        }
    }
}
