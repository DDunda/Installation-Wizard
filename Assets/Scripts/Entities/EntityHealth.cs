using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
	[Min(0),SerializeField] private float health;
	[Min(0),SerializeField] private float healthMax;

	[SerializeField] private bool destroyOnDeath = true;
	[Min(0), SerializeField] private float destroyDelay;

	[Space]

	[SerializeField] private UnityEvent onDamage;
	[SerializeField] private UnityEvent onDeath;

	protected virtual bool canTakeDamage { get => true; }

	protected virtual void OnDamage(float amt) {
		onDamage.Invoke();
	}
	protected virtual void OnDeath()
	{
		onDeath.Invoke();
		if (destroyOnDeath)
		{
			Destroy(gameObject, destroyDelay);
		}
	}

	private void Awake()
	{
		health = healthMax;
	}

	public bool ChangeHealth(float amt)
	{
		if (amt == 0) return true;
		if (amt < 0 && !canTakeDamage) return false;

		float lHealth = health;
		health = Mathf.Clamp(health + amt, 0, healthMax);

		if (lHealth == health) return false;

		if (amt < 0) OnDamage(lHealth - health); // True difference may not match amt
		if (health == 0) OnDeath();

		return true;
	}

	public float Health { get => health; }
	public float HealthMax { get => healthMax; }
}