using UnityEngine;

[CreateAssetMenu(fileName = "UIConfig", menuName = "UI/UI Config", order = 1)]
public class UIConfig : ScriptableObject
{
    // Position and scale data
    public Vector2 character1Position;
    public Vector2 character1Scale = Vector2.one;
    public Vector2 character2Position;
    public Vector2 character2Scale = Vector2.one;
    public Vector2 backgroundPosition;
    public Vector2 backgroundSize = Vector2.one;

    // Sprite data
    public Sprite character1Sprite;
    public Sprite character2Sprite;
    public Sprite backgroundSprite;
}
