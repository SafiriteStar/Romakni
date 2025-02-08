using UnityEngine;

public class AIInput : MonoBehaviour
{
    private CharacterController aiController;
    private Vector2 moveDirection;
    private Transform playerGO;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aiController = GetComponent<CharacterController>();
        moveDirection = Vector2.zero;
        playerGO = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        CalculateMoveToPlayer();
        aiController.SetMove(moveDirection);
    }

    private void CalculateMoveToPlayer()
    {
        if (playerGO != null)
        {
            moveDirection = (playerGO.position - transform.position).normalized;
        }
    }
}
