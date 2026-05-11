using UnityEngine;

public class Prompt_Rarity : MonoBehaviour
{
      [Header("Importing CSV to Unity")]
    public TextAsset Prompts;
    
    [System.Serializable]
    public class PromptRarity //Array
    {
        public string PromptRarityLabel = "Prompt Rarity";
        public string Common;
        public string Uncommon;
        public string Rare;
        public string Epic;
        public string Legendary;
    }

[System.Serializable]
    public class PromptList
    {
        public PromptRarity[] PromptRarity;
    }   

     public PromptList promptList = new PromptList();
    void ReadCSV()
    {
        const int columnsPerRow = 5;
        string[] data = Prompts.text.Split(new string[] { "", "\n" }, System.StringSplitOptions.None);


        int tableSize = data.Length / columnsPerRow - 1;
        if (tableSize < 0 || data.Length % columnsPerRow != 0)
        {
            Debug.LogWarning(
                $"CSV_Importer: expected a multiple of {columnsPerRow} cells (header + full rows). Length was {data.Length}.");
            tableSize = System.Math.Max(0, tableSize);
        }

        promptList.PromptRarity = new PromptRarity[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int rowStart = columnsPerRow * (i + 1);
            promptList.PromptRarity[i] = new PromptRarity();
            promptList.PromptRarity[i].Common = data[rowStart + 0].Trim();
            promptList.PromptRarity[i].Uncommon = data[rowStart + 1].Trim();
            promptList.PromptRarity[i].Rare = data[rowStart + 2].Trim();
            promptList.PromptRarity[i].Epic = data[rowStart + 3].Trim();
            promptList.PromptRarity[i].Legendary = data[rowStart + 4].Trim();
        }
    }

    void ReadCSV_Parser()
    {
        if (Prompts == null)
        {
            Debug.LogError("CSV file reference is missing!");
            return;
        }

        string csvText = Prompts.text;
        var lines = SplitLines(csvText);
         if (lines.Count <= 1)
    {
        Debug.LogWarning("Prompt_Rarity: CSV has no data rows.");
        promptList.PromptRarity = new PromptRarity[0];
        return;
    }
    // Skip header row (line 0). Everything after that is data.
    int tableSize = lines.Count - 1;
    promptList.PromptRarity = new PromptRarity[tableSize];
    for (int i = 0; i < tableSize; i++)
    {
        string line = lines[i + 1];
        var fields = ParseCsvLine(line);
        if (fields.Count < 5)
        {
            Debug.LogWarning($"Prompt_Rarity: Row {i + 2} has fewer than 5 columns. Skipping.");
            continue;
        }
        var row = new PromptRarity();
        row.Common = fields[0].Trim();
        row.Uncommon = fields[1].Trim();
        row.Rare = fields[2].Trim();
        row.Epic = fields[3].Trim();
        row.Legendary = fields[4].Trim();
        promptList.PromptRarity[i] = row;
    }
}
private System.Collections.Generic.List<string> SplitLines(string text)
{
    var lines = new System.Collections.Generic.List<string>();
    var raw = text.Split(new[] { "\r\n", "\n", "\r" }, System.StringSplitOptions.None);
    foreach (var line in raw)
    {
        // Keep only non-empty lines (helps avoid trailing blank row issues)
        if (!string.IsNullOrWhiteSpace(line))
            lines.Add(line);
    }
    return lines;
}
private System.Collections.Generic.List<string> ParseCsvLine(string line)
{
    var fields = new System.Collections.Generic.List<string>();
    var current = new System.Text.StringBuilder();
    bool inQuotes = false;
    for (int i = 0; i < line.Length; i++)
    {
        char c = line[i];
        if (c == '\"')
        {
            // Escaped quote inside quoted field: ""
            if (inQuotes && i + 1 < line.Length && line[i + 1] == '\"')
            {
                current.Append('\"');
                i++; // skip the second quote
            }
            else
            {
                inQuotes = !inQuotes;
            }
        }
        else if (c == ',' && !inQuotes)
        {
            fields.Add(current.ToString());
            current.Length = 0;
        }
        else
        {
            current.Append(c);
        }
    }
    // last field
    fields.Add(current.ToString());
    return fields;
}
    

    // void Start()
    // {
    //     ReadCSV_Parser();
    // }

    void Awake()
    {
        ReadCSV_Parser();
    }
}
