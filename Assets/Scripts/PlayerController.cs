using UnityEngine;

public class PlayerController : CharacterController
{
    [SerializeField] private bool isInShop;
    [SerializeField] private float shopSpeed;

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
