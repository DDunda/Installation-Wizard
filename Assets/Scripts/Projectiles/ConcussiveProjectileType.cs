using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Concussive Projectile", menuName = "Concussive Projectile Type", order = 1)]
public class ConcussiveProjectileType : ProjectileType
{
    public float knockback;

	public override void OnCollide(Projectile p, GameObject other)
    {

    }
	public override void OnDestroyProjectile(Projectile p)
    {

    }
}
