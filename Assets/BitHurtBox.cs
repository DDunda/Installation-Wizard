using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitHurtBox : BitEnemy
{
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
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("DealDamage");
            plrHealth.ChangeHealth(-dmg);
            return;
        }
    }
}
