using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa las acciones que va a ejecutar la IA
/// </summary>
public abstract class AIAction : ScriptableObject //Usando abstract toda clase que herede de esta usar� su m�todo principal
{
    /// <summary>
    /// Ejecuta la acci�n de la IA
    /// </summary>
    /// <param name="manager"></param>
    public abstract void Execute(AIManager manager);
}
