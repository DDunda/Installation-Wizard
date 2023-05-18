using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private List<GameObject> waves;
    public int waveIndex = 0;

    public void Start()
    {
        StartWave();
    }

    public void Update()
    {
        if (EnemyCounter.count == 0 && waveIndex != waves.Count)
        {
            StartWave();
        }
    }

    public void StartWave()
    {
        Wave currentWave = waves[waveIndex].GetComponent<Wave>();

        currentWave.isActive = true;

        waveIndex++;
    }


}
