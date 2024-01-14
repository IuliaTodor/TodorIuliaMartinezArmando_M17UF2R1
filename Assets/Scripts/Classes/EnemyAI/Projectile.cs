using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    [SerializeField] private float speed = 5;
    private float startTime;
    private float lifeTime = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
