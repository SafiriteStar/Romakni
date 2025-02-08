using UnityEngine;

public class PlayerController : CharacterController
{
    [SerializeField] private bool isInShop;
    [SerializeField] private float shopSpeed;
    [SerializeField] private float moneyCollectMultiplier = 1;

    public float GetMoneyMultiplier() {  return moneyCollectMultiplier; }
    public void SetShop(bool shop) { isInShop = shop; }

    protected override float CalculateSpeed()
    {
        if (isInShop)
        {
            return base.CalculateSpeed() + shopSpeed;
        }
        else return base.CalculateSpeed();
    }
}
