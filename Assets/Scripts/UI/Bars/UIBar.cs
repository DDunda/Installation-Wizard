using UnityEngine;
using UnityEngine.UI;

public class UIBar : BarComponent
{
	public Image barImage;
	public Image[] allImages;

	[SerializeField] private Vector2 minSize = Vector2.one;
	private Range<Vector2> size;
	private bool _imagesEnabled = true;

	[SerializeField] private bool _showIfFull = true;
	[SerializeField] private bool _showIfEmpty = true;

	public override bool ShowIfFull { get => _showIfFull; }
	public override bool ShowIfEmpty { get => _showIfEmpty; }

	private void Awake()
	{
		size = new(minSize, barImage.rectTransform.sizeDelta);
		foreach (var i in allImages) i.enabled = _imagesEnabled;
	}

	public override bool IsVisible
	{
		get => _imagesEnabled;
		set
		{
			if (value == _imagesEnabled) return;
			foreach (var s in allImages) s.enabled = value;
			_imagesEnabled = value;
		}
	}
	public override void SetValue(float v)
	{
		bool hidden = (!ShowIfEmpty && v <= 0.0) || (!ShowIfFull && v >= 1.0);
		IsVisible = !hidden;

		if (hidden) return;

		barImage.rectTransform.sizeDelta = size.Lerp(v);
	}
}
