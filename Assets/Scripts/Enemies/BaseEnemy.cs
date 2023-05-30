using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Runtime.InteropServices.WindowsRuntime;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class BaseEnemy : MonoBehaviour, IPausable, ITeams
{
    //Component Declaration
    protected Rigidbody2D rb;
    [SerializeField, EnumMask]
    protected Team _team;

    //Player Information
    protected Transform player;
    protected Vector3 playerPosition;
    protected Vector2 playerPosition2D;
    protected Vector2 directionToPlayer;
    protected Vector3 lastSeenPosition;

    //Object States Declaration
    protected bool playerInLOS = false;
    protected bool seenPlayer = false;
    public bool canAttack = true;
    public bool canMove = true;

    //Enemy Values
    [SerializeField] protected float fireRate;
    [SerializeField] protected float moveSpeed;

    [SerializeField] private LayerMask barrierLayer;

    //Pathfinding Variables
    protected Path currentPath;
    protected int currentWaypoint = 0;
    [SerializeField] float nextWaypointDistance;

    Seeker seeker;

    // Responds to pausing by disabling the enemy completely
    public void OnPause() => enabled = false;
    public void OnResume() => enabled = true;

    public Team team { get => _team; set => _team = value; }
    
    //Initialises components
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("PlayerHitBox").transform;
        seeker = GetComponent<Seeker>();

        lastSeenPosition = transform.position;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    protected virtual void Update()
    {
        //Update the player's position as they move
        GetPlayerLocation();
        //Determine whether the enemy can see the player
        CanSeePlayer();
        // If the enemy has not seen the player yet remain inactive
        if (!seenPlayer) return;
        //If the enemy can see the player move towards them and shoot
        if (playerInLOS)
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        if (canMove)
        {
            Move();
        }
    }

    //Updates the player's location and direction from the enemy.
    public void GetPlayerLocation()
    {
        playerPosition = player.position;
        playerPosition2D = player.position;
        directionToPlayer = playerPosition - transform.position;
    }

    public void CanSeePlayer()
    {
        RaycastHit2D hitObject = Physics2D.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, barrierLayer);

        playerInLOS = hitObject.collider == null;

        // If the player is detected, the corresponding object state is updated.
        if (playerInLOS)
        {
            seenPlayer = true;
            lastSeenPosition = playerPosition;
        }
    }

    //Fires a projectile at the player if the player is in line of sight.
    public virtual IEnumerator Attack()
    {
        canAttack = false;
        Debug.Log("Pew");
        //Create instance of enemy projectile
        yield return new WaitWithPause(fireRate);
        canAttack = true;
    }

    //Base enemy pathfinding
    //Creates a new path to the player
    public void GetPath()
    {
        if (canMove)
        {
            seeker.StartPath(rb.position, playerPosition, OnPathComplete);
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
        Vector2 force = direction * moveSpeed;

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
