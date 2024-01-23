using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El proyectil que dispara el enemigo projectile
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    private GameObject player;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    private Camera cam;

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
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        Vector2 direction = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) - transform.position;
        Debug.Log(direction);
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
        if ( collision.name == "RedMonke" || collision.name == "BlueMonke" || collision.name == "GreenMonke")
        {
            collision.gameObject.GetComponent<HandleEnemyHealth>().HandleDamage(1);
            // Hacer que la colision reciba daño

            boxCollider2D.enabled = false;
        }
    }
}
