using AptGames.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueController : MonoBehaviour {

    public enum DialogueType
    {
        Waiting,
        EnterRoom,
        CharacterDied,
        FoundCharacter,
    }

    struct CharWithTrait
    {
        public int Index;
        public PersonalityTrait Trait;
    }

    public GameController GameController;
    public PlayerController PlayerController;
    public MapController MapController;
    public UIController UIController;
    public float FadeoutTime = 1;
    public float WaitThreshold = 5;
    public Dialogues Dialogues;

    Dialogue currentDialogue;
    Coroutine dialogueRoutine;
    DialogueLine currentLine;
    List<CharWithTrait> charactersInDialogue = new List<CharWithTrait>();

    float timeSinceLastDialogue = 0;

    private void Awake()
    {
        PlayerController.CharacterAdded += OnCharacterAdded;
        PlayerController.CharacterDied += OnCharacterDied;
        MapController.RoomSelected += OnRoomSelected;
    }

    void Start () {
	}

	void Update () {
        if (currentDialogue == null)
        {
            //triggering DialogueType.Waiting
            if (timeSinceLastDialogue >= WaitThreshold)
            {
                StartDialogue(GetDialogueFromType(DialogueType.Waiting));
            }
            else
            {
                timeSinceLastDialogue += Time.deltaTime;
            }
        }
    }

    Dialogue GetDialogueFromType(DialogueType type)
    {
        List<Dialogue> dialogueList = null;
        switch(type)
        {
            case DialogueType.Waiting: dialogueList = Dialogues.Waiting; break;
            case DialogueType.CharacterDied: dialogueList = Dialogues.CharacterDied; break;
            case DialogueType.FoundCharacter: dialogueList = Dialogues.CharacterFound; break;
            case DialogueType.EnterRoom: dialogueList = Dialogues.EnterRoom; break;
        }
        if(dialogueList != null)
        {
            foreach(var d in dialogueList)
            {
                List<PersonalityTrait> traitsNeeded = d.GetPersonalitiesNeeded();
                List<PersonalityTrait> traitsFound = new List<PersonalityTrait>();
                charactersInDialogue.Clear();
                foreach(var trait in traitsNeeded)
                {
                    for (int i = 0; i < PlayerController.Characters.Count; ++i)
                    {
                        if(!charactersInDialogue.Any(c => c.Trait == trait))
                        {
                            var character = PlayerController.Characters[i];
                            if(character.CharacterInfo.Personality == trait &&
                                !charactersInDialogue.Any(c => c.Index == i))
                            {
                                charactersInDialogue.Add(new CharWithTrait { Index = i, Trait = trait });
                            }
                        }
                    }
                    if(charactersInDialogue.Count == traitsNeeded.Count)
                    {
                        return d;
                    }
                }
            }
        }
        return null;
    }

    public void Reset()
    {
        if (currentDialogue != null)
        {
            StopDialogue();
        }
        timeSinceLastDialogue = 0;
    }

    void StartDialogue(Dialogue dialogue)
    {
        if (dialogue == null)
            return;

        currentDialogue = dialogue;
        if (dialogueRoutine == null)
            dialogueRoutine = StartCoroutine(DialogRoutine());
    }

    private void OnCharacterAdded(CharacterBehaviour character)
    {
        //TODO: Start dialogue if char is discovered
        if(currentDialogue == null)
        {
            StartDialogue(GetDialogueFromType(DialogueType.FoundCharacter));
        }
    }

    private void OnCharacterDied(CharacterBehaviour character)
    {
        //NOTE: Stop dialogue if needed char is killed
        if (currentDialogue != null)
        {
            var indexOfDead = PlayerController.Characters.IndexOf(character);
            if (charactersInDialogue.Any(c => c.Index == indexOfDead))
            {
                StopDialogue();
            }
        }
        //TODO: Start dialogue if any char is killed
        if (currentDialogue == null)
        {
            StartDialogue(GetDialogueFromType(DialogueType.CharacterDied));
        }
    }

    void OnRoomSelected(RoomBehaviour room)
    {
        //TODO: Start dialogue when char is about the enter room
        if(currentDialogue == null)
        {
            StartDialogue(GetDialogueFromType(DialogueType.EnterRoom));
        }
        //TODO: Start dialog after char entered room without trap.
    }

    void StopDialogue()
    {
        if(dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine);
        }
        if(currentDialogue != null)
        {
            currentDialogue = null;
        }
    }

    IEnumerator DialogRoutine()
    {
        for (int i = 0; i < currentDialogue.DialogueLines.Count; ++i)
        {
            var line = currentDialogue.DialogueLines[i];
            var charIndex = charactersInDialogue.First(c => c.Trait == line.CharacterTraitsNeeded).Index;
            yield return new WaitForSeconds(line.WaitBefore);
            UIController.ShowDialogue(charIndex, line, FadeoutTime);
            yield return new WaitForSeconds(line.Duration);
        }
        dialogueRoutine = null;
        currentDialogue = null;
        timeSinceLastDialogue = 0;
    }
}
