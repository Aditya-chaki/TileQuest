using UnityEngine;

[CreateAssetMenu(fileName = "NewEventData", menuName = "Event Data")]
public class EventData : ScriptableObject
{

    public string eventText;       // The text to display
    public int food;               // Amount of food to deduct
    public int strength;           // Amount of strength to deduct
    public int health;             // Amount of health to deduct
    public int gold;               // Amount of gold to deduct
}
