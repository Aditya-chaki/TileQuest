using UnityEngine;

public class GameController : MonoBehaviour
{
    public PrefabGenerator objectInstantiator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Example: press 'R' to reset
        {
            objectInstantiator.ResetPrefabSequence();
        }
    }
}
