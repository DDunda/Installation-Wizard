using UnityEngine;

public class TestPause : MonoBehaviour
{
	public KeyCode button;

	void Update()
	{
		if (Input.GetKeyDown(button))
		{
			Pauser.instance.TogglePause();
		}
	}
}