using UnityEngine;

public class SkipUpgrades : MonoBehaviour
{
    EconomyManager economyManager;

    private bool playerIsHoveringOver = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHoveringOver = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHoveringOver = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        economyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EconomyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsHoveringOver && Input.GetKeyDown(KeyCode.E))
        {
            // Player wants to skip shop
            economyManager.CloseShop();
        }

    }
}
