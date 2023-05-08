using System.Collections.Generic;
using UnityEngine;

public interface IPauser
{
	bool RegisterListener(IPausable l);
	bool UnregisterListener(IPausable l);
	bool SetPaused(bool c);
	bool Pause();
	bool Resume();
	void TogglePause();
}

public class Pauser : IPauser
{
	public static Pauser instance { get; private set; } = new();

	private HashSet<IPausable> pauseListeners = new();
	public bool paused { get; private set; } = false;

	~Pauser() {
		if(instance == this)
		{
			instance = null;
		}
	}

	private void BroadcastPause()
	{
		foreach (var p in pauseListeners)
		{
			p.OnPause();
		}
	}

	private void BroadcastResume()
	{
		foreach (var p in pauseListeners)
		{
			p.OnResume();
		}
	}

	public bool RegisterListener(IPausable l)
	{
		if (pauseListeners.Contains(l)) return false;

		pauseListeners.Add(l);

		return true;
	}

	public bool UnregisterListener(IPausable l)
	{
		if (!pauseListeners.Contains(l)) return false;

		pauseListeners.Remove(l);

		return true;
	}

	public bool SetPaused(bool c)
	{
		if (c == paused) return false;

		paused = c;

		if (paused) BroadcastPause();
		else BroadcastResume();

		return true;
	}

	public bool Pause()
	{
		if (paused) return false;

		paused = true;
		BroadcastPause();

		return true;
	}

	public bool Resume()
	{
		if (!paused) return false;

		paused = false;
		BroadcastResume();

		return true;
	}

	public void TogglePause()
	{
		paused = !paused;

		if (paused) BroadcastPause();
		else BroadcastResume();
	}
}

public class PauseManager : MonoBehaviour, IPauser
{
	private Pauser pauser { get; } = new();

	public bool RegisterListener(IPausable p) => pauser.RegisterListener(p);
	public bool UnregisterListener(IPausable p) => pauser.UnregisterListener(p);
	public bool SetPaused(bool c) => pauser.SetPaused(c);
	public bool Pause() => pauser.Pause();
	public bool Resume() => pauser.Resume();
	public void TogglePause() => pauser.TogglePause();
}
