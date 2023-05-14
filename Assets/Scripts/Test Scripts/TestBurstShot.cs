using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBurstShot : MonoBehaviour
{
	public Vector2 netVelocity;
	[EnumMask] public Team team;
	public KeyCode button;

	public ProjectileBurst burstSettings;

	void Update()
	{
		if (Input.GetKeyDown(button))
		{
			burstSettings.Spawn(transform.position, netVelocity, team);
		}
	}
}
