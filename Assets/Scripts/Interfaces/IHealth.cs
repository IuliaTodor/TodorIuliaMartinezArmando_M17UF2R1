using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public float health { get; set; }
    public float maxHealth { get; set; }
    public bool isDead { get; set; }
    public void HandleDeath() {}
    public void HandleDamage() {}
}
