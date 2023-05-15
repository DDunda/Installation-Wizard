using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBounceStatus : MonoBehaviour
{
    [SerializeField]
    private float length = 5.0f; // how long the effect lasts, in seconds
    private BaseEnemy enemy;

    public BaseEnemy Enemy
	{
        set { enemy = value; }
	}

    // store entity stats that are changed by the projectile and status
    private float drag;
    private PhysicsMaterial2D material;

    void Awake()
    {
        drag = this.gameObject.GetComponent<Rigidbody2D>().drag;
        material = this.gameObject.GetComponent<Rigidbody2D>().sharedMaterial;
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
        if (length <= 0) 
        { 
            enemy.canMove = true;
            this.gameObject.GetComponent<Rigidbody2D>().sharedMaterial = material;
            this.gameObject.GetComponent<Rigidbody2D>().drag = drag;
            Destroy(this);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.gameObject.layer == 7)
        {
            //this.gameObject.GetComponent<Rigidbody2D>().drag = drag;
            length = 0.2f;
        }
    }
}

