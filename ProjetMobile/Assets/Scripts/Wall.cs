using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<EnemyMaster>().StopMovement();
    }
}
