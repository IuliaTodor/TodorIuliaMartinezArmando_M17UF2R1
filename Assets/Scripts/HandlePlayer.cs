using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayer : MonoBehaviour
{
    public float speed;
    public Vector2 aim;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();

        HandleAttack();

        HandleAim();
    }

    void HandleMove() 
    {
        float moveHorizontal = Input.GetAxisRaw ("Horizontal");
        float moveVertical = Input.GetAxisRaw ("Vertical");

        Vector2 direction = new Vector2 (moveHorizontal, moveVertical);
        if (direction.sqrMagnitude > 1) { direction = direction.normalized; }

        rb.velocity = direction * speed;
    }

    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("Pum te pego");
        }
    }

    void HandleAim()
    {
        aim = Input.mousePosition;
        print(Vector2.Angle(transform.position, aim));
        transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(transform.position, aim));
    }

    void HandleDeath()
    {

    }
}
