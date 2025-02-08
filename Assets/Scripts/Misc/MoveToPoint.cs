using UnityEngine;
using UnityEngine.Events;

public class MoveToPoint : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 destination;
    private bool canMove = true;

    public UnityEvent ReachedPoint {  get; private set; }

    private Rigidbody2D rb;

    public void SetDestination(Vector3 newDestination) { destination = newDestination; canMove = true; }
    
    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        // Have we arrived?
        if (Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            // Yes
            ReachedPoint.Invoke();
            canMove = false;
            rb.linearVelocity = Vector3.zero;
        }
        else if (canMove)
        {
            Vector3 directionToTarget = (destination - transform.position).normalized;

            rb.linearVelocity = directionToTarget * speed;
        }

    }

    private void Awake()
    {
        if (ReachedPoint == null)
            ReachedPoint = new UnityEvent();
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
}
