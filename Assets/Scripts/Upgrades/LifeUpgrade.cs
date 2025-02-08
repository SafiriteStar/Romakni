using UnityEngine;

public class LifeUpgrade : UpgradeOnCollision
{
    [SerializeField] private int lifeUpgrade = 2;
    protected override void ApplyUpgrade()
    {
        playerHealthSystem.AddLives(lifeUpgrade);
    }
}
