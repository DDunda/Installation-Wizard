using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestExplosion : MonoBehaviour
{
	public Range<float> speed;
	public EntityTeams team;
	public Range<float> directionRotation = 0;
	public Range<float> velocityRotation = 0;
	public Range<float> directionRandomness = 0;
	public Range<uint> n = 5;
	public Range<float> radius = 0;
	public WeightedArray<GameObject> projectilePrefab;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		ProjectileManager.SpawnProjectileRandomBurst(transform.position, 0, Vector2.zero, speed, rb.angularVelocity, team.teams, projectilePrefab, n, directionRotation, velocityRotation, directionRandomness, radius, 1);
		Destroy(gameObject);
	}
}
