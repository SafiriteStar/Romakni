using UnityEngine;

public class ProjectileCountUpgrade : UpgradeOnCollision
{
    [SerializeField] private int projectileCountIncrease = 1;

    protected override void ApplyUpgrade()
    {
        playerWeapon.SetNumberOfAttacksModifier(projectileCountIncrease + playerWeapon.numberOfAttacksModifier);
    }
}
