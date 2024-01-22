using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Los tipos de interacción extra de NPC
/// </summary>
public enum extraNPCInteraction
{
    GiveItem,
    Exchange,
    Shop,
}

[CreateAssetMenu]
public class NPCDialogue : ScriptableObject
{
    public string NPCName;
    public int NPCGiveItemAmount;
    public bool hasExtraInteraction;

    public extraNPCInteraction extraInteraction;
    public Sprite icon;
    public Item NPCGiveItem;
    public Item requiredItemForExchange;

 
    [TextArea]public string initialText;
    public DialogueText[] dialogue;
    [TextArea] public string finalText;
}
[Serializable]
public class DialogueText
{
    [TextArea] public string Dialoguetext;
}
