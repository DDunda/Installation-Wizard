using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectile Type", order = 1)]
public class ProjectileType : ScriptableObject
{
	[Min(0)]
	public float damage;
	[Min(0),Tooltip("Set to 0 for infinite lifetime")]
	public float lifetime = 0;
	[Min(0), Tooltip("Set to 0 for infinite collisions")]
	public uint maxCollisions = 0;
	public bool destroyProjectiles = false;
	[Space]
	public TeamRelationship entityRelationship;

	public virtual void OnCollide(Projectile p) { }
	public virtual void OnDestroyProjectile(Projectile p) { }
}
