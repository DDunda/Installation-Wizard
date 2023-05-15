using UnityEngine;

public class iFrameHealth : EntityHealth, IPausable
{
	private float iTime = 0;

	[Header("Invincibility")]
	[Min(0),SerializeField,Tooltip("i-frames applied when damaged")] private float iDamageTime;
	[SerializeField,Tooltip("Overrides iframes")] public bool invincible = false;
	[SerializeField, Tooltip("Whether this entity absorbs projectiles while invincible")] public bool absorbWhenInvincible = true;

	public override bool canAbsorbProjectiles { get => canTakeDamage || absorbWhenInvincible; }
	public override bool canTakeDamage { get => !invincible && iTime == 0; }
	public override bool canTakeDamageContinuous { get => !invincible; }

	private void Awake()
	{
		iTime = 0;
		this.RegisterPause();
	}

	private void OnDestroy()
	{
		this.UnregisterPause();
	}

	public void OnPause() {
		enabled = false;
	}

	public void OnResume() {
		enabled = true;
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
