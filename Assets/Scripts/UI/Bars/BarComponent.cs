using UnityEngine;

public abstract class BarComponent : MonoBehaviour, IBar
{
	public abstract void SetValue(float v);

	public abstract bool IsVisible { get; set; }
	public abstract bool ShowIfFull { get; }
	public abstract bool ShowIfEmpty { get; }
}