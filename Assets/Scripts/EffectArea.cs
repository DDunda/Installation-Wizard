using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EffectArea : MonoBehaviour
{
	public TeamRelationship targets;
	public LayerMask entityLayer;

	private ContactFilter2D filter;
	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		filter.SetLayerMask(entityLayer);
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public abstract void DoEffect(GameObject go, float delta);

	void FixedUpdate()
	{
		List<Collider2D> colliders = new();
		if (rigidbody.OverlapCollider(filter, colliders) == 0) return;

		GameObject[] objects = (from c in colliders where c.gameObject != gameObject select c.gameObject).ToArray();

		EntityTeams t;
		float delta = Time.fixedDeltaTime;
		foreach (var o in objects)
		{
			if (!o.TryGetComponent(out t) || !targets.GetRelationship(t.Teams)) continue;
			DoEffect(o, delta);
		}
	}
}
