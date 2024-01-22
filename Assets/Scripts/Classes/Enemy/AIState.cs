using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un estado del comportamiento de la IA
/// </summary>
[CreateAssetMenu(menuName = "AI/State")]
public class AIState : ScriptableObject
{
    public AIAction[] actions;
    public AITransition[] transitions;

    /// <summary>
    /// Ejecuta las acciones y hace transiciones
    /// </summary>
    /// <param name="manager"></param>
    public void ExecuteState(AIManager manager)
    {
        ExecuteActions(manager);
        MakeTransitions(manager);
    }

    /// <summary>
    /// Ejecuta las acciones dentro del array
    /// </summary>
    /// <param name="manager"></param>
    private void ExecuteActions(AIManager manager)
    {
        if(actions == null || actions.Length <= 0) {return;}

        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Execute(manager); //Ejecutamos todas las acciones dentro de en un estado
        }
    }

    /// <summary>
    /// Hace la transición del estado. Este depende de si la decisión es true o false.
    /// </summary>
    /// <param name="manager"></param>
    private void MakeTransitions(AIManager manager)
    {
        if(transitions == null || transitions.Length <= 0) { return; }

        for (int i = 0; i < transitions.Length; i++)
        {
            //Si la decisión es true ejecuta una acción, de lo contrario ejecuta otra
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
