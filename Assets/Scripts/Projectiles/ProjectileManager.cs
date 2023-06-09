using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ProjectileBurst
{
	public Range<float> projectileRotation; // Degrees
	public Range<float> projectileScale;
	[Space]
	public Range<float> speed; // The speed by which projectiles fire away from the center (positive: away from center, negative: toward center)
	public Range<float> velocityRotation; // By default projectiles fire away from the center, this rotates away from that direction (degrees)
	public Range<float> angularSpeed;
	[Space]
	public Range<float> radius; // The size of the spawning circle (may be negative)
	public Range<float> directionRotation; // Rotates the whole circle of projectiles (degrees)
	public Range<float> directionRandomness; // Determines how the projectiles are spawned along the circle (0: Evenly, 1: Randomly)
	[Space]
	public WeightedArray<GameObject> projectilePrefabs; // Projectile(s) that may be spawned
	public Range<uint> projectileCount;

	public ProjectileBurst()
	{
		projectileRotation = 0;
		projectileScale = 1;

		speed = 1;
		velocityRotation = 0;
		angularSpeed = 0;

		radius = 1;
		directionRotation = 0;
		directionRandomness = 0;

		projectilePrefabs = new();
		projectileCount = 16;
	}

	public void Spawn(Vector2 position, Vector2 netVelocity, Team team = 0)
	{
		ProjectileManager.SpawnProjectileRandomBurst(
			position,
			projectileRotation,
			netVelocity,
			speed,
			angularSpeed,
			team,
			projectilePrefabs,
			projectileCount,
			directionRotation,
			velocityRotation,
			directionRandomness,
			radius,
			projectileScale
		);
	}
}


[System.Serializable]
public class ProjectileSpiral
{
	public Vector2 position;
	public Vector2 netVelocity;
	public Range<float> rotation;
	public ITeams team;
	[Space]
	public Range<float> projectileRotation; // Degrees
	public Range<float> projectileScale;
	[Space]
	public Range<float> speed; // The speed by which projectiles fire away from the center (positive: away from center, negative: toward center)
	public Range<float> velocityRotation; // By default projectiles fire away from the center, this rotates away from that direction (degrees)
	public Range<float> angularSpeed;
	[Space]
	public Range<float> radius; // The size of the spawning circle (may be negative)
	public Range<float> directionRotation; // Rotates the whole circle of projectiles (degrees)
	public Range<float> directionRandomness; // Determines how the projectiles are spawned along the circle (0: Evenly, 1: Randomly)
	[Space]
	public WeightedArray<GameObject> projectilePrefabs; // Projectile(s) that may be spawned
	[Min(1)] public int arms = 3;
	public int iterations = 1;
	[Range(-180f,180f)] public float iterationRotation = 360f;

	public List<Projectile> projectiles { get; private set; } = new();

	public ProjectileSpiral()
	{
		position = Vector2.zero;
		netVelocity = Vector2.zero;
		team = null;

		projectileRotation = 0;
		projectileScale = 1;

		speed = 1;
		velocityRotation = 0;
		angularSpeed = 0;

		radius = 1;
		directionRotation = 0;
		directionRandomness = 0;

		projectilePrefabs = new();
		arms = 3;
		iterations = 1;
		iterationRotation = 1f;
	}

	private void DoIteration(float r)
	{
		for(int i = 0; i < arms; i++)
		{
			float dr = directionRandomness.Random();
			float a = i * (360f / arms) + r + Random.Range(-dr, dr) * 180f + directionRotation.Random();
			
			Vector2 direction = Extensions.Deg2Vec(a, radius.Random());
			Vector2 velocity = Extensions.Deg2Vec(a, speed.Random());

			projectiles.Add(ProjectileManager.SpawnProjectileRandom(position + direction, rotation, netVelocity + velocity, angularSpeed.Random(), team.team, projectilePrefabs, projectileScale));
		}
	}

