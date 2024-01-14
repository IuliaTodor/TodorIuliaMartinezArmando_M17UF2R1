using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/AttackRange")]
public class AttackRange : AIDecision
{
    public override bool Decide(AIManager manager)
    {
       return OnAttackRange(manager);
    }
    //Indica si podemos hacer la transición para atacar
    private bool OnAttackRange(AIManager manager)
    {
        if(manager.reference == null)
        {
            return false;
        }

        float distance = (manager.reference.position - manager.transform.position).sqrMagnitude;

        if(distance < Mathf.Pow(manager.attackRange, 2))
        {
            return true;
        }

        return false;
    }
}
