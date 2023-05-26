using UnityEngine;

public class HealthbarController : MonoBehaviour
{
	[SerializeField]
	private EntityHealth health;
	[SerializeField]
	private BarComponent healthbar;

	private void Update()
	{
		if (healthbar == null || health == null) return;
		healthbar.SetValue(health.Health / health.HealthMax);
	}
}
