using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //Enemy Variables
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> indexToSpawn;
    private int currentIndex = 0;
    [SerializeField] private int maximumEnemies;
    private int enemiesSpawned;

    //Spawn Area Variables
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] private List<Transform> fixedSpawnLocations;
    [SerializeField] private bool randomLocation;
    [SerializeField, Min(1)] private uint maxRandomAttempts = 1;
    [SerializeField] private Vector3 defaultLocation;
    [SerializeField] private float minDistanceFromPlayer;


    //Spawn Time Variables
    [SerializeField] private float fixedSpawnDelay;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    private bool canSpawn = true;
    [SerializeField] private bool randomInterval;

    private void Update()
    {
        //Only Spawn an enemy if not on cooldown
        if (canSpawn && enemiesSpawned < maximumEnemies)
        {
            //Spawn the enemy
            StartCoroutine(Spawn(enemies[currentIndex]));

            //Move to the next enemy in the list
            currentIndex = (currentIndex + 1) % indexToSpawn.Count;
        }
    }

    public IEnumerator Spawn(GameObject enemy)
    {
        canSpawn = false;
        var randDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        //Checks if randomSpawnIntervals are required
        var spawnDelay = randomInterval ? randDelay : fixedSpawnDelay;
        Vector3 spawnLocation = defaultLocation;

        if (randomLocation)
        {
            for(int attempts = 0; attempts < maxRandomAttempts; attempts++)
            {
                Vector3 loc = GenerateRandomPosition();

                //Chooses a new location if the chose location has an object present
                if (Physics2D.OverlapCircle(loc, 1.3f) == null)
                {
                    spawnLocation = loc;
                    break;
                }
            }
        } else
        {
            if (fixedSpawnLocations.Count > 0)
            {
                int i = Random.Range(0, fixedSpawnLocations.Count);
                spawnLocation = fixedSpawnLocations[i].position;
            }
        }

        enemiesSpawned++;
        //Spawn the enemy
        Instantiate(enemy, spawnLocation, Quaternion.identity);

        //wait for cooldown before spawning again
        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
    }

    public Vector3 GenerateRandomPosition()
    {
        var randX = Random.Range(-0.5f * width, 0.5f * width);
        var randY = Random.Range(-0.5f * height, 0.5f * height);

        return new Vector3(randX, randY);
    }
}
