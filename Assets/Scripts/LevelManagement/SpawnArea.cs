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

		randPos.Scale(transform.lossyScale);

		return transform.position + randPos;
	}
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Vector3 corner1 = new(-0.5f * width,  0.5f * height);
		Vector3 corner2 = new(-0.5f * width, -0.5f * height);
		Vector3 corner3 = new( 0.5f * width, -0.5f * height);
		Vector3 corner4 = new( 0.5f * width,  0.5f * height);

		Vector3 upright = new(0.5f, 0.5f);
		Vector3 upleft = new(-0.5f, 0.5f);
		Vector3 downright = new(0.5f, -0.5f);
		Vector3 downleft = new(-0.5f, -0.5f);

		corner1.Scale(transform.lossyScale);
		corner2.Scale(transform.lossyScale);
		corner3.Scale(transform.lossyScale);
		corner4.Scale(transform.lossyScale);

		corner1 += transform.position;
		corner2 += transform.position;
		corner3 += transform.position;
		corner4 += transform.position;

		upright.Scale(transform.lossyScale);
		upleft.Scale(transform.lossyScale);
		downright.Scale(transform.lossyScale);
		downleft.Scale(transform.lossyScale);

		Gizmos.DrawLine(corner1, corner2);
		Gizmos.DrawLine(corner2, corner3);
		Gizmos.DrawLine(corner3, corner4);
		Gizmos.DrawLine(corner4, corner1);

		Gizmos.DrawLine(corner1, corner1 + upleft);
		Gizmos.DrawLine(corner2, corner2 + downleft);
		Gizmos.DrawLine(corner3, corner3 + downright);
		Gizmos.DrawLine(corner4, corner4 + upright);
	}
}
