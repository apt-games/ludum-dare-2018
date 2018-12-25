using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Dialogues")]
public class Dialogues : ScriptableObject {
    public List<Dialogue> Waiting;
    public List<Dialogue> EnterRoom;
    public List<Dialogue> CharacterDied;
    public List<Dialogue> CharacterFound;
}
