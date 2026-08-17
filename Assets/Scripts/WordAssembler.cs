using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordAssembler : MonoBehaviour
{
    [SerializeField] GameObject LetterPrefab;
    [SerializeField] GameObject Parent;
    List<GameObject> Word = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnLetter(char c)
    {
        GameObject p=Instantiate(LetterPrefab);
        p.GetComponent<TextMeshProUGUI>().text=c.ToString();
        p.transform.SetParent(Parent.transform);
        Word.Add(p);
    }
    public void Erase()
    {
        foreach (GameObject p in Word) {
            GameObject.Destroy(p);
        }
    }
}
