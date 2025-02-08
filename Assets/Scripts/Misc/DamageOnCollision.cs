using UnityEngine;

public class DamageOnCollision : MonoBehaviour
{
    [SerializeField] string[] damageTargets;
    [SerializeField] int damage;
    private int damageModifier;
    public void SetAllDamage(int baseDamage, int damageModifier) { damage = baseDamage; this.damageModifier = damageModifier; }

    public void SetDamageModifier(int damageModifier) {  this.damageModifier = damageModifier; }

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
        foreach (string tag in damageTargets)
        {
            if (collision.CompareTag(tag))
            {
                collision.GetComponent<HealthSystem>().TakeDamage(damage + damageModifier);
            }
        }
    }
}
