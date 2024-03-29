using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// El enemigo sigue al jugador dentro de su rango de detecci�n
/// </summary>
[CreateAssetMenu (menuName = "AI/Actions/FollowPlayers")]
public class FollowPlayer : AIAction
{
    public override void Execute(AIManager manager)
    {
        FollowsPlayer(manager);   
    }

    /// <summary>
    /// Determina cuando el enemigo debe seguir al jugador
    /// </summary>
    /// <param name="manager"></param>
    private void FollowsPlayer(AIManager manager)
    {
        if (manager.reference == null)
        {
            return;
        }

        Vector3 directionTowardsPlayer = (manager.reference.position - manager.transform.position);
        Vector3 direction = directionTowardsPlayer.normalized;

        float distance = directionTowardsPlayer.magnitude;

        //As� el enemigo no colisiona con el jugador y no lo atraviesa
        if(distance >= 1.1)
        {
            manager.transform.Translate(direction * manager.speed * Time.deltaTime);
        }

    }
}
