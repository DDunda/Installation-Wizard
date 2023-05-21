using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownBarController : MonoBehaviour
{
	[SerializeField]
	private Ability ability;

	public SpriteRenderer cooldownSprite;
	public SpriteRenderer[] allSprites; // contains the bar and frame

	public bool showIfFull = false;
	public bool showIfDead = false;

	private Range<Vector2> size;
	private bool _spritesEnabled = true;

	private void Awake()
	{
		size = new(new(1, 1), cooldownSprite.size);
		foreach (SpriteRenderer s in allSprites) 
		{
			s.enabled = _spritesEnabled;
		}
	}

	private void Update()
	{
		float rechargePercent = ability.RechargePercent;

		bool hidden = (!showIfDead && rechargePercent == 0) || (!showIfFull && rechargePercent == 1);
		EnableSprites(!hidden);

		if (hidden) return;

		cooldownSprite.size = size.Lerp(rechargePercent);
	}

	protected void EnableSprites(bool e)
	{
		if (e == _spritesEnabled) return;
		foreach (var s in allSprites)
		{
			s.enabled = e;
		}
		_spritesEnabled = e;
	}
}
