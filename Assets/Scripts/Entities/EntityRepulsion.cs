using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityRepulsion : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField, Min(0)] private float maxForce;
    [SerializeField, Min(0)] private float maxDistance;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        List<RaycastHit2D> hits = new(Physics2D.CircleCastAll(transform.position, maxDistance, Vector2.zero, 0, layer));

        hits.RemoveAll(c => c.rigidbody == rigidBody);

        if (hits.Count == 0) return;

        Vector2 force = Vector2.zero;

        foreach(RaycastHit2D h in hits)
        {
            Vector2 off = rigidBody.position - h.point;
            //if (off.magnitude >= maxDistance) continue;

            force += off.normalized * maxForce * Mathf.InverseLerp(maxDistance, 0, off.magnitude) * maxForce;
        }

        rigidBody.AddForce(force);
    }
}
