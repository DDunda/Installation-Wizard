using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private List<GameObject> waves;
    public int waveIndex = 0;
    public bool repeatWaves = false;

    private Wave _currentWave = null;

    public void Start()
    {
        StartWave();
    }

    public void Update()
    {
        if ((_currentWave == null | !_currentWave.isActive) && EnemyCounter.count == 0)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        if (waveIndex >= waves.Count)
        {
            if (!repeatWaves) return;
            else waveIndex = 0;
        }

        if (waves[waveIndex].TryGetComponent(out _currentWave))
        {
            StartCoroutine(_currentWave.SpawnWave());
        }

		waveIndex++;
	}
}
