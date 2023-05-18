using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestSpiralShot : MonoBehaviour
{
	public KeyCode button;

	public Vector2 offset;
	public Range<float> preDelay = 0f;
	public Range<float> iterDelay = 0.1f;

	public ProjectileSpiral spiral;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		spiral.team = GetComponent<ITeams>();
	}

	void Update()
	{
		spiral.position = (Vector2)transform.position + offset;
		spiral.netVelocity = rb.velocity;

		if (Input.GetKeyDown(button))
		{
			StartCoroutine(spiral.Spawn(gameObject, preDelay, iterDelay));
		}
	}
}
