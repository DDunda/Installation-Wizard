using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner Projectile", menuName = "Projectile Type/Spawner", order = 1)]
public class SpawnerProjectileType : ProjectileType
{
	[Header("Spawning")]
	public bool inheritVelocity = true;
	public bool inheritTeam = true;

	[Header("Spawned on creation")]
	public List<GameObject> onCreateSpawn = new(); // Always spawned on creation
	public WeightedArray<GameObject[]> onCreateSpawnRandom = new(); // Randomly spawned on creation
	public uint onCreateSpawnN = 0; // How many sets of random objects to spawn
	public bool onCreateSpawnUnique = false; // Do not spawn repeats of random objects

	[Header("Spawned on impact")]
	public List<GameObject> onHitSpawn = new();
	public WeightedArray<GameObject[]> onHitSpawnRandom = new();
	public uint onHitSpawnN = 0;
	public bool onHitSpawnUnique = false;

	[Header("Spawned on destruction")]
	public List<GameObject> onDestroySpawn = new(); // Always spawned on destruction
	public WeightedArray<GameObject[]> onDestroySpawnRandom = new(); // Randomly spawned on destruction
	public uint onDestroySpawnN = 0; // How many sets of random objects to spawn
	public bool onDestroySpawnUnique = false; // Do not spawn repeats of random objects

	private void SpawnObjects(IEnumerable<GameObject> objects, Projectile p)
	{
		Rigidbody2D rb;
		Projectile p2;
		EntityTeams t;
		foreach (var o in objects)
		{
			Instantiate(o, p.transform.position, p.transform.rotation);
			if (inheritVelocity)
			{

				if (!o.TryGetComponent(out rb))
				{
					rb = o.AddComponent<Rigidbody2D>();
					rb.bodyType = RigidbodyType2D.Kinematic;
				}

				rb.velocity = p.rigidbody.velocity;
				rb.angularVelocity = p.rigidbody.angularVelocity;
			}
			if(inheritTeam) {
				if(o.TryGetComponent(out p2)) p2.team = p.team;
				else if(o.layer == 10) {
					if (!o.TryGetComponent(out t)) t = o.AddComponent<EntityTeams>();
					t.teams = p.team;
				}
			}
		}
	}

	private void DoSpawn(Projectile p, IEnumerable<GameObject> toSpawn, WeightedArray<GameObject[]> toSpawnRandom, uint randomN = 0, bool unique = false)
	{

		SpawnObjects(toSpawn, p);
		if (randomN == 0) return;

		GameObject[][] sets = toSpawnRandom.GetRandomN(randomN, unique);

		foreach (GameObject[] set in sets)
		{
			SpawnObjects(set, p);
		}
	}

	public override void OnCreateProjectile(Projectile p)
	{
		base.OnCreateProjectile(p);
		DoSpawn(p, onCreateSpawn, onCreateSpawnRandom, onCreateSpawnN, onCreateSpawnUnique);
	}

	public override void OnCollide(Projectile p, GameObject other)
	{
		base.OnCollide(p, other);
		DoSpawn(p, onHitSpawn, onHitSpawnRandom, onHitSpawnN, onHitSpawnUnique);
	}

	public override void OnDestroyProjectile(Projectile p)
	{
		base.OnDestroyProjectile(p);
		DoSpawn(p, onDestroySpawn, onDestroySpawnRandom, onDestroySpawnN, onDestroySpawnUnique);
	}
}
