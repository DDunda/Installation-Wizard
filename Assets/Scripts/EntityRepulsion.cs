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
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(layer);
    }

    private void Update()
    {
        List<Collider2D> colliders = new();
        if (rigidBody.OverlapCollider(contactFilter, colliders) == 0) return;

        HashSet<Rigidbody2D> rbs = new(from r in colliders select r.attachedRigidbody);

        Vector2 force = Vector2.zero;

        foreach(Rigidbody2D r in rbs)
        {
            Vector2 off = rigidBody.transform.position - r.transform.position;
            if (off.magnitude >= maxDistance) continue;

            force += off.normalized * Mathf.InverseLerp(maxDistance, 0, off.magnitude) * maxForce;
        }

        rigidBody.AddForce(force);
    }
}
