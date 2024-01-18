using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "AI/Decisions/DetectPlayer")]
public class DetectPlayer : AIDecision
{
    public override bool Decide(AIManager manager)
    {
      return DetectCharacter(manager);
    }
    /// <summary>
    /// Determina si el enemigo ve al jugador o no
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    public bool DetectCharacter(AIManager manager)
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(manager.transform.position, manager.detectionRange, manager.characterLayerMask); //Si colisiona con el jugador tendrá la referencia de su collider 
        //Si detecta al personaje
        if (detectedPlayer != null)
        {
            manager.reference = detectedPlayer.transform;
            return true; 
        }

        manager.reference = null;
        return false;
    }

}
