using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/Prompt")]
public class Prompt : Item
{
    public int HPRestore;

    public override bool UseItem()
    {
       if (!Player.instance.isDead)
       {
         Player.instance.RestoreHealth(1);
         Debug.Log(Player.instance.health);
         return true;
       }

        return false;
    }
}
