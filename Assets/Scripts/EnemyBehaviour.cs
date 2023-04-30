using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //Component Declaration
    private Rigidbody2D rb;

    //Player Information
    private Transform player;
    private Vector3 playerPosition;
    private Vector3 playerDirection;
    private Vector3 lastSeenPosition;

    //Object States Declaration
    private bool canSeePlayer = false;
    public bool canShoot = true;

    //Projectile Declaration
    [SerializeField] private GameObject bullet;

    //Enemy Values
    [SerializeField] private float fireRate = 2f;

    private void Start()
    {
        //Initialises the Components of the Object
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        CanSeePlayer();
        if (canShoot && canSeePlayer)
        {
            StartCoroutine(Shoot());
        }
    }

    private void FixedUpdate()
    {

    }

    //Checks if the enemy has line of sight with the player.
    public void CanSeePlayer()
    {
        UpdatePlayerInformation();

        RaycastHit2D hitObject = Physics2D.Raycast(transform.position, playerDirection);

        // If the player is detected, the corresponding object state is updated.
        if (hitObject.collider != null)
        {
            if (hitObject.collider.gameObject.CompareTag("Player"))
            {
                canSeePlayer = true;
                lastSeenPosition = playerPosition;
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }

    //Updates the player's location and direction from the enemy.
    public void UpdatePlayerInformation()
    {
        playerPosition = player.position;
        playerDirection = playerPosition - transform.position;
    }

    //Fires a projectile at the player if the player is in line of sight.
    public IEnumerator Shoot()
    {
        canShoot = false;
        Debug.Log("Pew");
        //Create instance of enemy projectile
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
