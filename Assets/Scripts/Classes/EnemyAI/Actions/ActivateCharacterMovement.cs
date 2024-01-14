using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/ActivateCharacterMovement")]
//Activa el path del enemigo para que lo siga si ya no sigue al player
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
