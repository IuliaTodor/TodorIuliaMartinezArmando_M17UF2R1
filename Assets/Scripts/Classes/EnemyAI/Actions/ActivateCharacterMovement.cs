using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activa el path del enemigo para que lo siga si este entra en su rango
/// </summary>
[CreateAssetMenu(menuName = "AI/Actions/ActivateCharacterMovement")]
public class ActivateCharacterMovement : AIAction
{

    public override void Execute(AIManager manager)
    {
        if(manager.characterMovement == null)
        {
            return;
        }

        manager.characterMovement.enabled = true;

    }


}
