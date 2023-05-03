using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            //Checks if the list has reached the end and restarts it
            if (currentIndex == indexToSpawn.Count)
            {
                currentIndex = 0;
            }
            //Spawn the enemy
            StartCoroutine(Spawn(enemies[currentIndex]));

            enemiesSpawned++;

            //Move to the next enemy in the list
            currentIndex++;
        }
    }

    public IEnumerator Spawn(GameObject enemy)
    {
        canSpawn = false;
        var randDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        //Checks if randomSpawnIntervals are required
        var spawnDelay = randomInterval ? randDelay : fixedSpawnDelay;

        var randX = Random.Range(-0.5f * width, 0.5f * width);
        var randY = Random.Range(-0.5f * height, 0.5f * height);
        var randLocation = new Vector3 (randX, randY);

        //Checks if the location is currently occupied
        var objectsAtLocation = Physics2D.OverlapCircle(randLocation, 1.3f);

        //Chooses a new location if the chose location has an object present
        while (objectsAtLocation != null)
        {
            randX = Random.Range(-0.5f * width, 0.5f * width);
            randY = Random.Range(-0.5f * height, 0.5f * height);
            randLocation = new Vector3(randX, randY);
            objectsAtLocation = Physics2D.OverlapCircle(randLocation, 1.3f);
        }

        var spawnLocation = randomLocation ? randLocation : fixedSpawnLocations[0].position;

        //Spawn the enemy
        Instantiate(enemy, spawnLocation, Quaternion.identity);

        //wait for cooldown before spawning again
        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
    }
}
