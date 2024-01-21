using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEnemyHealth : MonoBehaviour, IHealth, IDamage
{
    public static HandleEnemyHealth instance;
    [SerializeField]
    private float _health;
    public float health
    {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private float _maxHealth;
    public float maxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    public bool isDead { get; set; }

    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        isDead = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        UIManager.instance.UpdateCharacterHealth(health, maxHealth);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            HandleDamage(1);
        }
    }

    public void HandleDeath()
    {
        isDead = true;
        boxCollider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject);
    }

    public void HandleDamage(float damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            UIManager.instance.UpdateCharacterHealth(health, maxHealth);

            if (health <= 0)
            {
                health = 0;
                UIManager.instance.UpdateCharacterHealth(health, maxHealth);
                HandleDeath();
            }
        }
    }


}
