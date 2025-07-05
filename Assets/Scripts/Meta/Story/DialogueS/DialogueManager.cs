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
            Debug.Log("=== LOADING CSV ===");
            string[] data = csvFile.text.Split('\n');
            Debug.Log($"CSV has {data.Length} lines");

            for (int i = 1; i < data.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;

                string[] row = ParseCSVRow(data[i]);
                Debug.Log($"Row {i}: {row.Length} columns");
                
                if (row.Length < 3) continue;

                Dialogue d = new Dialogue
                {
                    Invariant = row[0].Trim(),
                    EN = row[1].Trim().Trim('"'),
                    Character = row[2].Trim()
                };

                // Parse decision card data
                if (d.Character == "DecisionCard" && row.Length >= 8)
                {
                    d.OptionA = row[3].Trim();
                    d.OptionB = row[4].Trim();
                    d.NextA = row[5].Trim();
                    d.NextB = row[6].Trim();

                    if (row.Length > 14) d.PortraitName = row[14].Trim();
                }

                // Debug: Print the last few columns to see where NextInvariant actually is
                if (row.Length > 13)
                {
                    Debug.Log($"Row {i} last columns: [14]='{row[14]}' [15]='{row[15]}' " + 
                             (row.Length > 16 ? $"[16]='{row[16]}'" : "[16]=MISSING"));
                }

                // Try multiple possible column positions for NextInvariant
                string nextInvariant = "";
                
                // Check column 16 (index 15) first
                if (row.Length > 15)
                {
                    nextInvariant = row[15].Trim().Replace("\r", "").Replace("\n", "");
                    if (!string.IsNullOrEmpty(nextInvariant) && nextInvariant.StartsWith("tut"))
                    {
                        d.NextInvariant = nextInvariant;
                        Debug.Log($"*** FOUND NextInvariant at column 16: '{d.Invariant}' -> '{nextInvariant}' ***");
                    }
                }
                
                // If not found, check column 17 (index 16)
                if (string.IsNullOrEmpty(d.NextInvariant) && row.Length > 16)
                {
                    nextInvariant = row[16].Trim().Replace("\r", "").Replace("\n", "");
                    if (!string.IsNullOrEmpty(nextInvariant) && nextInvariant.StartsWith("tut"))
                    {
                        d.NextInvariant = nextInvariant;
                        Debug.Log($"*** FOUND NextInvariant at column 17: '{d.Invariant}' -> '{nextInvariant}' ***");
                    }
                }

                if (!dialogues.ContainsKey(d.Invariant))
                    dialogues[d.Invariant] = new List<Dialogue>();

                dialogues[d.Invariant].Add(d);
            }

            Debug.Log($"=== CSV LOADING COMPLETE ===");
            Debug.Log($"Loaded {dialogues.Count} story blocks");
            
            // Check all NextInvariant values found
            Debug.Log($"=== ALL NEXTINVARIANT VALUES FOUND ===");
            foreach (var storyBlock in dialogues)
            {
                foreach (var line in storyBlock.Value)
                {
                    if (!string.IsNullOrEmpty(line.NextInvariant))
                    {
                        Debug.Log($"*** {storyBlock.Key}: '{line.EN}' -> '{line.NextInvariant}' ***");
                    }
                }
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
                else if (c != '\r' && c != '\n') // Skip carriage return and newline characters
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
        public string OptionA, OptionB;
        public string NextA, NextB;
        public int InfluenceA, InfluenceB;
        public int GoldA, GoldB;
        public int FoodA, FoodB;
        public int MagicA, MagicB;
        public int OpinionA, OpinionB;
        public string PortraitName;
        public string NextInvariant;
    }
}