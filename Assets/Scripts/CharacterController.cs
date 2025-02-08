using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] protected float speed = 2;
    public float speedModifier { get; private set; }

    protected Vector2 moveVector;
    protected Rigidbody2D rb;
    
    private bool isAllowedToMove = true;
    
    public void SetMove(Vector2 direction)
    {
        this.moveVector = direction * (speed + speedModifier);
    }

    public void ChangeSpeedModifier(float speedChange)
    {
        speedModifier += speedChange;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isAllowedToMove)
        {
            Move();
        }
    }

    private void Move()
    {
        rb.linearVelocity = moveVector;
    }
}
