using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Coin : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private float coinSpeed;
    [SerializeField] private float startImpulseForce;

    [Header("Private Infos")] 
    private Vector3 wantedPos;
    private int coinValue;
    
    [Header("References")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer sprite;
    
    
    public void Initialise(Vector3 wantedPos, int coinValue)
    {
        this.wantedPos = wantedPos;
        this.coinValue = coinValue;
        
        rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * startImpulseForce, ForceMode.Impulse);
    }
    
    private void FixedUpdate()
    {
        Vector3 forceDir = wantedPos - transform.position;
        
        if (forceDir.magnitude < 0.25f)
        {
            MoneyManager.Instance.AddMoney(coinValue);

            sprite.enabled = false;

            rb.isKinematic = true;
            Destroy(gameObject, 1f);
            Destroy(this);
        }

        transform.forward = CameraManager.Instance.transform.forward;
        rb.AddForce(forceDir.normalized * coinSpeed, ForceMode.Force);
    }
}
