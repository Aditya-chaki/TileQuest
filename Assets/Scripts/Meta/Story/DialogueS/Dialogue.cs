// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// public class Dialogue : MonoBehaviour
// {
//     public TextMeshProUGUI textComponent;
//     public string[] lines;
//     public float textSpeed;
//     public GameObject scene;
    
    
//     private int index;

//     void Start()
//     {
//         textComponent.text = string.Empty;
//         StartDialogue();
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             if (textComponent.text == lines[index])
//             {
//                 NextLine();
//             }
//             else
//             {
//                 StopAllCoroutines();
//                 textComponent.text = lines[index];
//             }
//         }
//     }

//     void StartDialogue()
//     {
//         index = 0;
//         StartCoroutine(TypeLine());
//     }

//     IEnumerator TypeLine()
//     {
//         foreach (char c in lines[index].ToCharArray())
//         {
//             textComponent.text += c;
//             yield return new WaitForSeconds(textSpeed);
//         }
//     }

//     void NextLine()
//     {
//         if (index < lines.Length - 1)
//         {
//             index++;
//             textComponent.text = string.Empty;
//             StartCoroutine(TypeLine());
//         }
//         else
//         {
//             scene.SetActive(false);
//             SceneManager.LoadScene("Menu");
           
//         }
//     }
// }
