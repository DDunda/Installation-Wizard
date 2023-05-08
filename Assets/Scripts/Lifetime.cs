using UnityEngine;

public class Lifetime : MonoBehaviour, IPausable
{
	[SerializeField, Min(0)] private float lifetime = 1;

	private void Awake()
	{
		this.RegisterPause();
	}

	private void OnDestroy()
	{
		this.UnregisterPause();
	}
	
	private void FixedUpdate()
	{
		lifetime -= Time.fixedDeltaTime;

		if (lifetime > 0) return;

		Destroy(gameObject);
	}

	public void OnPause()
	{
		enabled = false;
	}

	public void OnResume()
	{
		enabled = true;
	}
}