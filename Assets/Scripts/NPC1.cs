using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC1 : MonoBehaviour
{
    public GameManClass refGameMan;
    // Text UI white bloc and text UI
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] GameObject textBloc;

    // Dialogue Arrays before and after completion state check
    [SerializeField] DialogueInfo[] introDialogues;
    [SerializeField] DialogueInfo[] completionDialogues;

    // Portrait UIs
    [SerializeField] GameObject nPCPortrait;
    [SerializeField] GameObject playerPortrait;

    // Check item collect completion
    [SerializeField] bool playerItemCollected = false;

    // Check player state
    public bool introducingToPlayer = false;
    public bool completingPlayer = false;
    public bool playerIntroduced = false;

    // Dialogue length limit check
    int introIndex = 0;
    int numberOfIntroDialogues;

    int complIndex = 0;
    int numberOfComplDialogues;

    // Cache
    string textToPrint;
   // PlayerMove player;
    CircleCollider2D myTriggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        numberOfIntroDialogues = introDialogues.Length;
        numberOfComplDialogues = completionDialogues.Length;
        myTriggerCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player is just greeting
        if (introducingToPlayer)
        {
            if (Input.GetKeyUp(KeyCode.Return) && introIndex < numberOfIntroDialogues)
            {
                PrintIntroDialogue(introIndex++);
            }
            else if (Input.GetKeyDown(KeyCode.Return) && introIndex == numberOfIntroDialogues)
            {
                Debug.Log("Finish intro");
                introducingToPlayer = false;
                playerIntroduced = true;
                refGameMan.dialogueinAction = false;
            }
        }

        // If player is returning object
        if (completingPlayer)
        {
            if (Input.GetKeyUp(KeyCode.Return) && complIndex < numberOfComplDialogues)
            {
                PrintComplDialogue(complIndex++);
            }
            else if (Input.GetKeyUp(KeyCode.Return) && complIndex == numberOfComplDialogues)
            {
                completingPlayer = false;
                refGameMan.dialogueinAction = false;
                myTriggerCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !playerItemCollected && !playerIntroduced)
        {
            refGameMan.dialogueinAction = true;
            introducingToPlayer = true;

            PrintIntroDialogue(introIndex);
        }
        else if(collision.tag == "Player" && !playerItemCollected && playerIntroduced)
        {
            return;
        }
        else if(collision.tag == "Player" && playerItemCollected && playerIntroduced)
        {
            refGameMan.dialogueinAction = true;
            completingPlayer = true;

            PrintComplDialogue(complIndex);
        }
    }

    private void PrintIntroDialogue(int index)
    {
        textToPrint = introDialogues[index].dialogueText;
        textUI.text = textToPrint;

        // Turn the Portrait UIs on or off
        if (!introDialogues[index].playerSpeaks)
        {
            nPCPortrait.SetActive(true);
            playerPortrait.SetActive(false);
        }
        else if (introDialogues[index].playerSpeaks)
        {
            nPCPortrait.SetActive(false);
            playerPortrait.SetActive(true);
        }
    }

    private void PrintComplDialogue(int index)
    {
        textToPrint = completionDialogues[index].dialogueText;
        textUI.text = textToPrint;

        // Turn the Portrait UIs on or off
        if (!introDialogues[index].playerSpeaks)
        {
            nPCPortrait.SetActive(true);
            playerPortrait.SetActive(false);
        }
        else if (introDialogues[index].playerSpeaks)
        {
            nPCPortrait.SetActive(false);
            playerPortrait.SetActive(true);
        }
    }
}