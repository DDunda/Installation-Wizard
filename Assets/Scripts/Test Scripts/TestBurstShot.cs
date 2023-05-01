using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBurstShot : MonoBehaviour
{
	public Vector2 netVelocity;
	public float speed;
	public float directionRotation = 0;
	public float velocityRotation = 0;
	public uint n = 5;
	public float radius = 0;
	public GameObject projectilePrefab;
	public KeyCode button;

	void Update()
	{
		if (Input.GetKeyDown(button))
		{
			ProjectileManager.SpawnProjectileBurst(transform.position, netVelocity, speed, projectilePrefab, n, directionRotation, velocityRotation, radius);
		}
	}
}
