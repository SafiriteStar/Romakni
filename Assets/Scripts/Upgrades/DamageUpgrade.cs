using UnityEngine;

public class DamageUpgrade : UpgradeOnCollision
{
    [SerializeField] private int damageIncrease = 1;

    protected override void ApplyUpgrade()
    {
        playerWeapon.SetDamageModifier(playerWeapon.damageModifier + damageIncrease);
    }
}
