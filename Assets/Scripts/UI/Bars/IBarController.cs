public interface IBar
{
	// Expects a value 0..1
	public void SetValue(float v);

	public bool IsVisible { get; set; }
	public bool ShowIfFull { get; }
	public bool ShowIfEmpty { get; }
}