using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
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
        p.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBounce);
        Word.Add(p);
    }
    public void SpawnCriticalLetter(char c)
    {
        GameObject p = Instantiate(LetterPrefab);
        TextMeshProUGUI text = p.GetComponent<TextMeshProUGUI>();
        text.text=c.ToString();
        text.color = Color.gold;
      
        p.transform.SetParent(Parent.transform);
        p.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutElastic);
        Word.Add(p);
    }
    public void Erase()
    {
        foreach (GameObject p in Word) {
            GameObject.Destroy(p);
        }
    }
}
