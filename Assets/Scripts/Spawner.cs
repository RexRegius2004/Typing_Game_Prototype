using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 20;

    [Header("Spawn Area")]
    [SerializeField] private float spawnWidth = 5f;
    [SerializeField] private float spawnHeight = 2f;

    [Header("Force")]
    [SerializeField] private float minForce = 3f;
    [SerializeField] private float maxForce = 7f;
    [SerializeField] private float horizontalForce = 2f;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateObject();
        }
    }
    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, 1f);
    }
    private GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public void Spawn()
    {
        if (pool.Count == 0)
            CreateObject();

        GameObject obj = pool.Dequeue();

        // Random position inside rectangle
        float x = Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
        float y = Random.Range(-spawnHeight / 2f, spawnHeight / 2f);

        obj.transform.position = transform.position + new Vector3(x, y, 0f);
        obj.transform.rotation = Quaternion.identity;

        obj.SetActive(true);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        float upwardForce = Random.Range(minForce, maxForce);
        float sidewaysForce = Random.Range(-horizontalForce, horizontalForce);

        rb.AddForce(
            new Vector2(sidewaysForce, upwardForce),
            ForceMode2D.Impulse
        );

        StartCoroutine(ReturnToPool(obj, 2f));
    }

    private System.Collections.IEnumerator ReturnToPool(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.2f, 0.7f);
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(spawnWidth, spawnHeight, 0f)    
        );
    }
}