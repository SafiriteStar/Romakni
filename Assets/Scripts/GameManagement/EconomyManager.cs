using UnityEngine;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class UpgradeData
{
    public GameObject UpgradePrefab;
    public Transform spawnPoint;
    public int count;
    public int max;
}

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private int totalMoney = 0;
    [SerializeField] private TextMeshProUGUI moneyTMPro;

    [SerializeField] private GameObject upgradeGooberPrefab;
    [SerializeField] private Transform upgradeGooberSpawnPoint;
    [SerializeField] private Transform upgradeGooberShopPoint;
    private GameObject currentUpgradeGoober;
    private MoveToPoint currentGooberMoveToPoint;

    [SerializeField] private UpgradeData[] upgrades;
    private GameObject[] spawnedUpgrades;

    private GameManager gameManager;

    public void ChangeMoneyAmount(int changeAmount)
    {
        totalMoney += Mathf.RoundToInt(changeAmount * gameManager.GetPlayerController().GetMoneyMultiplier());

        UpdateMoneyUI();
    }

    public bool AttemptPurchase(int cost)
    {
        if (totalMoney >= cost)
        {
            ChangeMoneyAmount(-cost);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnUpgradeGoober()
    {
        if (currentUpgradeGoober == null)
        {
            currentUpgradeGoober = Instantiate(upgradeGooberPrefab, upgradeGooberSpawnPoint.position, Quaternion.identity);
        }

        currentUpgradeGoober.SetActive(true);
        currentUpgradeGoober.transform.position = upgradeGooberSpawnPoint.position;

        currentGooberMoveToPoint = currentUpgradeGoober.GetComponent<MoveToPoint>();

        currentGooberMoveToPoint.SetDestination(upgradeGooberShopPoint.position);
        // Don't want to immediately go to next level!
        currentGooberMoveToPoint.ReachedPoint.RemoveListener(gameManager.EnableToNextLevel);
        currentGooberMoveToPoint.ReachedPoint.AddListener(SpawnShop);

        gameManager.GetExistingPlayer().GetComponent<PlayerController>().SetShop(true);
    }

    public void SpawnShop()
    {
        // Don't want to spawn things twice!
        currentGooberMoveToPoint.ReachedPoint.RemoveListener(SpawnShop);

        spawnedUpgrades = new GameObject[upgrades.Length];

        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].count <= upgrades[i].max)
            {
                spawnedUpgrades[i] = Instantiate(upgrades[i].UpgradePrefab, upgrades[i].spawnPoint.position, Quaternion.identity);
                UpgradeOnCollision upgradeOnCollision = spawnedUpgrades[i].GetComponent<UpgradeOnCollision>();
                upgradeOnCollision.SetEconomyManager(this);
                upgradeOnCollision.SetCount(upgrades[i].count);
                upgradeOnCollision.SetEconomyIndex(i);
            }
        }
    }

    public void CloseShop()
    {
        foreach (GameObject spawnedUpgrade in spawnedUpgrades)
        {
            Destroy(spawnedUpgrade);
        }

        DespawnGoober();

        gameManager.GetExistingPlayer().GetComponent<PlayerController>().SetShop(false);
    }

    public void DespawnGoober()
    {
        currentGooberMoveToPoint = currentUpgradeGoober.GetComponent<MoveToPoint>();

        currentGooberMoveToPoint.SetDestination(upgradeGooberSpawnPoint.position);
        currentGooberMoveToPoint.ReachedPoint.AddListener(gameManager.EnableToNextLevel);
    }

    public void IncrementUpgradeCount(int upgradeIndex)
    {
        upgrades[upgradeIndex].count++;
    }

    private void UpdateMoneyUI()
    {
        moneyTMPro.text = "€" + totalMoney.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        UpdateMoneyUI();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
