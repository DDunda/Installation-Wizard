using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //Enemies to Spawn
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 5f;
    [SerializeField] private int maxSpawns = 5;

    //Spawning Area Characteristics
    [SerializeField] private float height;
    [SerializeField] private float width;

    //Object States
    private bool canSpawn = true;
    private int activeSpawns = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && activeSpawns < maxSpawns)
        {
            //StartCoroutine(RandomSpawn(enemies[0]));
            StartCoroutine(StaticSpawn(enemies[0]));
        }
    }

    private IEnumerator RandomSpawn(GameObject enemy)
    {
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        var spawnLocation = new Vector3(Random.Range(-0.5f * width, 0.5f * width), Random.Range(-0.5f * height, 0.5f * height));
        Instantiate(enemy, spawnLocation, enemy.transform.rotation);
        canSpawn = false;
        activeSpawns++;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }

    private IEnumerator StaticSpawn(GameObject enemy)
    {
        var spawnLocation = new Vector3(Random.Range(-0.5f * width, 0.5f * width), Random.Range(-0.5f * height, 0.5f * height));
        Instantiate(enemy, spawnLocation, enemy.transform.rotation);
        canSpawn = false;
        activeSpawns++;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }
}
