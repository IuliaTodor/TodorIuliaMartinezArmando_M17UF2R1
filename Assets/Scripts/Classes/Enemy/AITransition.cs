using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AITransition
{
    public AIDecision decision;
    public AIState stateTrue;
    public AIState stateFalse;
}
