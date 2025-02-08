using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterController playerController;
    private Weapon weapon;
    private Vector2 moveDirection;
    private Vector2 attackDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        weapon = GetComponent<Weapon>();
        moveDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();

        attackDirection.x = Input.GetAxisRaw("HorizontalAttack");
        attackDirection.y = Input.GetAxisRaw("VerticalAttack");

        playerController.SetMove(moveDirection);
        
        // If we are trying to attack
        if (attackDirection.magnitude > 0)
        {
            weapon.AttemptAttack(new Vector3(attackDirection.x, 0, attackDirection.y));
        }
    }
}
