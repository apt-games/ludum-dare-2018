using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    public enum DialogueType
    {
        Waiting,
        EnterRoom,
        CharacterDied,
        FoundCharacter,
    }

    public GameController GameController;
    public PlayerController PlayerController;
    public UIController UIController;
    public float FadeoutTime = 1;
    public float WaitThreshold = 5;

    List<Dialogue> dialogues = new List<Dialogue>();
    Dialogue currentDialogue = null;
    //List<int> completedDialogueIndices = new List<int>();
    float timeSinceLastDialogue = 0;

    private void Awake()
    {
        ConstructDialogues();
        PlayerController.PlayersChanged += OnPlayersChanged;
    }

    void Start () {
	}

    void ConstructDialogues()
    {
        //TODO: generate dialogues dynamically based on number of teammembers
        dialogues.Clear();

        var helloWorld = new List<DialogueItem>
        {
            new DialogueItem(0, "Hello World!"),
            new DialogueItem(1, "Oh shut up!"),
            new DialogueItem(0, "You shut up"),
        };
        dialogues.Add(ConstructDialogue(DialogueType.Waiting, helloWorld));
    }

    Dialogue ConstructDialogue(DialogueType type, List<DialogueItem> entries)
    {
        var gameObject = new GameObject("Dialogue");
        gameObject.AddComponent<Dialogue>();
        Dialogue dialogue = gameObject.GetComponent<Dialogue>();
        dialogue.Init(UIController, type, entries);
        return dialogue;
    }

	// Update is called once per frame
	void Update () {
        if(currentDialogue == null)
        {
            //triggering DialogueType.Waiting
            if (timeSinceLastDialogue >= WaitThreshold)
            {
                //TODO: make generic
                StartDialogue(dialogues[0]);
            }
            else
            {
                timeSinceLastDialogue += Time.deltaTime;
            }
        }
    }

    public void Reset()
    {
        if(currentDialogue != null)
        {
            currentDialogue.Stop();
        }
        timeSinceLastDialogue = 0;
    }

    void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        int charactersAccountedFor = 0;
        foreach(var cId in dialogue.CharactersNeeded)
        {
            if (PlayerController.Characters.Count > cId)
            {
                charactersAccountedFor++;
            }
        }
        if(charactersAccountedFor == dialogue.CharactersNeeded.Count)
        {
            currentDialogue.DialogueComplete += OnDialogueComplete;
            currentDialogue.Begin(FadeoutTime);
        }
        else
        {
            currentDialogue = null;
            timeSinceLastDialogue = 0;
        }
    }

    void OnDialogueComplete()
    {
        currentDialogue.DialogueComplete -= OnDialogueComplete;
        currentDialogue = null;
        timeSinceLastDialogue = 0;
    }

    void OnPlayersChanged()
    {
        //TODO: Stop dialogue if needed char is killed
        //TODO: Start dialogue if any char is killed
        //TODO: Start dialogue if char is discovered
        //TODO: Start dialogue when char is about the enter room
        //TODO: Start dialog after char entered room without trap.
    }
}
