using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class AIState : ScriptableObject
{
    public AIAction[] actions;
    public AITransition[] transitions;


    public void ExecuteState(AIManager manager)
    {
        ExecuteActions(manager);
        MakeTransitions(manager);
    }

    private void ExecuteActions(AIManager manager)
    {

        if(actions == null || actions.Length <= 0) {return;}

        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Execute(manager); //Ejecutamos todas las acciones en un estado
        }
    }

    private void MakeTransitions(AIManager manager)
    {
        if(transitions == null || transitions.Length <= 0) { return; }

        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionValue = transitions[i].decision.Decide(manager);
            if(decisionValue)
            {
                manager.ChangeState(transitions[i].stateTrue);
            }
            else
            {
                manager.ChangeState(transitions[i].stateFalse);
            }
        }
    }
}
