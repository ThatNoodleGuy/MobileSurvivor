using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;

    private BoxCollider2D bulletCollider;
    private Rigidbody2D bulletRB;

    private void Awake()
    {
        bulletCollider = GetComponent<BoxCollider2D>();
        bulletRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        bulletRB.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 2f);
    }
}