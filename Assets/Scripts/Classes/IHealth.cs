using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int health {get;set;}
    public bool isDead {get;set;}
    private void HandleDeath() {}
    public void HandleDamage() {}
}
