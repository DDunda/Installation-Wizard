using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EnemySpawn
{
	public GameObject enemy;
	[Min(1)] public uint count;
	[Min(0)] public float spawnDelay;
}

public class Wave : MonoBehaviour
{
	//Spawn Variables
	[SerializeField] private List<EnemySpawn> enemySpawns;
	[SerializeField] private List<GameObject> spawnLocations;
	public bool isActive = false;

	// The delay for the wave beginning
	[SerializeField, Min(0)] private float startDelay;
	// The delay between groups of enemies spawning
	[SerializeField, Min(0)] private float spawnSetDelay;

	public IEnumerator SpawnWave()
	{
		isActive = true;

		int currentSpawnPoint = 0;
		
		if (startDelay > 0) yield return new WaitWithPause(startDelay);

		for (int spawnIndex = 0; spawnIndex < enemySpawns.Count; spawnIndex++)
		{
			EnemySpawn es = enemySpawns[spawnIndex];

			GameObject enemy = es.enemy;

			for (uint i = 0; i < enemySpawns[spawnIndex].count; i++)
			{
				// Gets the spawn point
				ISpawnpoint spawnPointGetter = spawnLocations[currentSpawnPoint].GetComponent<ISpawnpoint>();

				currentSpawnPoint++;
				currentSpawnPoint %= spawnLocations.Count;

				Vector3 spawnPoint = spawnPointGetter.GetSpawnLocation();

				// Spawns the enemy
				Instantiate(enemy, spawnPoint, Quaternion.identity);

				if(es.spawnDelay > 0) yield return new WaitWithPause(es.spawnDelay);
			}

			if (spawnSetDelay > 0) yield return new WaitWithPause(spawnSetDelay);
		}

		isActive = false;
	}
}
