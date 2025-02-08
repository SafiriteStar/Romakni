using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    private GameObject existingPlayer;
    private HealthSystem existingplayerHealthSystem;

    [SerializeField] private LevelManager[] levels;
    private int levelIndex = 0;

    private EconomyManager economyManager;

    [SerializeField] private GameObject mainMenuGO;
    [SerializeField] private GameObject levelDisplayGO;
    private TextMeshProUGUI levelDisplayTMPro;
    [SerializeField] private GameObject currencyDisplayGO;
    private bool gameStarted = false;

    [SerializeField] private GameObject youWinScreen;
    [SerializeField] private float youWinDisplayTime = 5f;
    private float youWinDisplayTimer = 0f;
    private bool playerWon = false;

    [SerializeField] private GameObject lifeDisplayPrefab;
    [SerializeField] private GameObject lifeDisplayContainer;
    private GameObject[] lifeDisplayGOs;

    public GameObject GetExistingPlayer() {  return existingPlayer; }
    public void LevelComplete()
    {
        // We beat the stage

        // Is there anything next?
        if (levelIndex + 1 >= levels.Length)
        {
            // No more levels!
            Debug.Log("All levels completed!");
            playerWon = true;
            DisplayWinScreen();
        }
        else
        {
            // Lets shop!
            economyManager.SpawnUpgradeGoober();
        }
    }

    public void EnableToNextLevel()
    {
        // We still have levels
        // Disable the current level
        levels[levelIndex].gameObject.SetActive(false);
        // Target the next one
        levelIndex++;
        // Activate it
        ActivateCurrentLevel();
    }

    public void PlayerDied()
    {
        Debug.Log("Player died");
        // Just restart the scene like we did with arcade games
        ReloadScene();
    }

    public void StartGame()
    {
        mainMenuGO.SetActive(false);
        levelDisplayGO.SetActive(true);
        currencyDisplayGO.SetActive(true);
        ActivateCurrentLevel();
        UpdateLifeDisplay();
    }

    public void UpdateLifeDisplay()
    {
        int lives = existingplayerHealthSystem.GetLives();

        for (int i = 0; i < lifeDisplayGOs.Length; i++)
        {
            Destroy(lifeDisplayGOs[i]);
        }

        lifeDisplayGOs = new GameObject[lives];

        for (int i = 0; i < lifeDisplayGOs.Length; i++)
        {
            lifeDisplayGOs[i] = Instantiate(lifeDisplayPrefab, lifeDisplayContainer.transform);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void DisplayWinScreen()
    {
        youWinScreen.SetActive(true);
        youWinDisplayTimer = youWinDisplayTime;
    }

    private void ActivateCurrentLevel()
    {
        levels[levelIndex].gameObject.SetActive(true);
        levels[levelIndex].SetOnCompleteCall(LevelComplete);
        levelDisplayTMPro.text = levels[levelIndex].GetLevelName();
        AstarPath.active.Scan();
    }

    private void Awake()
    {
        // Does the player exist?
        existingPlayer = GameObject.FindGameObjectWithTag("Player");

        // No
        if (existingPlayer == null)
        {
            // Create the player
            existingPlayer = Instantiate(playerPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
        }


        economyManager = GetComponent<EconomyManager>();
        levelDisplayTMPro = levelDisplayGO.GetComponent<TextMeshProUGUI>();

        existingplayerHealthSystem = existingPlayer.GetComponent<HealthSystem>();
        lifeDisplayGOs = new GameObject[0];
    }

    private void Start()
    {
        existingplayerHealthSystem.HealthChanged.AddListener(UpdateLifeDisplay);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWon)
        {
            if (youWinDisplayTimer > 0)
            {
                youWinDisplayTimer -= Time.deltaTime;
            }
            else
            {
                ReloadScene();
            }
        }

        if (!gameStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }
    }
}
