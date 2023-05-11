using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatus : MonoBehaviour
{
    [SerializeField]
    private float length = 5.0f; // how long the effect lasts, in seconds
    private BaseEnemy enemy;

    public BaseEnemy Enemy
	{
        set { enemy = value; }
	}

    // Update is called once per frame
    void Update()
    {
        // disable the enemy's ability to do anything
        // (this is done every frame, which is inefficient, but works)
        enemy.canMove = false;
        enemy.canAttack = false;

        // destroy this script once time has elapsed
        length -= Time.deltaTime;
        if (length <= 0) { Destroy(this); }
    }
}
