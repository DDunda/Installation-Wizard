using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
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
		}
		else if(instance != this)
		{
			Destroy(this);
		}
	}

	private static void CheckProjectileCollisions()
	{
		HashSet<Projectile> toDelete = new();
		List<Collider2D> colliders = new();
		HashSet<GameObject> cObjects = new();

		Projectile p2 = null;

		foreach (Projectile p in instance.projectiles.Where(x => x.type.destroyProjectiles))
		{
			if (toDelete.Contains(p)) continue;
			if (p.rigidbody.OverlapCollider(instance.projectileLayerFilter, colliders) == 0) continue;

			cObjects = new(from c in colliders select c.attachedRigidbody.gameObject);
			foreach (var o in cObjects)
			{
				if (p.gameObject == o) continue;
				if (!o.TryGetComponent(out p2)) continue;
				if (toDelete.Contains(p2)) continue;

				p.OnCollide();
				toDelete.Add(p2);

				if(p2.type.destroyProjectiles)
				{
					p2.OnCollide();
					toDelete.Add(p);
					break;
				}
				else if (p.Collisions >= p.type.maxCollisions)
				{
					toDelete.Add(p);
					break;
				}
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
		EntityTeams t;
		Team mask;

		foreach (Projectile p in instance.projectiles)
		{
			if (p.rigidbody.OverlapCollider(instance.entityLayerFilter, colliders) == 0) continue;

			cObjects = new(from c in colliders select c.attachedRigidbody.gameObject);

			foreach (var o in cObjects)
			{
				t = o.GetComponent<EntityTeams>();
				mask = t != null ? t.Teams : 0;

				if (!p.type.entityRelationship.GetRelationship(mask)) continue;
				if (!(o.TryGetComponent(out h) && h.ChangeHealth(-p.type.damage))) continue;

				p.OnCollide();

				if (p.Collisions < p.type.maxCollisions) continue;

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
		Projectile[] toDelete = (from p in instance.projectiles where p.rigidbody.IsTouchingLayers(instance.environmentLayer) select p).ToArray();

		foreach (Projectile p in toDelete)
		{
			Destroy(p.root != null ? p.root : p.gameObject);
		}
	}

	private void FixedUpdate()
	{
		CheckProjectileCollisions();
		CheckEntityCollisions();
		CheckEnvironmentCollisions();
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

	public static bool SpawnProjectile(Vector2 position, Vector2 velocity, ProjectileType type = null, Sprite sprite = null, float scale = 1)
	{
		if (instance == null) return false;
		if (instance.projectilePrefab == null) return false;

		float angle = velocity.magnitude > 0 ? Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg : 0;
		Quaternion rot = instance.projectilePrefab.transform.rotation * Quaternion.Euler(0, 0, angle);

		GameObject projectile = Instantiate(instance.projectilePrefab, position, rot, instance.projectileContainer);

		Projectile p;
		SpriteRenderer sr;

		if (!projectile.TryGetComponent(out p))
		{
			Destroy(projectile);
			return false;
		}

		if (type != null) p.type = type;
		if (sprite != null && projectile.TryGetComponent(out sr)) sr.sprite = sprite;

		projectile.transform.localScale *= scale;
		p.rigidbody.velocity = velocity;

		AddProjectile(p);

		return true;
	}

	public static bool SpawnProjectile(Vector2 position, Vector2 velocity, GameObject projectilePrefab, float scale = 1)
	{
		if (instance == null) return false;

		float angle = velocity.magnitude > 0 ? Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg : 0;
		Quaternion rot = projectilePrefab.transform.rotation * Quaternion.Euler(0, 0, angle);

		GameObject projectile = Instantiate(projectilePrefab, position, rot, instance.projectileContainer);

		Projectile p;

		if (!projectile.TryGetComponent(out p))
		{
			Destroy(projectile);
			return false;
		}

		projectile.transform.localScale *= scale;
		p.rigidbody.velocity = velocity;

		AddProjectile(p);

		return true;
	}

	public static int SpawnProjectileBurst(Vector2 position, Vector2 netVelocity, float speed, GameObject projectilePrefab, uint n, float directionRot = 0, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		if (n == 0) return 0;
		if (instance == null) return 0;

		int c = 0;

		for (int i = 0; i < n; i++)
		{
			float a = (i / (float)n) * 360f;
			Vector2 direction = Extensions.Deg2Vec(a + directionRot, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);

			if (SpawnProjectile(position + direction, netVelocity + velocity, projectilePrefab, scale)) c++;
		}

		return c;
	}
	public static int SpawnProjectileArc(Vector2 position, Vector2 netVelocity, float speed, GameObject projectilePrefab, uint n, Range<float> arcRange, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		if (n == 0) return 0;
		if (instance == null) return 0;

		int c = 0;

		if (n == 1)
		{
			float a = arcRange.Lerp(0.5f);
			Vector2 direction = Extensions.Deg2Vec(a, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);
			return SpawnProjectile(position + direction, netVelocity + velocity, projectilePrefab, scale) ? 1 : 0;
		}

		for (int i = 0; i < n; i++)
		{
			float a = arcRange.Lerp(i / (float)(n - 1));
			Vector2 direction = Extensions.Deg2Vec(a, radius);
			Vector2 velocity = Extensions.Deg2Vec(a + velocityRot, speed);

			if (SpawnProjectile(position + direction, netVelocity + velocity, projectilePrefab, scale)) c++;
		}

		return c;
	}

	public static int SpawnProjectileArc(Vector2 position, Vector2 netVelocity, float speed, GameObject projectilePrefab, uint n, float arcAngle, float directionRot, float velocityRot = 0, float radius = 0, float scale = 1)
	{
		Range<float> arcRange = new(directionRot - arcAngle / 2f, directionRot + arcAngle / 2f);
		return SpawnProjectileArc(position, netVelocity, speed, projectilePrefab, n, arcRange, velocityRot, radius, scale);
	}
}
