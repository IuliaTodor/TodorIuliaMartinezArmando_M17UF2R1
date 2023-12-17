using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IHealth, IDamage
{
    public Inventory inventory;
    public Weapon activeWeapon;

    [SerializeField]
    private int _health; //Esta propiedad solo es para que aparezca en el inspector, ya que el get set no lo permitía. Lo mismo para maxHealth
    public int health
    {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private int _maxHealth;
    public int maxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }
    public bool isDead { get; set; }

    public static Action characterDeathEvent;

    public BoxCollider2D boxCollider2D;
    public Animator animator;
    public static Player instance;

    void Awake()
    {
        instance = this;
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        health = maxHealth;
        isDead = false;
        UIManager.instance.UpdateCharacterHealth(health, maxHealth);
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

    //Death logic
    public void HandleDeath()
    {
        isDead = true;
        boxCollider2D.enabled = false;
        characterDeathEvent?.Invoke(); //El ? significa "si no es null". Es decir, si no es null que haga Invoke.
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
    public void HandleDamage(int damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            UIManager.instance.UpdateCharacterHealth(health, maxHealth);

            if (health <= 0)
            {
                UIManager.instance.UpdateCharacterHealth(health, maxHealth);
                HandleDeath();
            }
        }
    }
    public void RestoreHealth(int healthRestored)
    {
        //Restaura vida si al jugador le queda vida y no está muerto 
        if (health < maxHealth && !isDead)
        {
            health += healthRestored;

            //Así la vida no supera a la máxima si recupera más del total
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            UIManager.instance.UpdateCharacterHealth(health, maxHealth);
        }
    }
}