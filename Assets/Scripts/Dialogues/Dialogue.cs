using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueController;

public class Dialogue : MonoBehaviour {

    public event Action DialogueComplete;
    public List<int> CharactersNeeded { get; private set; } = new List<int>();

    List<DialogueItem> Entries;
    UIController UIController;
    Coroutine dialogueRoutine = null;
    int currentEntry;

    public DialogueType Type;

    public void Init(UIController uiController, DialogueType type, List<DialogueItem> entries)
    {
        Type = type;
        Entries = entries;
        UIController = uiController;
        foreach (var e in entries)
        {
            if (!CharactersNeeded.Contains(e.Character))
                CharactersNeeded.Add(e.Character);
        }
    }

   
    void Start () {
		
	}
	
	void Update () {
		
	}

    public void Begin(float fadeoutTime)
    {
        if(dialogueRoutine == null)
            dialogueRoutine = StartCoroutine(DialogRoutine(fadeoutTime));
    }

    public void Stop()
    {
        if(dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine);
        }
    }

    IEnumerator DialogRoutine(float fadeoutTime)
    {
        for(int i = 0; i < Entries.Count; ++i)
        {
            var entry = Entries[i];
            yield return new WaitForSeconds(entry.WaitBefore);
            UIController.ShowDialogue(entry, fadeoutTime);
            yield return new WaitForSeconds(entry.Duration);
        }
        DialogueComplete?.Invoke();
    }
}
