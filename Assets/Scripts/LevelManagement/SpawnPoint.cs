using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnpoint
{
	public Vector3 GetSpawnLocation() => transform.position;

	void OnDrawGizmos()
	{
		Extensions.DrawGizmoSpiral1(transform.position, transform.lossyScale, 0.1f, 6, 15f, 10);
	}
}
