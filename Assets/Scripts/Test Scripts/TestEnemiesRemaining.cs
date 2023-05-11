using UnityEngine;

public class TestEnemiesRemaining : MonoBehaviour
{
    private int _count = 0;
    void Update()
    {
        if(_count != EnemyCounter.count)
        {
			_count = EnemyCounter.count;
            Debug.Log(_count);
		}
    }
}
