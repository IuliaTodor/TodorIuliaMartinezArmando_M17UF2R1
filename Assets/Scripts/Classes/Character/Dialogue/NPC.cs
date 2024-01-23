using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject npcButtonInteraction;
    [SerializeField] public NPCDialogue npcDialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.instance.AvailableNPC = this; //Para que se cargue el diálogo de este NPC
            npcButtonInteraction.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialogueManager.instance.AvailableNPC = null;
            npcButtonInteraction.SetActive(false);
        }
    }
}
