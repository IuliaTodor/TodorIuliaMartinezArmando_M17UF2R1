using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, IHealth, IDamage
{
    //Esta propiedad solo es para que aparezca en el inspector, ya que el get set no lo permit�a. Lo mismo para maxHealth
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

    public static Action characterDeathEvent;

    public BoxCollider2D boxCollider2D;
    public Rigidbody2D rb;
    public Animator animator;
    public static Player instance;
    // de nuestra clase Inputs, del new input system
    private Inputs input;

    private Vector2 movementVector = Vector2.zero;

    [SerializeField] public string idleLayer;
    [SerializeField] private string walkLayer;

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
        EnableLayer(walkLayer);
    }
    void OnMovementCancelled(InputAction.CallbackContext value)
    {
        movementVector = Vector2.zero;
        EnableLayer(idleLayer);
    }

    /// <summary>
    /// Cambia las layers del Animator entre Walk y Idle
    /// </summary>
    /// <param name="layer"></param>
    public void EnableLayer(string layer)
    {
        //Primero apaga todas las layers para evitar problemas
        for(int i = 0; i <animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0); //El peso del layer. 0 desactivado. 1 activado
        }

        animator.SetLayerWeight(animator.GetLayerIndex(layer), 1);
    }

    void Start()
    {
        health = maxHealth;
        isDead = false;
        UIManager.instance.UpdatePlayerHealth(health, maxHealth);
        rb.bodyType = RigidbodyType2D.Dynamic;
        EnableLayer(idleLayer);
    }

    private void FixedUpdate() 
    {
        rb.velocity = movementVector * speed;
    }

    /// <summary>
    /// Lógica de muerte
    /// </summary>
    public void HandleDeath()
    {
        isDead = true;
        boxCollider2D.enabled = false;

        rb.bodyType = RigidbodyType2D.Static;
        characterDeathEvent?.Invoke(); //El ? significa "si no es null". Es decir, si no es null que haga Invoke.
        StartCoroutine(UIManager.instance.GameOverMenu());
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
    }

    /// <summary>
    /// Cuando el jugador recibe daño
    /// </summary>
    /// <param name="damageTaken"></param>
    public void HandleDamage(float damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            UIManager.instance.UpdatePlayerHealth(health, maxHealth);

            if (health <= 0)
            {
                //Así evitamos números negativos
                health = 0;
                UIManager.instance.UpdatePlayerHealth(health, maxHealth);
                HandleDeath();
            }
        }
    }
    /// <summary>
    /// Restaura vida si al jugador le queda vida y no est� muerto 
    /// </summary>
    /// <param name="healthRestored"></param>
    public void RestoreHealth(int healthRestored)
    {      
        if (health < maxHealth && !isDead)
        {
            health += healthRestored;

            //As� la vida no supera a la m�xima si recupera m�s del total
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UIManager.instance.UpdatePlayerHealth(health, maxHealth);
        }
    }
}