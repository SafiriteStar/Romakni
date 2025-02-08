using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UpgradeOnCollision : MonoBehaviour
{
    [SerializeField] private int baseCost = 1;

    protected CharacterController playerController;
    protected Weapon playerWeapon;
    protected HealthSystem playerHealthSystem;
    protected EconomyManager economyManager;

    private bool playerIsOverUpgrade = false;
    private int count = 0;
    private int economyIndex;

    [SerializeField] private TextMeshPro explanationTMPro;
    [SerializeField] private string explanationText;
    [SerializeField] private TextMeshPro costTMPro;

    public void SetEconomyManager(EconomyManager economyManager) { this.economyManager = economyManager; }
    public void SetCount(int count) { this.count = count; UpdateCostText(); }

    public void SetEconomyIndex(int index) { economyIndex = index; }

    protected virtual void ApplyUpgrade()
    {
        Debug.Log("Empty Upgrade");
    }

    private int GetCurrentCost() { return baseCost * (count + 1); }

    private void UpdateCostText()
    {
        costTMPro.text = "€" + GetCurrentCost().ToString();
    }

    private void AttemptUpgrade()
    {
        if (economyManager.AttemptPurchase(GetCurrentCost()))
        {
            // We can buy the upgrade!
            ApplyUpgrade();
            economyManager.IncrementUpgradeCount(economyIndex);
            economyManager.CloseShop();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        playerController = playerGO.GetComponent<CharacterController>();
        playerWeapon = playerGO.GetComponent<Weapon>();
        playerHealthSystem = playerGO.GetComponent<HealthSystem>();

        explanationTMPro.text = explanationText;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverUpgrade && Input.GetKeyDown(KeyCode.E))
        {
            AttemptUpgrade();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsOverUpgrade = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsOverUpgrade = false;
        }
    }
}
