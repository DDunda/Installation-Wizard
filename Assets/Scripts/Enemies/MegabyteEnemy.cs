using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MegabyteEnemy : BaseEnemy
{
	[Header("Projectile ring")]
	[SerializeField] protected float stopDistance = 5f;
	[SerializeField] protected ProjectileBurst burstSettings = new();

	[Header("Bit spawning")]
	[SerializeField] protected bool canSpawn = true;
	[SerializeField] protected GameObject bitPrefab;
	[SerializeField, Min(0)] protected float spawnClearRadius = 1f; // The radius of the circle used to check for a clear area
	[SerializeField] protected LayerMask spawnBlockingLayers = 0;
	[SerializeField] protected uint bitsToSpawn = 2;
	[SerializeField] protected uint maximumBits = 6; // Set to 0 for unlimited bits
	[SerializeField, Min(0)] protected float bitSpawnDelay = 5;
	[SerializeField] protected WeightedArray<Vector2> spawnLocations = new();

	protected HashSet<GameObject> _spawnedBits = new();
	protected int _nBits = 0;

	protected override void Update()
	{
		GetPlayerLocation();
		CanSeePlayer();

		if (!seenPlayer) return;

		if (_nBits > 0) _nBits -= _spawnedBits.RemoveWhere(x => x == null);

		if (canSpawn && bitsToSpawn > 0 && (maximumBits == 0 || _nBits < maximumBits)) StartCoroutine(SpawnBits());

		if (playerInLOS && directionToPlayer.magnitude <= stopDistance)
		{
			if (canAttack) StartCoroutine(Attack());
		}
		else
		{
			if (canMove) Move();
		}
	}

	public override IEnumerator Attack()
	{
		canAttack = false;
		// Assuming fireRate is actually a rate, the seconds between shots is its inverse
		burstSettings.Spawn(transform.position, rb.velocity, team.teams);
		yield return new WaitWithPause(1f / fireRate);
		canAttack = true;
	}

	private Vector2[] GetSpawnLocations(uint n)
	{
		List<Vector2> loc = new((int)n);
		Vector2 entityPosition = transform.position;
		WeightedArray<Vector2> _spawnLocations = new(spawnLocations.Entries);

		while(n > 0)
		{
			int i = _spawnLocations.GetRandomIndex();
			if (i == -1) return loc.ToArray();

			Vector2 pos = entityPosition + _spawnLocations[i].item;
			_spawnLocations.RemoveAt(i);

			if (Physics2D.OverlapCircle(pos, spawnClearRadius, spawnBlockingLayers) == null)
			{
				loc.Add(pos);
				n--;
			}
		}

		return loc.ToArray();
	}

	public IEnumerator SpawnBits()
	{
		Vector2[] locations = GetSpawnLocations((uint)Math.Min(maximumBits - _nBits, bitsToSpawn));

		if (locations.Length > 0)
		{
			canSpawn = false;

			foreach (Vector2 pos in locations)
			{
				_spawnedBits.Add(Instantiate(bitPrefab, pos, Quaternion.identity));
				_nBits++;
			}

			yield return new WaitWithPause(bitSpawnDelay);

			canSpawn = true;
		}
	}
}