using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] float width;
    [SerializeField] float height;

    [SerializeField] bool isArea = true;

    public Vector3 GetRandomArea()
    {
        var randWidth = Random.Range(-0.5f*width, 0.5f*width);
        var randHeight = Random.Range(-0.5f * height, 0.5f * height);

        var randPos =  new Vector3(randWidth, randHeight);

        return transform.position - randPos;
    }

    public Vector3 GetSpawnLocation()
    {
        if (isArea)
        {
            return GetRandomArea();
        }
        else
        {
            return transform.position;
        }
    }
}
