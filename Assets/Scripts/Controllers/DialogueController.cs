using AptGames.Utils;
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
    public MapController MapController;
    public UIController UIController;
    public float FadeoutTime = 1;
    public float WaitThreshold = 5;

    Dictionary<DialogueType, List<Dialogue>> dialogues = new Dictionary<DialogueType, List<Dialogue>>();
    Dictionary<DialogueType, int> dialogueIndex = new Dictionary<DialogueType, int>() {
        {DialogueType.Waiting, 0 },
        {DialogueType.EnterRoom, 0 },
        {DialogueType.CharacterDied, 0 },
        {DialogueType.FoundCharacter, 0 },
    };
    Dialogue currentDialogue = null;
    float timeSinceLastDialogue = 0;

    private void Awake()
    {
        ConstructDialogues();
        PlayerController.CharacterAdded += OnCharacterAdded;
        PlayerController.CharacterDied += OnCharacterDied;
        MapController.RoomSelected += OnRoomSelected;
    }

    void Start () {
	}

    void ConstructDialogues()
    {
        dialogues.Add(DialogueType.Waiting, ConstructWaitingDialogues());
        dialogues.Add(DialogueType.CharacterDied, ConstructCharacterDiedDialogues());
    }

    List<Dialogue> ConstructWaitingDialogues()
    {
        //TODO: generate dialogues dynamically based on number of teammembers
        var waitingDialogues = new List<Dialogue>();

        var helloWorld = new List<DialogueItem>
        {
            new DialogueItem(0, "Hello World!"),
            new DialogueItem(1, "Oh shut up!"),
            new DialogueItem(0, "You shut up"),
        };
        waitingDialogues.Add(ConstructDialogue(DialogueType.Waiting, helloWorld));

        return waitingDialogues;
    }

    List<Dialogue> ConstructCharacterDiedDialogues()
    {
        var diedDialogues = new List<Dialogue>();

        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(0, "Noooooooooo!")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(1, "Whatever...")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(0, "Finally!")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(1, "What the...")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(2, "Why would you even go in there?")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(0, "Good riddance!")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(1, "Another one bites the dust")}));
        diedDialogues.Add(ConstructDialogue(DialogueType.CharacterDied,
            new List<DialogueItem> { new DialogueItem(0, "I wonder if they had life insurance")}));

        return diedDialogues;
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
                StartDialogue(GetDialogueFromType(DialogueType.Waiting));
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

    Dialogue GetDialogueFromType(DialogueType type)
    {
        var index = dialogueIndex[type]++;
        index %= dialogues[type].Count;
        return dialogues[type][index];
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

    private void OnCharacterAdded(CharacterBehaviour character)
    {
        //TODO: Start dialogue if char is discovered
    }

    private void OnCharacterDied(CharacterBehaviour character)
    {
        //NOTE: Stop dialogue if needed char is killed
        if(currentDialogue != null)
        {
            var indexOfDead = PlayerController.Characters.IndexOf(character);
            if(currentDialogue.CharactersNeeded.Contains(indexOfDead))
            {
                currentDialogue.Stop();
            }
        }
        //TODO: Start dialogue if any char is killed
        if(currentDialogue == null)
        {
            StartDialogue(GetDialogueFromType(DialogueType.CharacterDied));
        }
    }

    void OnRoomSelected(RoomBehaviour room)
    {
        //TODO: Start dialogue when char is about the enter room
        //TODO: Start dialog after char entered room without trap.
    }
}
