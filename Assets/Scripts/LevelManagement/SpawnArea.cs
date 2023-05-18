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
}
