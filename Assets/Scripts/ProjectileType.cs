using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectile Type", order = 1)]
public class ProjectileType : ScriptableObject
{
	[Min(0)]
	public float damage;
	[Min(0),Tooltip("Set to 0 for infinite lifetime")]
	public float lifetime = 0;
	[Min(1)]
	public uint maxCollisions = 1;
	public bool destroyProjectiles = false;
	[Space]
	public TeamRelationship entityRelationship;

	[Space]
	[SerializeField] private UnityEvent onCollide;
	[SerializeField] private UnityEvent onDestroy;

	public virtual void OnCollide(Projectile p) {
		onCollide.Invoke();
	}
	public virtual void OnDestroyProjectile(Projectile p)
	{
		onDestroy.Invoke();
	}
}
