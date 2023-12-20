using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Ammo")]
public class Ammo : Item
{
    public int ammoRestore;
    public override bool UseItem()
    {
        if (!Player.instance.isDead)
        {
            Weapon.instance.RegenerateAmmo(ammoRestore);
            return true;
        }

        return false;
    }

}
