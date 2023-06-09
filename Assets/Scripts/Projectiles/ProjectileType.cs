using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectile Type/Standard", order = 1)]
public class ProjectileType : ScriptableObject
{
	public Range<float> damage = 10;
	[Tooltip("Set to 0 for infinite lifetime")]
	public Range<float> lifetime = 1;
	[Min(0), Tooltip("Set to 0 for infinite collisions")]
	public uint maxCollisions = 0;
	public bool destroyProjectiles = false;
	[Space]
	public TeamRelationship entityRelationship;
	[Space]
	public WeightedArray<AudioClip[]> onCreateSounds = new();
	public WeightedArray<AudioClip[]> onHitSounds = new();
	public WeightedArray<AudioClip[]> onDestroySounds = new();

	public virtual void OnCreateProjectile(Projectile p) { }
	public virtual void OnCollide(Projectile p, GameObject other) { }
	public virtual void OnDestroyProjectile(Projectile p) { }
}
