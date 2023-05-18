using UnityEngine;

public class SpawnCircle : MonoBehaviour, ISpawnpoint
{
    [SerializeField, Min(0)] float radius;

	public Vector3 GetSpawnLocation() => transform.position + (Vector3)(Random.insideUnitCircle * radius);
}
