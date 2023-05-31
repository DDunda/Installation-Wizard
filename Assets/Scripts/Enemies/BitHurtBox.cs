using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitHurtBox : BitEnemy
{
    [SerializeField] private iFrameHealth healthScript; // to track current health

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected override void Update()
    {
        rb.transform.localPosition = Vector3.zero;
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collision detected with " + col.name);
        if (col.gameObject.CompareTag("PlayerHitBox") && healthScript.Health > 0)
        {
            Debug.Log("DealDamage");
            plrHealth.ChangeHealth(-dmg);
            return;
        }
    }
}
