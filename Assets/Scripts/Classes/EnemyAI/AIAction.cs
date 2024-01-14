using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction : ScriptableObject //Usando abstract toda clase que herede de esta usará su método principal
{
    public abstract void Execute(AIManager manager);
}
