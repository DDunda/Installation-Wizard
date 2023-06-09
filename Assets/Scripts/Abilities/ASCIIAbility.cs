using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ASCIIAbility : Ability
{
    [SerializeField]
    private KeyCode button;
	[SerializeField]
	private GameObject projectilePrefab; // the projectile to fire
	[SerializeField]
	private GameObject trailText; // the text field to modify
	[SerializeField]
	private float textTimerMax = 0.2f; // every n seconds, change the text in projectile trails
	private float textTimer;
	[SerializeField]
	private int trailCharNum = 10; // amount of characters to add into trail texture when randomising
	[SerializeField]
	private float projectileSpeed = 5.0f;
	private Transform entity;
	private AudioSource abilitySound;
	[SerializeField] private string soundName;

	// Called at the start of the scene
	private void Awake()
	{
		abilitySound = GameObject.Find(soundName).GetComponent<AudioSource>();
		entity = transform;
		textTimer = textTimerMax;
	}

	public override bool Activate()
	{
		if (!OnCooldown && Input.GetKeyDown(button))
		{
			Vector2 direction = Extensions.GetMouseWorldPosition(Camera.main) - (Vector2)entity.position;
			Vector3 offset = gameObject.GetComponent<BoxCollider2D>().offset; // get offset of casting object's box collider 2d

			direction.Normalize();
			var angle = direction.Angle();

			// define initial velocity (don't factor in parent's movement)
			Vector2 v = Extensions.Deg2Vec(angle, projectileSpeed);

			// find team of parent, if possible
			ITeams tt;
			Team t = 0;
			if (entity.TryGetComponent(out tt)) t = tt.team;

			abilitySound.Play();

			ProjectileManager.SpawnProjectile(
				entity.position + offset, 
				0,
				v,
				0,
				t,
				projectilePrefab);

			RestartCooldown();
			return true;
		}

		// randomise the text inside the trail
		textTimer -= Time.deltaTime;
		if (textTimer < 0)
		{
			string newText = "";

			// pick ten random characters from a set
			for (var i = 0; i < trailCharNum; i++)
			{
				var code = Random.Range(33, 125);
				newText += (char)code;
			}

			// set new trail text
			trailText.GetComponent<TMP_Text>().text = newText;
			// reset timer
			textTimer += textTimerMax; 
		}

		return false;
	}
}
