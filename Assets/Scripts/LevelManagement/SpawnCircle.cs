using UnityEngine;

public class SpawnCircle : MonoBehaviour, ISpawnpoint
{
    [SerializeField, Min(0)] float radius;

	public Vector3 GetSpawnLocation() => transform.position + (Vector3)(Random.insideUnitCircle * radius);

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 lp = (Vector3)Extensions.Deg2Vec(360 * 31 / 32, radius) + transform.position;
		for(int i = 0; i < 32; i++)
		{
			Vector3 p = (Vector3)Extensions.Deg2Vec(360 * i / 32, radius) + transform.position;
			Gizmos.DrawLine(lp, p);
			lp = p;
		}
	}
}
