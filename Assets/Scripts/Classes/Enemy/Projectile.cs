using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El proyectil que dispara el enemigo projectile
/// </summary>
public class Projectile : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5;
    /// <summary>
    /// El tiempo que lleva activo el proyectil
    /// </summary>
    private float startTime;
    /// <summary>
    /// El tiempo antes de que el proyectil se destruya
    /// </summary>
    private float lifeTime = 25;

    void Start()
    {

        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        startTime = Time.time;
        boxCollider2D.enabled = true;
    }

    void Update()
    {
        if ((Time.time - startTime) > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           
            AIManager.instance.DamagePlayer(AIManager.instance.enemyDamage); 
            Debug.Log(AIManager.instance.enemyDamage);
            boxCollider2D.enabled = false;
        }
    }
}
