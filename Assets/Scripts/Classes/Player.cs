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
    void Start()
    {
        health = maxHealth;
        isDead = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            HandleDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestoreHealth(10);
        }
    }

    //Death logic
    public void HandleDeath()
    {
        characterDeathEvent?.Invoke(); //El ? significa "si no es null". Es decir, si no es null que haga Invoke.
    }


    public void HandleDamage(int damageTaken)
    {
        if (health > 0)
        {
            health -= damageTaken;
            UpdateLifebar(health, maxHealth);

            if (health <= 0)
            {
                UpdateLifebar(health, maxHealth);
                isDead = true;
                HandleDeath();
            }
        }
    }

    public void UpdateLifebar(float health, float maxHealth)
    {
        // Implement your life bar update logic here
    }

    public void RestoreHealth(int healthRestored)
    {
        if (health < maxHealth)
        {
            health += healthRestored;

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            UpdateLifebar(health, maxHealth);
        }
    }
}