using UnityEngine;

public class DialogueLoader
{
    public static DialogueSet LoadDialogueSet(string filename)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Dialogues/" + filename);
        if (jsonFile == null)
        {
            Debug.LogError("Dialogue file not found: " + filename);
            return null;
        }
        return JsonUtility.FromJson<DialogueSet>(jsonFile.text);
    }
}
