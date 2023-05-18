using UnityEngine;

// Equivalent to WaitForSeconds, but can be safely used with pausing
public class WaitWithPause : CustomYieldInstruction, IPausable
{

	private bool paused;
	private float delay;

	public override bool keepWaiting
	{
		get
		{
			if (paused) return true;
			delay -= Time.deltaTime;
			return delay > 0;
		}
	}

	public void OnPause() => paused = true;
	public void OnResume() => paused = false;

	public WaitWithPause(float t)
	{
		delay = t;
		paused = Pauser.instance.paused;
		this.RegisterPause();
	}

	~WaitWithPause()
	{
		this.UnregisterPause();
	}
}