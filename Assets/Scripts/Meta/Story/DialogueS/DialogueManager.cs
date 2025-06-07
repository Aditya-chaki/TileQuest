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
                if (string.IsNullOrWhiteSpace(data[i])) continue;

                string[] row = ParseCSVRow(data[i]);

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
                else
                {
                    Debug.LogWarning($"Skipping malformed CSV line at {i}: {data[i]}");
                }
            }
        }

        /// <summary>
        /// Safely parses CSV lines handling quoted commas and escaped quotes.
        /// </summary>
        private string[] ParseCSVRow(string line)
        {
            var fields = new List<string>();
            bool inQuotes = false;
            string field = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    // Escaped quote
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        field += '"';
                        i++; // Skip next quote
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(field);
                    field = "";
                }
                else
                {
                    field += c;
                }
            }

            fields.Add(field); // last field
            return fields.ToArray();
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

