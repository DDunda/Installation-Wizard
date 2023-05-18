using UnityEngine;

public class SpawnPoint : MonoBehaviour, ISpawnpoint
{
	public Vector3 GetSpawnLocation() => transform.position;
}
