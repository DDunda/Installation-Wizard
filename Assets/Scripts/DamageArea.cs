using UnityEngine;

public class DamageArea : EffectArea
{
	public float damageRate;

	public override void DoEffect(GameObject go, float delta) {
		EntityHealth health;

		if (!go.TryGetComponent(out health)) return;

		health.ChangeHealthContinuous(-damageRate * delta);
	}
}
