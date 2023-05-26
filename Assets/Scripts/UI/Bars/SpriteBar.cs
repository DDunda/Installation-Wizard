using UnityEngine;

public class SpriteBar : BarComponent
{
	public SpriteRenderer barSprite;
	public SpriteRenderer[] allSprites;

	private Range<Vector2> size;
	private bool _spritesEnabled = true;

	[SerializeField] private bool _showIfFull = true;
	[SerializeField] private bool _showIfEmpty = true;

	public override bool ShowIfFull { get => _showIfFull; }
	public override bool ShowIfEmpty { get => _showIfEmpty; }

	private void Awake()
	{
		size = new(new(1,1), barSprite.size);
		foreach (var s in allSprites) s.enabled = _spritesEnabled;
	}

	public override bool IsVisible
	{
		get => _spritesEnabled;
		set
		{
			if (value == _spritesEnabled) return;
			foreach (var s in allSprites) s.enabled = value;
			_spritesEnabled = value;
		}
	}
	public override void SetValue(float v)
	{
		bool hidden = (!ShowIfEmpty && v <= 0.0) || (!ShowIfFull && v >= 1.0);
		IsVisible = !hidden;

		if (hidden) return;

		barSprite.size = size.Lerp(v);
	}
}
