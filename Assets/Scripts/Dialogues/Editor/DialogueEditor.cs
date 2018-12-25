using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditor : EditorWindow {

    public Dialogues Dialogues;

    [MenuItem("Window/Dialogue Editor %#e")]
    static void Init()
    {
        GetWindow(typeof(DialogueEditor));
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Dialogue editor", EditorStyles.boldLabel);
        if(Dialogues != null)
        {

        }
        GUILayout.EndHorizontal();
    }

    void CreateNewDialogue()
    {

    }
}
