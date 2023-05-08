using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleShot : MonoBehaviour
{
	public Vector2 velocity;
	[EnumMask] public Team team;
	public GameObject projectilePrefab;
	public KeyCode button;

	void Update()
	{
		if(Input.GetKeyDown(button))
		{
			ProjectileManager.SpawnProjectile(transform.position, 0, velocity, 0, team, projectilePrefab);
		}
	}
}
