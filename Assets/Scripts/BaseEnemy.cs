using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.InteropServices.WindowsRuntime;

public class BaseEnemy : MonoBehaviour
{
    //Component Declaration
    private Rigidbody2D rb;

    //Player Information
    private Transform player;
    private Vector3 playerPosition;
    private Vector2 directionToPlayer;
    private Vector3 lastSeenPosition;

    //Object States Declaration
    private bool playerInLOS = false;
    private bool canAttack = true;
    private bool canMove = false;

    //Enemy Values
    [SerializeField] private float fireRate;
    [SerializeField] private float moveSpeed;

    [SerializeField] private LayerMask barrierLayer;

    //Pathfinding Variables
    private Path currentPath;
    private int currentWaypoint = 0;
    [SerializeField] float nextWaypointDistance;

    Seeker seeker;

    //Initialises components
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();

        lastSeenPosition = transform.position;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void Update()
    {
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();
        //If the enemy can see the player move towards them and shoot
        if (playerInLOS)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
            canMove = true;
        }
        Move();
    }

    //Updates the player's location and direction from the enemy.
    public void GetPlayerLocation()
    {
        playerPosition = player.position;
        directionToPlayer = playerPosition - transform.position;
    }

    public void CanSeePlayer()
    {
        RaycastHit2D hitObject = Physics2D.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, barrierLayer);

        playerInLOS = hitObject.collider == null;

        // If the player is detected, the corresponding object state is updated.
        if (playerInLOS)
        {
            lastSeenPosition = playerPosition;
        }
    }

    //Fires a projectile at the player if the player is in line of sight.
    public IEnumerator Attack()
    {
        canAttack = false;
        Debug.Log("Pew");
        //Create instance of enemy projectile
        yield return new WaitForSeconds(fireRate);
        canAttack = true;
    }

    //Base enemy pathfinding
    //Creates a new path to the player
    public void GetPath()
    {
        if (canMove)
        {
            seeker.StartPath(rb.position, lastSeenPosition, OnPathComplete);
        }
    }

    //Updates the existing path to the player's new location
    public void UpdatePath()
    {
        if (seeker.IsDone())
        {
            GetPath();
        }
    }

    //Attempts to move the enemy towards the player
    public void Move()
    {
        //If no path exists or the enemy cannot move, exit the function
        if (currentPath == null || canMove == false)
        {
            return;
        }
        
        //Determines whether or not we have reached the end of the path
        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            return;
        }

        //Calculates the direction and force in which to move the enemy
        Vector2 direction = ((Vector2)currentPath.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, currentPath.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    //After the path has been calculated, applies it to the enemy
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }
}
