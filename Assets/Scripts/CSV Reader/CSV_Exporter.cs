using UnityEngine;
using System.IO;


public class CSV_Exporter : MonoBehaviour
{
    [Header("Exporting CSV from Unity")]
    string filename = " ";

    [System.Serializable]
    public class PromptRarity
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        filename = Application.dataPath + "/Prompts.csv";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
