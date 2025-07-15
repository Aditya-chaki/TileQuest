using System;
using System.Collections.Generic;
using UnityEngine;

public class MetricLoader : MonoBehaviour
{
    private const string CsvResourcePath = "Metric Sheet"; // Path to the CSV file in the Resources folder (without the extension)

    void Start()
    {
        LoadMetricsFromCsv();
    }

    private void LoadMetricsFromCsv()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(CsvResourcePath);
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found in Resources folder.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        if (lines.Length == 0)
        {
            Debug.LogError("CSV file is empty.");
            return;
        }

        for (int i = 1; i < lines.Length; i++) // Start from 1 to skip the header
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');
            if (values.Length < 2)
            {
                Debug.LogWarning($"Invalid line in CSV: {line}");
                continue;
            }

            var metricName = values[0].Trim();
            if (!int.TryParse(values[1].Trim(), out var metricValue))
            {
                Debug.LogWarning($"Invalid metric value in CSV: {line}");
                continue;
            }

            SetMetric(metricName, metricValue);
        }
    }

    private void SetMetric(string metricName, int metricValue)
    {
        switch (metricName.ToLower())
        {
            case "influence":
                Config.Influence = metricValue;
                break;
            case "gold":
                Config.Gold = metricValue;
                break;
            case "magic":
                Config.Magic = metricValue;
                break;
            // If you want to set opinion, you need to specify the faction name,
            // e.g., "opinion_nobles", "opinion_peasants", etc.
            // case "opinion_nobles":
            //     Config.SetFactionOpinion("nobles", metricValue);
            //     break;
            // Add more cases for specific faction opinions if needed.
            default:
                Debug.LogWarning($"Unknown metric: {metricName}");
                break;
        }
    }
}