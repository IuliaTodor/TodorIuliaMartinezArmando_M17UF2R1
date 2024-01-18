using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Desactiva el path base del enemigo si este tiene que seguir al personaje para evitar ejecutar dos acciones a la vez (caminar y seguir personaje)
/// </summary>
[CreateAssetMenu(menuName = "AI/Actions/DisableCharacterMovement")]
public class DisableCharacterMovement : AIAction
{
    public override void Execute(AIManager manager)
    {
        if (manager.characterMovement == null)
        {
            return;
        }

        manager.characterMovement.enabled = false;

    }
}
