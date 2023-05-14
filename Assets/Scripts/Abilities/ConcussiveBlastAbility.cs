using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussiveBlastAbility : Ability
{
	[SerializeField]
	private KeyCode button;
	[SerializeField]
	[Min(0)] public float deadzone = 0.1f;
	public Range<float> speed = 5;
	public GameObject projectilePrefab;
	public Range<uint> n = 5;
	[Range(0,360)] public float spreadAngle = 30;
	[Range(0,1)] public float directionRandomness = 1;
	[Min(0)] public float fireRadius;
	public Range<float> randomScale = 1;
	public Range<float> randomRotation = 0;
	public Range<float> randomAngularSpeed = 0;

	public SortingLayer projectileSortingLayer;

	private WeightedArray<GameObject> projectileArray = new();

	public void Awake()
	{
		projectileArray.AddEntry(1, projectilePrefab);
	}

	public override bool Activate()
	{
		if (!OnCooldown && Input.GetKeyDown(button))
		{
			Transform entity = transform; // the transform of the player

			Vector2 direction = Extensions.GetMouseWorldPosition(Camera.main) - (Vector2)entity.position;
			if (direction.magnitude <= deadzone) return false;

			direction.Normalize();

			float directionAngle = direction.Angle();

			Vector2 v = Vector2.zero;
			Rigidbody2D rb;
			if (entity.TryGetComponent(out rb)) v = rb.velocity;

			EntityTeams et;
			Team t = 0;
			if (entity.TryGetComponent(out et)) t = et.Teams;

			var projectiles = ProjectileManager.SpawnProjectileRandomArc(
				entity.position,
				randomRotation,
				v,
				speed,
				randomAngularSpeed,
				t,
				projectileArray,
				n,
				new Range<float>(directionAngle - spreadAngle / 2f, directionAngle + spreadAngle / 2f),
				0,
				directionRandomness,
				fireRadius,
				randomScale
			);

			RestartCooldown();
			return true;
		}

		return false;
	}
}