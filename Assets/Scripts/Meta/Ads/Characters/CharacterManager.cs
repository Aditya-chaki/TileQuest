// using UnityEngine;
// using System.Collections.Generic;

// public class CharacterManager : MonoBehaviour
// {
//     public static CharacterManager Instance { get; private set; }

//     private Dictionary<string, Character> characters = new Dictionary<string, Character>();

//     private void Awake()
//     {
//         // Singleton pattern to ensure only one instance exists
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         RegisterAllCharacters();
//     }

//     // Find and register all characters in the scene
//     private void RegisterAllCharacters()
//     {
//         Character[] foundCharacters = FindObjectsOfType<Character>();
//         foreach (Character character in foundCharacters)
//         {
//             RegisterCharacter(character);
//         }
//     }

//     // Register a single character
//     public void RegisterCharacter(Character character)
//     {
//         if (!characters.ContainsKey(character.characterId))
//         {
//             characters.Add(character.characterId, character);
//         }
//     }

//     // Access a character by ID
//     public Character GetCharacterById(string id)
//     {
//         if (characters.TryGetValue(id, out Character character))
//         {
//             return character;
//         }
//         return null;
//     }

//     // You might also want to add methods to unregister characters, handle scene changes, etc.
// }
