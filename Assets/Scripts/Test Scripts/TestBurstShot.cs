using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBurstShot : MonoBehaviour
{
	public Vector2 netVelocity;
	public Range<float> speed;
	[EnumMask] public Team team;
	public Range<float> directionRotation = 0;
	public Range<float> velocityRotation = 0;
	public Range<float> directionRandomness = 0;
	public Range<uint> n = 5;
	public Range<float> radius = 0;
	public WeightedArray<GameObject> projectilePrefab;
	public KeyCode button;

	void Update()
	{
		if (Input.GetKeyDown(button))
		{
			ProjectileManager.SpawnProjectileRandomBurst(transform.position, 0, netVelocity, speed, 0, team, projectilePrefab, n, directionRotation, velocityRotation, directionRandomness, radius, 1);
		}
	}
}
