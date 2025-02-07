using System;
using UnityEngine;
using Utilities;

public class Wall : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private MeshRenderer[] cheeseMeshRenderers;
    [SerializeField] private Transform[] spikesParents;
    
    public void ActualiseWallMeshes(int currentHealth, int maxHealth)
    {
        if (currentHealth == 0)
        {
            cheeseMeshRenderers[0].gameObject.SetActive(false);
            return;
        }
        
        float ratio = (float)currentHealth / maxHealth;
        ratio = (int)(ratio * cheeseMeshRenderers.Length);

        for (int i = 0; i < spikesParents.Length; i++)
        {
            if (currentHealth == maxHealth) continue;
            spikesParents[i].UShakeLocalPosition(0.4f, 0.15f, 0.025f);
        }
        
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
