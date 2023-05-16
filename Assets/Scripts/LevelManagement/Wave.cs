using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    //Enemy Variables
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> spawnOrder;
    public int enemyIndex = 0;
    public int spawnIndex = 0;
    
    //Spawn Variables
    [SerializeField] private List<GameObject> spawnLocations;
    private int currentSpawnPoint = 0;
    [SerializeField] private float spawnDelay;
    private bool canSpawn = true;
    public bool isActive = false;

    public void Update()
    {
        if (isActive && canSpawn)
        {
            StartCoroutine(SpawnWave());
        }
            
    }

    public IEnumerator SpawnWave()
    {
        canSpawn = false;
        //Gets the spawn point
        SpawnArea spawnPointGetter = spawnLocations[currentSpawnPoint].GetComponent<SpawnArea>();

        Vector3 spawnPoint = spawnPointGetter.GetSpawnLocation();

        //Determines which enemy to spawn and moves to the next one
        enemyIndex = spawnOrder[spawnIndex];

        spawnIndex++;

        if (spawnIndex ==  spawnOrder.Count)
        {
            isActive = false;
        }

        //Spawns the enemy
        Instantiate(enemies[enemyIndex], spawnPoint, Quaternion.identity);

        //Moves to the next spawn point if there is one otherwise starts from the first spawn point again
        currentSpawnPoint++;
        if (currentSpawnPoint == spawnLocations.Count)
        {
            currentSpawnPoint = 0;
        }

        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
    }
}
