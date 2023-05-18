using UnityEngine;

public class SpawnArea : MonoBehaviour, ISpawnpoint
{
    [SerializeField, Min(0)] float width;
    [SerializeField, Min(0)] float height;

    public Vector3 GetSpawnLocation()
	{
		var randWidth = Random.Range(-0.5f * width, 0.5f * width);
		var randHeight = Random.Range(-0.5f * height, 0.5f * height);

		var randPos = new Vector3(randWidth, randHeight);

		return transform.position + randPos;
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 corner1 = transform.position + new Vector3(-0.5f * width, 0.5f * height);
		Vector3 corner2 = transform.position + new Vector3(-0.5f * width, -0.5f * height);
		Vector3 corner3 = transform.position + new Vector3( 0.5f * width, -0.5f * height);
		Vector3 corner4 = transform.position + new Vector3( 0.5f * width, 0.5f * height);

		Gizmos.DrawLine(corner1, corner2);
		Gizmos.DrawLine(corner2, corner3);
		Gizmos.DrawLine(corner3, corner4);
		Gizmos.DrawLine(corner4, corner1);
	}
}
