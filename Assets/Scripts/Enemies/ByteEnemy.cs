using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ByteEnemy : BaseEnemy
{
    [SerializeField]
    private GameObject projectilePrefab; // the projectile that this enemy fires at the player
    [SerializeField]
    private float projectileSpeed = 10.0f;
    [SerializeField]
    private Transform entity; // the reference to this enemy

    private void Awake()
	{
        canMove = true;
	}

	protected override void Update()
	{
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();
        // if the enemy can see the player, shoot at them
        if (playerInLOS)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        // if the enemy can't see the player, move towards them
        else
        {
            if (canMove)
            {
                Move();
            }
        }
    }

    public override IEnumerator Attack()
	{
        canAttack = false;

        // fire projectile at player
        var angle = directionToPlayer.Angle();

        // define initial velocity (don't factor in parent's movement)
        Vector2 v = Extensions.Deg2Vec(angle, projectileSpeed);

        // find team of parent, if possible
        EntityTeams et;
        Team t = 0;
        if (entity.TryGetComponent(out et)) t = et.teams;

        ProjectileManager.SpawnProjectile(
            entity.position,
            0,
            v,
            0,
            Team.Enemy,
            projectilePrefab);

        // wait amount of seconds before firing again
        yield return new WaitWithPause(fireRate);
        canAttack = true;
    }
}
