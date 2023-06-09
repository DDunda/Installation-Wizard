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
    [SerializeField] private string soundName;
	private AudioSource abilitySound;
    [SerializeField] private string deathName;
	private AudioSource deathSound;

    private float oldMoveSpeed;

    private void Awake()
	{
        oldMoveSpeed = moveSpeed;
        canMove = true;
        abilitySound = GameObject.Find(soundName).GetComponent<AudioSource>();
        deathSound = GameObject.Find(deathName).GetComponent<AudioSource>();
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
            moveSpeed = oldMoveSpeed/10;
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        // if the enemy can't see the player, move towards them
        else
        {
            moveSpeed = oldMoveSpeed;
        }
        if (canMove)
        {
            Move();
        }
    }

    public override IEnumerator Attack()
	{
        abilitySound.Play();
        canAttack = false;

        // fire projectile at player
        var angle = directionToPlayer.Angle();

        // define initial velocity (don't factor in parent's movement)
        Vector2 v = Extensions.Deg2Vec(angle, projectileSpeed);
        Vector3 offset = gameObject.GetComponent<BoxCollider2D>().offset; // get offset of casting object's box collider 2d

        ProjectileManager.SpawnProjectile(
            entity.position + offset,
            0,
            v,
            0,
            _team,
            projectilePrefab);

        // wait amount of seconds before firing again
        yield return new WaitWithPause(fireRate);
        canAttack = true;
    }

    public void DeathSound()
    {
        deathSound.Play();
    }
}
