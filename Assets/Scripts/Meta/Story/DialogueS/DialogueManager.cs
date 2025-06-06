// using System.Collections.Generic;
// using UnityEngine;

// namespace VNGame
// {
//     public class DialogueManager : MonoBehaviour
//     {
//         public TextAsset csvFile;
//         public Dictionary<string, List<Dialogue>> dialogues = new Dictionary<string, List<Dialogue>>();

//         void Start()
//         {
//             LoadCSV();
//         }

//         void LoadCSV()
//         {
//             string[] data = csvFile.text.Split(new char[] { '\n' });

//             for (int i = 1; i < data.Length; i++)
//             {
//                 string[] row = data[i].Split(new char[] { ',' });
//                 if (row.Length >= 3) // Ensure there are enough columns
//                 {
//                     Dialogue dialogue = new Dialogue();
//                     dialogue.Invariant = row[0];
//                     dialogue.EN = row[1];
//                     dialogue.Character = row[2];

//                     if (!dialogues.ContainsKey(dialogue.Invariant))
//                     {
//                         dialogues[dialogue.Invariant] = new List<Dialogue>();
//                     }
//                     dialogues[dialogue.Invariant].Add(dialogue);
//                 }
//             }
//         }
//     }

//     [System.Serializable]
//     public class Dialogue
//     {
//         public string Invariant;
//         public string EN;
//         public string Character;
//         // Add other fields as needed
//     }
// }

using System.Collections.Generic;
using UnityEngine;

namespace VNGame
{
    public class DialogueManager : MonoBehaviour
    {
        public TextAsset csvFile;
        public Dictionary<string, List<Dialogue>> dialogues = new Dictionary<string, List<Dialogue>>();

        void Start()
        {
            LoadCSV();
        }

        void LoadCSV()
        {
            string[] data = csvFile.text.Split(new char[] { '\n' });

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                if (row.Length >= 3)
                {
                    Dialogue dialogue = new Dialogue
                    {
                        Invariant = row[0].Trim(),
                        EN = row[1].Trim().Trim('"'),
                        Character = row[2].Trim()
                    };

                    if (!dialogues.ContainsKey(dialogue.Invariant))
                        dialogues[dialogue.Invariant] = new List<Dialogue>();

                    dialogues[dialogue.Invariant].Add(dialogue);
                }
            }
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string Invariant;
        public string EN;
        public string Character;
    }
}
