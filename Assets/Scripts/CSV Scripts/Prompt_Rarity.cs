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
        string[] data = Prompts.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);


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

    void Start()
    {
        ReadCSV();
    }
}
