using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private string playerTag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            // Player picked up money
            GameObject.FindGameObjectWithTag("GameController").GetComponent<EconomyManager>().CollectMoney(value);
        }
    }
}
