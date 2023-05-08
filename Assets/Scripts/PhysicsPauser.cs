using UnityEngine;

public class PhysicsPauser : MonoBehaviour, IPausable
{
	private void Awake()
    {
        this.RegisterPause();
    }

    private void OnDestroy() {
        this.UnregisterPause();
    }

    public void OnPause()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
	}

	public void OnResume()
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
}
