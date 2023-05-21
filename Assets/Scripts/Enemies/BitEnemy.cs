using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitEnemy : BaseEnemy
{
    [SerializeField] private float minDistToPlayer = 0f;
    [SerializeField] protected float dmg = 3;
    [SerializeField] private float pwr = 5;

    protected iFrameHealth plrHealth;
    public GameObject hurtBox;

    private void Awake()
    {
        plrHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<iFrameHealth>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetPlayerLocation();

        CanSeePlayer();

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

    public override IEnumerator Attack()
    {
        canMove = false;
        canAttack = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(1f);
        Vector2 dir = directionToPlayer;
        hurtBox.GetComponent<BoxCollider2D>().enabled = true;
        dir.Normalize();
        var force = dir * pwr;
        rb.AddForce(force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(fireRate / 2);
        canMove = true;
        yield return new WaitForSeconds(fireRate / 2);
        hurtBox.GetComponent<BoxCollider2D>().enabled = false;
        canAttack = true;
    }

    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(lastSeenPosition, transform.position);
    }
}
