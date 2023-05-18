using UnityEngine;

public class HealthbarController : MonoBehaviour
{
	public EntityHealth health;

	public SpriteRenderer healthbarSprite;
	public SpriteRenderer[] allSprites;

	public bool showIfFull = false;
	public bool showIfDead = false;

	private Range<Vector2> size;
	private bool _spritesEnabled = true;

	private void Awake()
	{
		size = new(new(1,1), healthbarSprite.size);
		foreach (var s in allSprites) s.enabled = _spritesEnabled;
	}

	private void Update()
	{
		bool hidden = (!showIfDead && health.Health == 0) || (!showIfFull && health.Health == health.HealthMax);
		EnableSprites(!hidden);

		if (hidden) return;

		healthbarSprite.size = size.Lerp(Mathf.InverseLerp(0, health.HealthMax, health.Health));
	}

	protected void EnableSprites(bool e)
	{
		if (e == _spritesEnabled) return;
		foreach (var s in allSprites) s.enabled = e;
		_spritesEnabled = e;
	}
}
