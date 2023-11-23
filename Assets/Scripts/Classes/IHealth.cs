using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int health;
    public bool isDead;
    private void HandleDeath();
    public void HandleDamage();
}
