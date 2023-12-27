using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum extraNPCInteraction
{
    GiveItem,
    Shop,
}

[CreateAssetMenu]
public class NPCDialogue : ScriptableObject
{
    public string NPCName;
    public Sprite icon;
    public bool hasExtraInteraction;
    public extraNPCInteraction extraInteraction;

    [TextArea]public string initialText;

    public DialogueText[] dialogue;

    [TextArea] public string finalText;

}
[Serializable]
public class DialogueText
{
    [TextArea] public string Dialoguetext;
}
