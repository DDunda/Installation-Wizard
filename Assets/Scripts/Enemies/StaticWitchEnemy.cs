using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticWitchEnemy : BaseEnemy
{
    public GameObject attackPrefab;
    [SerializeField] private int boltsPerAttack = 5;
    [SerializeField] private float boltInterval = 1;
    private float spawnDelay = 1;
    public bool randomSpawn = false;

    private void Awake()
    {
        canAttack = false;
        StartCoroutine(SpawnDelay());
    }

    // Update is called once per frame
    protected override void Update()
    {
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();

        if (canMove)
        {
            Move();
        }

        if (canAttack)
        {
            StartCoroutine(Attack());
        }

        if(randomSpawn)
        {
            StartCoroutine(SpawnRandom());
        }
    }

    public IEnumerator SpawnRandom()
    {
        randomSpawn = false;
        var randHeight = Random.Range(-9f,9f);
        var randWidth = Random.Range(-16, 16);
        Vector2 spawnLocation = new Vector2(randHeight, randWidth);
        Instantiate(attackPrefab, spawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        randomSpawn = true;
    }

    public override IEnumerator Attack()
    {
        canAttack = false;
        for (int i = 0; i < boltsPerAttack; i++)
        {
            Instantiate(attackPrefab, playerPosition, Quaternion.identity);
            yield return new WaitForSeconds(boltInterval);
        }
        
        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }

    public IEnumerator SpawnDelay()
    {
        yield return new WaitWithPause(spawnDelay);
        canAttack = true;
        randomSpawn = true;
    }
}