	public IEnumerator Spawn(GameObject parent, Range<float> preDelay, Range<float> iterationDelay)
	{
		if (iterations != 0)
		{
			float d = preDelay.Random();
			if (d > 0) yield return new WaitWithPause(d);

			uint i = 0;
			for (float r = 0; parent != null; r = Mathf.Repeat(r + iterationRotation, 360f))
			{
				DoIteration(r);
				if (iterations > 0 && ++i == iterations) break;

				d = iterationDelay.Random();
				if (d > 0) yield return new WaitWithPause(d);
			}
		}
	}
}

[RequireComponent(typeof(PhysicsPauser))]
public class ProjectileManager : MonoBehaviour, IPausable
{
	public static ProjectileManager instance { get; private set; } = null;

	[SerializeField] private HashSet<Projectile> projectiles;
	[SerializeField] private LayerMask projectileLayer;
	[SerializeField] private LayerMask entityLayer;
	[SerializeField] private LayerMask environmentLayer;

	[SerializeField] private Transform projectileContainer;
	[SerializeField] private GameObject projectilePrefab;

	private ContactFilter2D projectileLayerFilter;
	private ContactFilter2D entityLayerFilter;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			projectileLayerFilter.SetLayerMask(projectileLayer);
			entityLayerFilter.SetLayerMask(entityLayer);
			projectiles = new();
			this.RegisterPause();
		}
		else if(instance != this)
		{
			Destroy(this);
		}
	}

	private void OnDestroy()
	{
		this.UnregisterPause();
	}

	public void OnPause()
	{
		enabled = false;
	}

	public void OnResume()
	{
		enabled = true;
	}

	private static void CheckProjectileCollisions()
	{
		HashSet<Projectile> toDelete = new();
		List<Collider2D> colliders = new();
		HashSet<Projectile> cProjectiles = new();

		foreach (Projectile p in instance.projectiles.Where(x => x.type.destroyProjectiles))
		{
			if (toDelete.Contains(p)) continue;
			if (p.rigidbody.OverlapCollider(instance.projectileLayerFilter, colliders) == 0) continue;

			cProjectiles = new(from c in colliders select c.GetComponent<Projectile>());
			cProjectiles.Remove(p);
			cProjectiles.Remove(null);

			foreach (Projectile p2 in cProjectiles.Where(x => p.type.entityRelationship.GetRelationship(x.team)))
			{
				if (toDelete.Contains(p2)) continue;

				p.OnCollide(p2.gameObject);
				toDelete.Add(p2);

				if (p2.type.destroyProjectiles && p2.type.entityRelationship.GetRelationship(p.team))
				{
					p2.OnCollide(p.gameObject);
				}
				else if (p.type.maxCollisions == 0 || p.Collisions < p.type.maxCollisions) continue;

				toDelete.Add(p);
				break;
			}
		}

		foreach (Projectile p in toDelete)
		{
			Destroy(p.root != null ? p.root : p.gameObject);
		}
	}

	private static void CheckEntityCollisions()
	{
		List<Projectile> toDelete = new();
		List<Collider2D> colliders = new();
		HashSet<GameObject> cObjects = new();

		EntityHealth h;
		ITeams t;
		Team mask;

		foreach (Projectile p in instance.projectiles)
		{
			if (p.rigidbody.OverlapCollider(instance.entityLayerFilter, colliders) == 0) continue;

			cObjects = new(from c in colliders select c.attachedRigidbody.gameObject);

			foreach (var o in cObjects)
			{
				t = o.GetComponent<ITeams>();
				mask = t != null ? t.team : 0;

				if (!p.type.entityRelationship.GetRelationship(mask)) continue;
				if (!(o.TryGetComponent(out h) && h.ChangeHealth(-p.type.damage.Random()))) continue;

				p.OnCollide(o.gameObject);

				if (p.type.maxCollisions == 0 || p.Collisions < p.type.maxCollisions) continue;

				toDelete.Add(p);
				break;
			}

			colliders.Clear();
		}

		foreach (Projectile p in toDelete)
		{
			Destroy(p.root != null ? p.root : p.gameObject);
		}
	}

	private static void CheckEnvironmentCollisions()
	{
		var toDelete = (from p in instance.projectiles where p.rigidbody.IsTouchingLayers(instance.environmentLayer) select p).ToArray();
		foreach (Projectile p in toDelete)
		{
			Destroy(p.root != null ? p.root : p.gameObject);
		}
	}

	private static void CheckLifetime()
	{
		List<Projectile> toDelete = new();
		foreach (Projectile p in instance.projectiles.Where(x => x.lifetime != null))
		{
			p.lifetime -= Time.fixedDeltaTime;
			if (p.lifetime <= 0) toDelete.Add(p);
		}

		foreach(Projectile p in toDelete)
		{
			Destroy(p.root != null ? p.root : p.gameObject);
		}
	}

	private void FixedUpdate()
	{
		CheckProjectileCollisions();
		CheckEntityCollisions();
		CheckEnvironmentCollisions();
		CheckLifetime();
	}

	public static void AddProjectile(Projectile p)
	{
		instance.projectiles.Add(p);
		if(instance.projectileContainer != null)
		{
			p.root.transform.SetParent(instance.projectileContainer);
		}
	}

	public static void RemoveProjectile(Projectile p)
	{
		instance.projectiles.Remove(p);
	}

	public static Projectile SpawnProjectile(Vector2 position, float rotation, Vector2 velocity, float angularSpeed, Team team, ProjectileType type = null, Sprite sprite = null, float scale = 1)
	{
		Projectile p = SpawnProjectile(position, rotation, velocity, angularSpeed, team, instance.projectilePrefab, scale);
		SpriteRenderer sr;

		if (p == null) return null;

		if (type != null) p.type = type;
		if (sprite != null && p.TryGetComponent(out sr)) sr.sprite = sprite;

		return p;
	}

	public static Projectile SpawnProjectile(Vector2 position, float rotation, Vector2 velocity, float angularSpeed, Team team, GameObject projectilePrefab, float scale = 1)
	{
		if (projectilePrefab == null) return null;

		float angle = velocity.magnitude > 0 ? Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg : 0;
		Quaternion rot = projectilePrefab.transform.rotation * Quaternion.Euler(0, 0, angle + rotation);

		GameObject projectile = Instantiate(projectilePrefab, position, rot, instance != null ? instance.projectileContainer : null);

		Projectile p;

		if (!projectile.TryGetComponent(out p))
		{
			Destroy(projectile);
			return null;
		}

		projectile.transform.localScale *= scale;
		p.rigidbody.velocity = velocity;
		p.rigidbody.angularVelocity = angularSpeed;
		p.team = team;

		AddProjectile(p);

		return p;
	}

	public static Projectile SpawnProjectileRandom(Vector2 position, Range<float> rotation, Vector2 velocity, Range<float> angularSpeed, Team team, WeightedArray<GameObject> projectilePrefabs, Range<float> scale)
		=> SpawnProjectile(position, rotation.Random(), velocity, angularSpeed.Random(), team, projectilePrefabs.GetRandom(), scale.Random());

	public static Projectile[] SpawnProjectileBurst(Vector2 position, float rotation, Vector2 netVelocity, float speed, float angularSpeed, Team team, GameObject projectilePrefab, uint n, float directionRot = 0, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		if (n == 0) return new Projectile[0];
		if (instance == null) return new Projectile[0];

		List<Projectile> projectiles = new();

		for (int i = 0; i < n; i++)
		{
			float a = (i / (float)n) * 360f;
			Vector2 direction = Extensions.Deg2Vec(a + directionRot, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);

			projectiles.Add(SpawnProjectile(position + direction, rotation, netVelocity + velocity, angularSpeed, team, projectilePrefab, scale));
		}

		return projectiles.ToArray();
	}
	public static Projectile[] SpawnProjectileRandomBurst(Vector2 position, Range<float> rotation, Vector2 netVelocity, Range<float> speed, Range<float> angularSpeed, Team team, WeightedArray<GameObject> projectilePrefabs, Range<uint> nr, Range<float> directionRot, Range<float> velocityRot, Range<float> directionRandomness, Range<float> radius, Range<float> scale)
	{
		uint n = nr.Random();
		if (n == 0) return new Projectile[0];
		if (instance == null) return new Projectile[0];

		List<Projectile> projectiles = new();

		for (int i = 0; i < n; i++)
		{
			float dr = directionRandomness.Random();
			float a = (i / (float)n) * 360f + Random.Range(-dr, dr) * 180f;
			Vector2 direction = Extensions.Deg2Vec(a + directionRot.Random(), radius.Random());
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot.Random(), speed.Random());

			projectiles.Add(SpawnProjectileRandom(position + direction, rotation, netVelocity + velocity, angularSpeed.Random(), team, projectilePrefabs, scale));
		}

		return projectiles.ToArray();
	}

	public static Projectile[] SpawnProjectileArc(Vector2 position, float rotation, Vector2 netVelocity, float speed, float angularSpeed, Team team, GameObject projectilePrefab, uint n, Range<float> arcRange, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		if (n == 0) return new Projectile[0];
		if (instance == null) return new Projectile[0];

		if (n == 1)
		{
			float a = arcRange.Lerp(0.5f);
			Vector2 direction = Extensions.Deg2Vec(a, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);
			return new Projectile[1] { SpawnProjectile(position + direction, rotation, netVelocity + velocity, angularSpeed, team, projectilePrefab, scale) };
		}

		List<Projectile> projectiles = new();

		for (int i = 0; i < n; i++)
		{
			float a = arcRange.Lerp(i / (float)(n - 1));
			Vector2 direction = Extensions.Deg2Vec(a, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);

			projectiles.Add(SpawnProjectile(position + direction, rotation, netVelocity + velocity, angularSpeed, team, projectilePrefab, scale));
		}

		return projectiles.ToArray();
	}
	public static Projectile[] SpawnProjectileRandomArc(Vector2 position, Range<float> rotation, Vector2 netVelocity, Range<float> speed, Range<float> angularSpeed, Team team, WeightedArray<GameObject> projectilePrefab, Range<uint> nr, Range<float> arcRange, Range<float> velocityRot, Range<float> directionRandomness, Range<float> radius, Range<float> scale)
	{
		uint n = nr.Random();
		if (n == 0) return new Projectile[0];
		if (instance == null) return new Projectile[0];

		if (n == 1)
		{

			float a = Mathf.Lerp(arcRange.Lerp(0.5f), arcRange.Random(), directionRandomness.Random());
			Vector2 direction = Extensions.Deg2Vec(a, radius.Random());
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot.Random(), speed.Random());
			return new Projectile[1] { SpawnProjectileRandom(position + direction, rotation, netVelocity + velocity, angularSpeed, team, projectilePrefab, scale) };
		}

		List<Projectile> projectiles = new();

		for (int i = 0; i < n; i++)
		{
			float a = Mathf.Lerp(arcRange.Lerp(i / (float)(n - 1)), arcRange.Random(), directionRandomness.Random());
			Vector2 direction = Extensions.Deg2Vec(a, radius.Random());
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot.Random(), speed.Random());

			projectiles.Add(SpawnProjectileRandom(position + direction, rotation, netVelocity + velocity, angularSpeed, team, projectilePrefab, scale));
		}

		return projectiles.ToArray();
	}


	public static Projectile[] SpawnProjectileArc(Vector2 position, float rotation, Vector2 netVelocity, float speed, float angularSpeed, Team team, GameObject projectilePrefab, uint n, float arcAngle, float directionRot, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		Range<float> arcRange = new(directionRot - arcAngle / 2f, directionRot + arcAngle / 2f);
		return SpawnProjectileArc(position, rotation, netVelocity, speed, angularSpeed, team, projectilePrefab, n, arcRange, velocityRot, radius, scale);
	}
}
