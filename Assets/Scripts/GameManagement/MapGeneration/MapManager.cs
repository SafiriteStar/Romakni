using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellularAutomataGeneration))]
[RequireComponent(typeof(DepthFirstSearch))]
[RequireComponent(typeof(RandomWalk))]
[RequireComponent(typeof(Cyclical))]
[RequireComponent(typeof(RandomPlacement))]
public class MapManager : MonoBehaviour
{
    public List<MapCell> mapData { get; private set; }

    [Tooltip("Includes Start and End")]
    [SerializeField] protected int maxLevels = 10;

    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject[] regularLevelPrefab;
    [SerializeField] private GameObject endPrefab;
    
    [SerializeField] private MapGenerationType targetGenerationType;
    private CellularAutomataGeneration cellularAutomataGeneration;
    private DepthFirstSearch depthFirstSearch;
    private RandomWalk randomWalk;
    private Cyclical cyclical;
    private RandomPlacement randomPlacement;

    public List<MapCell> GenerateMapData()
    {
        Debug.Log("Generating Map Data...");
        // Choose PCG algorithm here

        switch(targetGenerationType)
        {
            case MapGenerationType.None:
                Debug.Log("None selected. Change to random later...");
                break;
            case MapGenerationType.CellularAutomata:
                Debug.Log("Generating map using cellular automata...");
                mapData = cellularAutomataGeneration.GenerateMapData(maxLevels);
                break;
            case MapGenerationType.DepthFirstSearch:
                Debug.Log("Generating map using DFS");
                mapData = depthFirstSearch.GenerateMapData(maxLevels);
                break;
            case MapGenerationType.RandomWalk:
                Debug.Log("Generating map using Random Walk");
                mapData = randomWalk.GenerateMapData(maxLevels);
                break;
            case MapGenerationType.Cyclical:
                Debug.Log("Generating map using cyclical algorithm");
                mapData = cyclical.GenerateMapData(maxLevels);
                break;
            case MapGenerationType.RandomPlacement:
                Debug.Log("Generating map using random placement");
                mapData = randomPlacement.GenerateMapData(maxLevels);
                break;
            default:
                Debug.LogError("Map generation setting not found!");
                break;
        }

        // Return generated data
        return mapData;
    }

    private GameObject GetRandomLevelPrefab()
    {
        return regularLevelPrefab[Random.Range(0, regularLevelPrefab.Length)];
    }

    private void Awake()
    {
        cellularAutomataGeneration = GetComponent<CellularAutomataGeneration>();
        depthFirstSearch = GetComponent<DepthFirstSearch>();
        randomWalk = GetComponent<RandomWalk>();
        cyclical = GetComponent<Cyclical>();
        randomPlacement = GetComponent<RandomPlacement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMapData();
        foreach (MapCell mapCell in mapData)
        {
            mapCell.Print();

            GameObject targetPrefab = GetRandomLevelPrefab();
            
            if (mapCell.cellType == CellType.Start)
            {
                targetPrefab = startPrefab;
            }
            else if (mapCell.cellType == CellType.End)
            {
                targetPrefab = endPrefab;
            }

            GameObject currentLevel = Instantiate(targetPrefab, new Vector3(mapCell.x * 25, mapCell.y * 17, 0), Quaternion.identity);

            //currentLevel.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
