using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, IHealth, IDamage
{
    public Inventory inventory;
    public Weapon activeWeapon;

    [SerializeField]
    private float _health; //Esta propiedad solo es para que aparezca en el inspector, ya que el get set no lo permit�a. Lo mismo para maxHealth
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

    public static Action characterDeathEvent;

    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rb;
    public Animator animator;
    public static Player instance;
    // de nuestra clase Inputs, del new input system
    private Inputs input;
    private Vector2 movementVector = Vector2.zero;

    void Awake()
    {
        instance = this;
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        input = new Inputs();
        speed = 10f;
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }
    void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    void OnMovementPerformed(InputAction.CallbackContext value)
    {
        movementVector = value.ReadValue<Vector2>();
        animator.SetFloat("XMovement", value.ReadValue<Vector2>().x);
        animator.SetFloat("YMovement", value.ReadValue<Vector2>().y);
    }
    void OnMovementCancelled(InputAction.CallbackContext value)
    {
        movementVector = Vector2.zero;
    }

    void Start()
    {
        health = maxHealth;
        isDead = false;
        UIManager.instance.UpdateCharacterHealth(health, maxHealth);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            HandleDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreHealth(2);
        }
    }

    private void FixedUpdate() 
    {
        rb.velocity = movementVector * speed;
    }

    //Death logic
    public void HandleDeath()
    {
        isDead = true;
        boxCollider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        characterDeathEvent?.Invoke(); //El ? significa "si no es null". Es decir, si no es null que haga Invoke.
        StartCoroutine(UIManager.instance.GameOverMenu());
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

    }

    public void HandleRespawn()
    {
        boxCollider2D.enabled = true;
        isDead = true;
        health = maxHealth;
        UIManager.instance.UpdateCharacterHealth(health, maxHealth);
        animator.SetBool(GameManager.instance.characterDeath, false);
    }

    //When the player recieves damage
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
    public void RestoreHealth(int healthRestored)
    {
        //Restaura vida si al jugador le queda vida y no est� muerto 
        if (health < maxHealth && !isDead)
        {
            health += healthRestored;

            //As� la vida no supera a la m�xima si recupera m�s del total
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UIManager.instance.UpdateCharacterHealth(health, maxHealth);
        }
    }
}