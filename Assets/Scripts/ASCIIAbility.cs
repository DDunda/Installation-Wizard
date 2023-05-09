using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASCIIAbility : Ability
{
    [SerializeField]
    private KeyCode button;
	[SerializeField]
	private GameObject projectilePrefab;
	[SerializeField]
	private float projectileSpeed = 5.0f;
	private Transform entity;

	// Called at the start of the scene
	private void Awake()
	{
		entity = transform;
	}

	public override bool Activate()
	{
		if (!OnCooldown && Input.GetKeyDown(button))
		{
			Vector2 direction = Extensions.GetMouseWorldPosition(Camera.main) - (Vector2)entity.position;
			//if (direction.magnitude <= deadzone) return false;

			direction.Normalize();
			var angle = direction.Angle();

			// find velocity of parent, if possible
			Vector2 v = Vector2.zero;
			Rigidbody2D rb;
			if (entity.TryGetComponent(out rb)) v = rb.velocity;

			// add initial velocity of projectile
			v += Extensions.Deg2Vec(angle, projectileSpeed);

			// find team of parent, if possible
			EntityTeams et;
			Team t = 0;
			if (entity.TryGetComponent(out et)) t = et.Teams;

			ProjectileManager.SpawnProjectile(
				entity.position, 
				angle,
				v,
				0,
				t,
				projectilePrefab);

			RestartCooldown();
			return true;
		}

		return false;
	}
}
