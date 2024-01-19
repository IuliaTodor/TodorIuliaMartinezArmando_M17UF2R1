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

    /// <summary>
    /// Ejecuta una acción o otra dependiendo del rango de ataque del enemigo
    /// </summary>
    /// <param name="manager"></param>
    private void Attack(AIManager manager)
    {
        if (manager.reference == null || manager.IsAttackTime() == false)
        {
            return;
        }

        if (manager.AttackRange(manager.TypeOfAttackRange()))
        {
            switch (manager.attackType)
            {
                case AttackType.Bomb:
                    manager.BombAttack(manager.enemyDamage);
                    break;
                case AttackType.Tackle:
                    manager.TackleAttack(manager.enemyDamage);
                    manager.UpdateTimeBetweenAttacks();
                    break;
                case AttackType.Projectile:
                    manager.ProjectileAttack();
                    manager.UpdateTimeBetweenAttacks();
                    break;
            }
            
        }
    }
}
