using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStraight : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float speed = 400;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }
    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
    }
}
