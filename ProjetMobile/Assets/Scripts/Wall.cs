using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private MeshRenderer[] cheeseMeshRenderers;
    
    public void ActualiseWallMeshes(int currentHealth, int maxHealth)
    {
        if (currentHealth == 0)
        {
            cheeseMeshRenderers[0].gameObject.SetActive(false);
            return;
        }
        
        float ratio = (float)currentHealth / maxHealth;
        ratio = (int)(ratio * cheeseMeshRenderers.Length);

        for (int i = cheeseMeshRenderers.Length - 1; i > ratio; i--)
        {
            cheeseMeshRenderers[i].gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<EnemyMaster>().StopMovement();
    }
}
