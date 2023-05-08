using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArcShot : MonoBehaviour
{
	public Vector2 netVelocity;
	public float speed;
	[EnumMask] public Team team;
	public float arcAngle;
	public float directionAngle = 0;
	public float velocityRotation = 0;
	public uint n = 5;
	public float radius = 0;
	public GameObject projectilePrefab;
	public KeyCode button;

	void Update()
	{
		if (Input.GetKeyDown(button))
		{
			ProjectileManager.SpawnProjectileArc(transform.position, netVelocity, speed, team, projectilePrefab, n, arcAngle, directionAngle, velocityRotation, radius);
		}
	}
}
