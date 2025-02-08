using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject attackGO;
    [SerializeField] private int baseDamage;
    [SerializeField] private int baseNumberOfAttacks;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float spawnOffset;
    [SerializeField] private float spreadAngle = 40f;
    public int damageModifier { get; private set; }
    public int numberOfAttacksModifier { get; private set; }
    public float attackSpeedModifier { get; private set; }
    private float attackTimer = 0;

    [SerializeField] Transform[] shootPoints;
    [SerializeField] private bool EightWayPower;
    public void SetDamageModifier(int newModifier) { damageModifier = newModifier; }
    public void SetNumberOfAttacksModifier(int newNumberOfAttacksModifier) { numberOfAttacksModifier = newNumberOfAttacksModifier; }
    public void SetAttackSpeedModifier(float newAttackSpeedModifier) {  attackSpeedModifier = newAttackSpeedModifier; }

    public void AttemptAttack(Vector3 attackDirection)
    {
        if (attackTimer > 0)
        {
            return;
        }

        // We are allowed to attack
        attackTimer = 1.5f;

        Attack(attackDirection);
    }

    protected virtual void Attack(Vector3 attackDirection)
    {
        if (EightWayPower)
        {
            EightWayAttack();
        }
        else
        {
            NormalAttack(attackDirection);
        }
    }

    protected void NormalAttack(Vector3 attackDirection)
    {
        Transform correctShootPoint = shootPoints[0];

        if (attackDirection.z > 0 && attackDirection.x == 0)
        {
            correctShootPoint = shootPoints[0];
        }
        else if (attackDirection.z > 0 && attackDirection.x > 0)
        {
            correctShootPoint = shootPoints[1];
        }
        else if (attackDirection.z == 0 && attackDirection.x > 0)
        {
            correctShootPoint = shootPoints[2];
        }
        else if (attackDirection.z < 0 && attackDirection.x > 0)
        {
            correctShootPoint = shootPoints[3];
        }
        else if (attackDirection.z < 0 && attackDirection.x == 0)
        {
            correctShootPoint = shootPoints[4];
        }
        else if (attackDirection.z < 0 && attackDirection.x < 0)
        {
            correctShootPoint = shootPoints[5];
        }
        else if (attackDirection.z == 0 && attackDirection.x < 0)
        {
            correctShootPoint = shootPoints[6];
        }
        else if (attackDirection.z > 0 && attackDirection.x < 0)
        {
            correctShootPoint = shootPoints[7];
        }

        SpawnAttack(correctShootPoint.position, correctShootPoint.rotation);
    }

    protected void EightWayAttack()
    {
        foreach (Transform shootPoint in shootPoints)
        {
            SpawnAttack(shootPoint.position, shootPoint.rotation);
        }
    }

    private void SpawnAttack(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        int totalAttacks = baseNumberOfAttacks + numberOfAttacksModifier;

        float[] attackAngles = CalculateProjectileAngles(totalAttacks);

        for (int i = 0; i < totalAttacks; i++)
        {
            Quaternion attackRotation = spawnRotation;
            // Remember, we multiply matrices to add!
            attackRotation *= Quaternion.Euler(0, 0, attackAngles[i]);

            GameObject currentAttackGO = Instantiate(attackGO, spawnPosition, attackRotation);
            DamageOnCollision damageData = currentAttackGO.GetComponent<DamageOnCollision>();
            damageData.SetAllDamage(baseDamage, damageModifier);
        }
    }

    // Recieves a number of projectiles and spits out
    // angles to create a fan like shot. Always tries
    // to split things evenly which may make it hard
    // to aim at what you want on even shots.
    private float[] CalculateProjectileAngles(int totalAttacks)
    {

        if (totalAttacks <= 1)
        {
            return new float[1] { 0f };
        }

        float[] spreadAngles = new float[totalAttacks];

        float spreadStep = spreadAngle / (totalAttacks - 1);
        float adjustedAngle = spreadAngle / 2;

        for (int i = 0; i < spreadAngles.Length; i++)
        {
            spreadAngles[i] = -adjustedAngle + (spreadStep * i);
        }

        return spreadAngles;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime * (baseAttackSpeed + attackSpeedModifier);
        }
    }
}
