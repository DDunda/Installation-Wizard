using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //Enemy Variables
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<int> spawnOrder;
    [SerializeField] private List<int> enemiesPerWave;
    private int enemyIndex = 0;
    private int spawnIndex = 0;
    private int waveIndex = 0;
    private int endWave = 0;
    
    //Spawn Variables
    [SerializeField] private List<GameObject> spawnLocations;
    private int currentSpawnPoint = 0;
    [SerializeField] private float spawnDelay;
    private bool canSpawn = true;

    //This value should be adjusted when an enemy is killed
    public int enemiesRemaining;

    private void Start()
    {
        StartNewWave();
    }

    public void Update()
    {
        if (spawnIndex < endWave)
        {
            if (canSpawn)
            {
                Debug.Log("Attempting to Spawn Wave");
                enemyIndex = spawnOrder[spawnIndex];
                StartCoroutine(Spawn(enemies[enemyIndex], spawnLocations[currentSpawnPoint]));
                spawnIndex++;
                currentSpawnPoint++;
                if (currentSpawnPoint == spawnLocations.Count)
                {
                    currentSpawnPoint = 0;
                }
            }
        }

        if (enemiesRemaining == 0 && spawnIndex >= endWave)
        {
            waveIndex++;

            if (waveIndex < enemiesPerWave.Count)
            {
                StartNewWave();
            }
        }
    }

    public void StartNewWave()
    {
        Debug.Log("NewWaveStarting");
        enemiesRemaining = enemiesPerWave[waveIndex];
        endWave += enemiesRemaining;
    }

    //Spawns the enemy.
    public IEnumerator Spawn(GameObject enemy, GameObject spawnPoint)
    {
        canSpawn = false;

        Debug.Log("spawning Enemy" +  enemyIndex);
        Instantiate(enemy, spawnPoint.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(spawnDelay);

        canSpawn = true;
    }

}
