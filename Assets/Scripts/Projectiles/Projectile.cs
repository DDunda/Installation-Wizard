using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, ITeams
{
	public GameObject root = null;
	public ProjectileType type;
	[HideInInspector]
	public new Rigidbody2D rigidbody;
	[SerializeField,EnumMask]
	protected Team _team;

	public float? lifetime = null;

	public uint Collisions { get; private set; } = 0;

	public Team team { get => _team; set => _team = value; }

	protected bool TryPlaySounds(WeightedArray<AudioClip[]> clips)
	{
		if (clips.Entries.Length == 0) return false;
		AudioManager.instance.PlayClips(clips.GetRandom(), transform.position);
		return true;
	}

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		ProjectileManager.AddProjectile(this);
		if (type == null) return;

		if (type.lifetime.min > 0 || type.lifetime.max > 0)
		{
			lifetime = type.lifetime.Random();
		}

		TryPlaySounds(type.onCreateSounds);
		type.OnCreateProjectile(this);
	}
	
	private void OnDestroy()
	{
		ProjectileManager.RemoveProjectile(this);
		if (type == null) return;

		TryPlaySounds(type.onDestroySounds);
		type.OnDestroyProjectile(this);
	}

	public void OnCollide(GameObject other)
	{
		Collisions++;
		if (type == null) return;

		TryPlaySounds(type.onHitSounds);
		type.OnCollide(this, other);
	}

	public void OnCollisionEnter(Collision collision)
	{
		OnCollide(collision.gameObject);

		if(Collisions >= type.maxCollisions)
		{
			Destroy(gameObject);
			return;
		}
	}
}