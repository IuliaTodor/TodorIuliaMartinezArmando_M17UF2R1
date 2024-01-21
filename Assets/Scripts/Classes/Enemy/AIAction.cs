using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa las acciones que va a ejecutar la IA
/// </summary>
public abstract class AIAction : ScriptableObject //Usando abstract toda clase que herede de esta usará su método principal
{
    /// <summary>
    /// Ejecuta la acción de la IA
    /// </summary>
    /// <param name="manager"></param>
    public abstract void Execute(AIManager manager);
}
