using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITalkable
{
    public string dialog {get;set;}
    private void HandleTalk() { }
}
