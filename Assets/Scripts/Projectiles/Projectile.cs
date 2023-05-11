using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
	public GameObject root = null;
	public ProjectileType type;
	[HideInInspector]
	public new Rigidbody2D rigidbody;
	[EnumMask]
	public Team team;

	public float? lifetime = null;

	public uint Collisions { get; private set; } = 0;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		if (type.lifetime.min > 0 || type.lifetime.max > 0)
		{
			lifetime = type.lifetime.Random();
		}
		ProjectileManager.AddProjectile(this);
	}
	
	private void OnDestroy()
	{
		ProjectileManager.RemoveProjectile(this);
		type.OnDestroyProjectile(this);
	}

	public void OnCollide(GameObject other)
	{
		Collisions++;
		type.OnCollide(this, other);
	}
}