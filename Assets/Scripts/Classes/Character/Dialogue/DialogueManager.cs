using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image NPCIcon;
    [SerializeField] private TextMeshProUGUI NPCNameTMP;
    [SerializeField] private TextMeshProUGUI NPCDialogueTMP;

    public NPC AvailableNPC { get; set; }

    private Queue<string> dialogueSequence; //Almacena los diálogos a mostrar
    private bool animatedDialogue;
    private bool showFinalText;

    private void Start()
    {
       dialogueSequence = new Queue<string>();
    }

    private void Update()
    {
        if(AvailableNPC == null)
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.E))
        {
            ManageDialogueBox(AvailableNPC.npcDialogue);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(showFinalText)
            {
                OpenCloseDialogue(false);
                showFinalText= false;
                return;
            }

            if(animatedDialogue)
            {
                ContinueDialogue();
            }
        }


    }

    private void Awake()
    {
        instance = this;
    }

    public void OpenCloseDialogue(bool state)
    {
        dialogueBox.SetActive(state);
    }

    private void ManageDialogueBox(NPCDialogue dialogue)
    {
        OpenCloseDialogue(true);
        ShowDialogueSequence(dialogue);

        NPCIcon.sprite = dialogue.icon;
        NPCNameTMP.text = dialogue.NPCName;
        ShowAnimatedText(dialogue.initialText);
    }

    private void ShowDialogueSequence(NPCDialogue NPCdialogue)
    {
        if(NPCdialogue.dialogue == null || NPCdialogue.dialogue.Length <= 0)
        {
            return;
        }

        //Cargamos los diálogos del NPC
        for (int i = 0; i < NPCdialogue.dialogue.Length; i++)
        {
            dialogueSequence.Enqueue(NPCdialogue.dialogue[i].Dialoguetext);
        }


    }

    private void ContinueDialogue()
    {
        if(AvailableNPC == null)
        {
            return;
        }

        if(showFinalText)
        {
            return;
        }

        if(dialogueSequence.Count == 0)
        {
            string finalText = AvailableNPC.npcDialogue.finalText;
            ShowAnimatedText(finalText);
            showFinalText = true;
            return;
        }

        string nextDialogue = dialogueSequence.Dequeue();
        ShowAnimatedText(nextDialogue);
    }

    private IEnumerator AnimateText(string text)
    {
        animatedDialogue = false;
        NPCDialogueTMP.text = "";
        char[] characters = text.ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            NPCDialogueTMP.text += characters[i];
            yield return new WaitForSeconds(0.02f);
        }

        animatedDialogue = true;
    }

    private void ShowAnimatedText(string text)
    {
        StartCoroutine(AnimateText(text));
    }

}
