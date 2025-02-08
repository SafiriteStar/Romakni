using UnityEngine;

public class CallPlayerDeath : MonoBehaviour
{
    private GameManager gameManager;

    public void OnDeath()
    {
        gameManager.PlayerDied();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
}
