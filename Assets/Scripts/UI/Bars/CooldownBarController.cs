using UnityEngine;

public class CooldownBarController : MonoBehaviour
{
	[SerializeField]
	private Ability ability;
	[SerializeField]
	private BarComponent cooldownBar;

	private void Update()
	{
		if (cooldownBar == null || ability == null) return;
		cooldownBar.SetValue(ability.RechargePercent);
	}
}