using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] classPickerPrefabs;
    private ClassChoice[] classChoices;
    [SerializeField] private GameObject classPickerContainer;
    [SerializeField] private int currentClassChoiceIndex;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    private GameObject existingPlayer;
    private PlayerController existingPlayerController;
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
    public PlayerController GetPlayerController() { return existingPlayerController; }

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
        classPickerContainer.SetActive(false);

        // Get our class choice and set it
        playerPrefab = classChoices[currentClassChoiceIndex].GetTargetClass();
        CreatePlayer();

        ActivateCurrentLevel();
        UpdateLifeDisplay();
        gameStarted = true;
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

    private void CreatePlayer()
    {
        // Does the player exist?
        existingPlayer = GameObject.FindGameObjectWithTag("Player");

        // No
        if (existingPlayer == null)
        {
            // Create the player
            existingPlayer = Instantiate(playerPrefab, playerSpawnPoint.transform.position, Quaternion.identity);
        }

        existingplayerHealthSystem = existingPlayer.GetComponent<HealthSystem>();
        existingplayerHealthSystem.HealthChanged.AddListener(UpdateLifeDisplay);
        lifeDisplayGOs = new GameObject[0];

        existingPlayerController = existingPlayer.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        economyManager = GetComponent<EconomyManager>();
        levelDisplayTMPro = levelDisplayGO.GetComponent<TextMeshProUGUI>();
    }

    private void SetUpClassChoices()
    {
        classChoices = new ClassChoice[classPickerPrefabs.Length];

        for (int i = 0; i < classPickerPrefabs.Length; i++)
        {
            GameObject classChoiceGO = Instantiate(classPickerPrefabs[i], classPickerContainer.transform);

            classChoices[i] = classChoiceGO.GetComponent<ClassChoice>();
        }

        UpdateClassChoiceDisplay();
    }

    private void UpdateClassChoiceDisplay()
    {
        for (int i = 0; i < classChoices.Length; i++)
        {
            classChoices[i].SetSelected(i == currentClassChoiceIndex);
        }
    }

    private void MoveClassIndicator(int direction)
    {
        int newIndex = currentClassChoiceIndex + direction;
        if (newIndex > -1 && newIndex < classChoices.Length)
        {
            currentClassChoiceIndex = newIndex;
            UpdateClassChoiceDisplay();
        }
    }

    private void MenuInputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveClassIndicator(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveClassIndicator(1);
        }
    }

    private void Start()
    {
        SetUpClassChoices();
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

        if (!gameStarted)
        {
            MenuInputs();
        }

    }
}
