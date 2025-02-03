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


    private void Start()
    {
        currentHealth = startHealth;
        ActualiseHealthText();
    }
    
    private void ActualiseHealthText()
    {
        healthText.text = currentHealth.ToString();
    }


    public void LoseHealth(int damages)
    {
        currentHealth -= damages;
        ActualiseHealthText();
        
        CameraManager.Instance.DoHurtEffect();
    }
}
