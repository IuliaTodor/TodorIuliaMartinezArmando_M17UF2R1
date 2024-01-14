using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/AttackPlayer")]
public class AttackPlayer : AIAction
{
    public override void Execute(AIManager manager)
    {
        Attack(manager);
    }

    private void Attack(AIManager manager)
    {
        if (manager.reference == null || manager.AttackTime() == false)
        {
            return;
        }

        if (manager.AttackRange(manager.TypeOfAttackRange))
        {
            if(manager.attackType == AttackType.Tackle)
            {
                manager.TackleAttack(manager.enemyDamage);
            }
            else
            {
                manager.BombAttack(manager.enemyDamage);
            }

            manager.BombAttack(manager.enemyDamage);
            manager.UpdateTimeBetweenAttacks();
        }


    }

}
