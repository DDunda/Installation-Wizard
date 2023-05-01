using UnityEngine;

public class iFrameHealth : EntityHealth
{
	[Min(0),SerializeField] private float iTime = 0;
	[Min(0),SerializeField,Tooltip("i-frames applied when damaged")] private float iDamageTime;

	[SerializeField,Tooltip("Overrides iframes")] private bool invincible = false;

	protected override bool canTakeDamage { get => !invincible && iTime == 0; }

	private void Awake()
	{
		iTime = 0;
	}

	private void FixedUpdate()
	{
		if (iTime > 0)
		{
			iTime = Mathf.Max(iTime - Time.fixedDeltaTime, 0);
		}
	}

	protected override void OnDamage(float amt)
	{
		base.OnDamage(amt);
		iTime += iDamageTime;
	}
}
