using UnityEngine;

public class MovementUpgrade : UpgradeOnCollision
{
    [SerializeField] private float speedIncrement = 0.5f;
    protected override void ApplyUpgrade()
    {
        playerController.ChangeSpeedModifier(playerController.speedModifier + speedIncrement);
    }
}
