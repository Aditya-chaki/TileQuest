using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Event Data")]
public class EventData : ScriptableObject
{
    public Sprite backgroundImage; // The image to display
    public string eventText;       // The text to display
}
