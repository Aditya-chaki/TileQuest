using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
}

[System.Serializable]
public class DialogueSet
{
    public string id; // Unique ID for this set
    public DialogueLine[] lines;
}
