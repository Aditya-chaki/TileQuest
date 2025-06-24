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

// using System.Collections.Generic;
// using UnityEngine;

// namespace VNGame
// {
//     public class DialogueManager : MonoBehaviour
//     {
//         public TextAsset csvFile;
//         public Dictionary<string, List<Dialogue>> dialogues = new Dictionary<string, List<Dialogue>>();

//         void Start() => LoadCSV();

//         void LoadCSV()
//         {
//             string[] data = csvFile.text.Split('\n');

//             for (int i = 1; i < data.Length; i++)
//             {
//                 if (string.IsNullOrWhiteSpace(data[i])) continue;

//                 string[] row = ParseCSVRow(data[i]);
//                 if (row.Length < 3) continue;

//                 Dialogue d = new Dialogue
//                 {
//                     Invariant = row[0].Trim(),
//                     EN = row[1].Trim().Trim('"'),
//                     Character = row[2].Trim()
//                 };

//                 if (d.Character == "DecisionCard" && row.Length >= 8)
//                 {
//                     d.OptionA = row[3].Trim();
//                     d.OptionB = row[4].Trim();
//                     d.NextA = row[5].Trim();
//                     d.NextB = row[6].Trim();

//                     int.TryParse(row[7], out d.InfluenceA);
//                     int.TryParse(row[8], out d.InfluenceB);
//                     int.TryParse(row[9], out d.FoodA);
//                     int.TryParse(row[10], out d.FoodB);
//                     int.TryParse(row[9], out d.GoldA);
//                     int.TryParse(row[10], out d.GoldB);
//                     int.TryParse(row[11], out d.MagicA);
//                     int.TryParse(row[12], out d.MagicB);
//                     int.TryParse(row[13], out d.OpinionA);
//                     int.TryParse(row[14], out d.OpinionB);
//                     // Add more if you use HealthA, EnergyB, etc.
//                 }

//                 if (!dialogues.ContainsKey(d.Invariant))
//                     dialogues[d.Invariant] = new List<Dialogue>();

//                 dialogues[d.Invariant].Add(d);
//             }
//         }

//         string[] ParseCSVRow(string line)
//         {
//             var result = new List<string>();
//             bool inQuotes = false;
//             string current = "";

//             foreach (char c in line)
//             {
//                 if (c == '"')
//                 {
//                     inQuotes = !inQuotes;
//                 }
//                 else if (c == ',' && !inQuotes)
//                 {
//                     result.Add(current);
//                     current = "";
//                 }
//                 else
//                 {
//                     current += c;
//                 }
//             }

//             result.Add(current);
//             return result.ToArray();
//         }
//     }

//     [System.Serializable]
//     public class Dialogue
//     {
//         public string Invariant;
//         public string EN;
//         public string Character;

//         // Optional for decision cards
//         public string OptionA, OptionB;
//         public string NextA, NextB;
//         public int InfluenceA, InfluenceB;
//         public int GoldA, GoldB;

//         public int FoodA, FoodB; // gonna get delete in future as it should not exist but here for testing purposes

//         public int MagicA, MagicB;

//         public int OpinionA, OpinionB;
//         // Add more fields like HealthA, EnergyA, etc., if needed
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

        void Start() => LoadCSV();

        void LoadCSV()
        {
            string[] data = csvFile.text.Split('\n');

            for (int i = 1; i < data.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;

                string[] row = ParseCSVRow(data[i]);
                if (row.Length < 3) continue;

                Dialogue d = new Dialogue
                {
                    Invariant = row[0].Trim(),
                    EN = row[1].Trim().Trim('"'),
                    Character = row[2].Trim()
                };

                if (d.Character == "DecisionCard" && row.Length >= 8)
                {
                    d.OptionA = row[3].Trim();
                    d.OptionB = row[4].Trim();
                    d.NextA = row[5].Trim();
                    d.NextB = row[6].Trim();

                    int.TryParse(row[7], out d.InfluenceA);
                    int.TryParse(row[8], out d.InfluenceB);
                    int.TryParse(row[9], out d.FoodA);
                    int.TryParse(row[10], out d.FoodB);
                    int.TryParse(row[9], out d.GoldA);
                    int.TryParse(row[10], out d.GoldB);
                    int.TryParse(row[11], out d.MagicA);
                    int.TryParse(row[12], out d.MagicB);
                    int.TryParse(row[13], out d.OpinionA);
                    int.TryParse(row[14], out d.OpinionB);
                    // Add more if you use HealthA, EnergyB, etc.
                }

                if (!dialogues.ContainsKey(d.Invariant))
                    dialogues[d.Invariant] = new List<Dialogue>();

                dialogues[d.Invariant].Add(d);
            }
        }

        string[] ParseCSVRow(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            string current = "";

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            result.Add(current);
            return result.ToArray();
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string Invariant;
        public string EN;
        public string Character;

        // Optional for decision cards
        public string OptionA, OptionB;
        public string NextA, NextB;
        public int InfluenceA, InfluenceB;
        public int GoldA, GoldB;

        public int FoodA, FoodB; // gonna get delete in future as it should not exist but here for testing purposes

        public int MagicA, MagicB;

        public int OpinionA, OpinionB;
        // Add more fields like HealthA, EnergyA, etc., if needed
    }
}
