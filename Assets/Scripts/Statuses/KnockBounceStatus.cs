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
        // destroy this script once time has elapsed
        length -= Time.deltaTime;
        if (length <= 0) 
        { 
            this.gameObject.GetComponent<Rigidbody2D>().sharedMaterial = material;
            this.gameObject.GetComponent<Rigidbody2D>().drag = drag;
            Destroy(this);
        }
    }

    public void OnCollide(GameObject other)
	{
		length = 0.2f;
        if(other.layer == 7)
        {
            length = 0.2f;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        length = 0.2f;
        if(collision.transform.gameObject.layer == 7)
        {
            length = 0.2f;
        }
    }
}
