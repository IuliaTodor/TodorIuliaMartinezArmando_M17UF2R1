using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandlePlayer : MonoBehaviour
{
    public Camera camera;
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
        aim = camera.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.Euler(0, 0, Convert.ToSingle(getAngle(transform.position, aim)));
    }

    void HandleDeath()
    {

    }

    public double getAngle(Vector2 me, Vector2 target) {
    	return Math.Atan2(target.y - me.y, target.x - me.x) * (180/Math.PI);
    }

}
