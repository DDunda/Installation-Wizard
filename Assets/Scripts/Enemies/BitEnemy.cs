using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BitEnemy : BaseEnemy
{
    [SerializeField] private float minDistToPlayer = 0f;
    [SerializeField] private float dmg = 3;
    [SerializeField] private float pwr = 5;

    private iFrameHealth plrHealth;

    private void Awake()
    {
        plrHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<iFrameHealth>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetPlayerLocation();

        CanSeePlayer();

        Debug.Log(GetDistanceToPlayer());

        if (playerInLOS)
        {
            if (GetDistanceToPlayer() < minDistToPlayer && canAttack)
            {
                StartCoroutine(Attack());
            }
        }

        if (canMove)
        {
            Move();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            plrHealth.ChangeHealth(dmg);
        }
    }

    public override IEnumerator Attack()
    {
        canMove = false;
        canAttack = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        Vector2 dir = directionToPlayer;
        dir.Normalize();
        var force = dir * pwr;
        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(fireRate / 2);
        canMove = true;
        yield return new WaitForSeconds(fireRate / 2);
        canAttack = true;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(lastSeenPosition, transform.position);
    }
}
