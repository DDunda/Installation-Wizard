using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussive Projectile", menuName = "Concussive Projectile Type", order = 1)]
public class ConcussiveProjectileType : ProjectileType
{
    public PhysicsMaterial2D bounceMaterial;

	public override void OnCollide(Projectile p, GameObject other)
    {
        var stun = other.AddComponent<StunStatus>();
        stun.Enemy = other.GetComponent<BaseEnemy>();

        var knockBounce = other.AddComponent<KnockBounceStatus>();
        knockBounce.Enemy = other.GetComponent<BaseEnemy>();


        //BoxCollider2D col = other.GetComponent<BoxCollider2D>();
        Rigidbody2D rbOther = other.GetComponent<Rigidbody2D>();
        Rigidbody2D rbProjectile = p.GetComponent<Rigidbody2D>();

        Vector3 vProjectile = rbProjectile.velocity; //get the velocity of the projectile
        Vector3 vOther = rbOther.velocity;  //get the velocity of the other
 
        // Set the projectile's velocity to be the other's velocity
        vOther.x = vProjectile.x;
        vOther.y = vProjectile.y;
 
        rbOther.velocity = vOther; // actually apply values to the object

        rbOther.sharedMaterial = bounceMaterial; // set the object's material to be the bouncy one
        rbOther.drag = 0;
    }
	public override void OnDestroyProjectile(Projectile p)
    {

    }
}
