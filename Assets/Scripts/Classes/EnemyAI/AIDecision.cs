using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa las decisiones que va a tomar la IA
/// </summary>
public abstract class AIDecision : ScriptableObject
{
    /// <summary>
    /// Decide si es momento de ejecutar la decisión de la IA
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    public abstract bool Decide(AIManager manager);
    

    
}
