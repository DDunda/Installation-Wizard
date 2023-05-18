using UnityEngine;

public class SpawnCircle : MonoBehaviour, ISpawnpoint
{
    [SerializeField, Min(0)] float radius;

	public Vector3 GetSpawnLocation()
	{
		Vector3 p = Random.insideUnitCircle * radius;
		p.Scale(transform.lossyScale);
		return p + transform.position;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Extensions.DrawGizmoCircle(transform.position, transform.lossyScale, radius);
	}
}
