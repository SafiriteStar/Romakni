using UnityEngine;

public class AttackSpeedUpgrade : UpgradeOnCollision
{
    [SerializeField] private float attackSpeedIncrease = 1f;

    protected override void ApplyUpgrade()
    {
        playerWeapon.SetAttackSpeedModifier(attackSpeedIncrease + playerWeapon.attackSpeedModifier);
    }
}
