using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IHealth, IDamage
{
    public Inventory inventory;
    public Weapon activeWeapon;
    public int health {get;set;}
    public bool isDead {get;set;}
    // Start is called before the first frame update
    void Start()
    {
        health = 0;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleDeath() {}
    public void HandleDamage() {}
}
