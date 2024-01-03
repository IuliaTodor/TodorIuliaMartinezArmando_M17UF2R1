using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public Sprite icon;
    public bool hasExtraInteraction;
    public extraNPCInteraction extraInteraction;
    public Item NPCGiveItem;
    public int NPCGiveItemAmount;
    public Item requiredItemForExchange;
    

    [TextArea]public string initialText;

    public DialogueText[] dialogue;

    [TextArea] public string finalText;
    
    [TextArea]public string afterExtraInteractionText;

}
[Serializable]
public class DialogueText
{
    [TextArea] public string Dialoguetext;
}
