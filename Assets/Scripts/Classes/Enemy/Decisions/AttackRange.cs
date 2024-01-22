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
    /// <summary>
    /// Indica si podemos hacer la transición para atacar
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    private bool OnAttackRange(AIManager manager)
    {
        if(manager.reference == null)
        {
            return false;
        }

        float distance = (manager.reference.position - manager.transform.position).sqrMagnitude;

        if(distance < Mathf.Pow(manager.TypeOfAttackRange(), 2))
        {
            return true;
        }

        return false;
    }
}
