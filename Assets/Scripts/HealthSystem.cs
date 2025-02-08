using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] protected int lives = 1;
    [SerializeField] protected bool canTakeDamage = true;
    [SerializeField] protected float invincibilityTime = 1;
    protected float invincibilityTimer = 0;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private float flashSpeed;
    private float flashTimer = 0;
    [SerializeField] private Color flashColor;
    private Color baseColor;

    [SerializeField] private GameObject deathEffect;

    public delegate void OnDeathCall();
    private OnDeathCall callOnDeath;

    public UnityEvent HealthChanged {  get; private set; }

    public void SetOnDeathCall(OnDeathCall onDeathCall)
    {
        callOnDeath = onDeathCall;
    }
    

    public int GetLives() { return lives; }

    public void AddLives(int livesAmount)
    {
        lives += livesAmount;
        HealthChanged.Invoke();
    }

    public void SetCanTakeDamage(bool canTakeDamage) { this.canTakeDamage = canTakeDamage; }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage)
        {
            return;
        }

        // Are we invincibile and not dead?
        if (invincibilityTimer <= 0 && lives > 0)
        {
            // No!
            // Lose life
            lives -= damage;
            // Set the invincibility timer
            invincibilityTimer = invincibilityTime;
            spriteRenderer.color = flashColor;

            if (lives <= 0)
            {
                // We died!
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, transform.position, Quaternion.identity);
                }

                if (callOnDeath != null)
                {
                    callOnDeath();
                }

                Destroy(this.gameObject);
            }
        }

        HealthChanged.Invoke();
    }

    private void Awake()
    {
        if (HealthChanged == null)
            HealthChanged = new UnityEvent();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Count down the invincibility
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;

            // WE NEED TO FLASH
            // Is it time to switch?
            if (flashTimer <= 0)
            {
                // YES
                flashTimer = flashSpeed;

                if (spriteRenderer.color == baseColor)
                {
                    spriteRenderer.color = flashColor;
                }
                else
                {
                    spriteRenderer.color = baseColor;
                }
            }
            else
            {
                flashTimer -= Time.deltaTime;
            }
        }
        else
        {
            // Make sure we have the right color when we stop flashing
            spriteRenderer.color = baseColor;
        }
    }
}
