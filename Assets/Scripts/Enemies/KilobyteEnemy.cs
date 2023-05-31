using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilobyteEnemy : BaseEnemy
{
    [SerializeField] private GameObject projectilePrefab; // the projectile that this enemy fires at the player
    [SerializeField] private float projectileSpeed = 10.0f;
    [SerializeField] private Transform entity; // the reference to this enemy
	[SerializeField] protected float maxAttackRange = 1f;



	public Range<float> speed = 5;
	//public GameObject projectilePrefab;
	public Range<uint> n = 5;
	[Range(0,360)] public float spreadAngle = 30;
	[Range(0,1)] public float directionRandomness = 1;
	[Min(0)] public float fireRadius;
	public Range<float> randomScale = 1;
	public Range<float> randomRotation = 0;
	public Range<float> randomAngularSpeed = 0;

    private WeightedArray<GameObject> projectileArray;

    private void Awake()
	{
        canMove = true;
        projectileArray = new(projectilePrefab);
	}

	protected override void Update()
	{
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();
        // if the enemy can see the player, shoot at them
        if (playerInLOS && maxAttackRange == -1 || directionToPlayer.magnitude <= maxAttackRange)
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

        Vector2 direction = playerPosition2D - (Vector2)entity.position;
        Vector3 offset = gameObject.GetComponent<BoxCollider2D>().offset; // get offset of casting object's box collider 2d

		direction.Normalize();

		float directionAngle = direction.Angle();

        ProjectileManager.SpawnProjectileRandomArc(
			entity.position + offset,
			randomRotation,
			v,
			speed,
			randomAngularSpeed,
			_team,
			projectileArray,
			n,
			new Range<float>(directionAngle - spreadAngle / 2f, directionAngle + spreadAngle / 2f),
			0,
			directionRandomness,
			fireRadius,
			randomScale);

        // wait amount of seconds before firing again
        yield return new WaitWithPause(fireRate);
        canAttack = true;
    }
}
