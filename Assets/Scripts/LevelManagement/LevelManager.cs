using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string LevelName = "TestLevel";
    [SerializeField] private Transform[] spawnPoints;

    // Where are things spawning from?
    [SerializeField] private WaveData[] waves;
    private Queue<EnemySpawnData> enemySpawnQueue;
    int liveEnemies = 0;

    private float spawnTimer;

    public delegate void OnCompleteCall();
    private OnCompleteCall callOnComplete;
    public void SetOnCompleteCall(OnCompleteCall onCompleteCall)
    {
        callOnComplete = onCompleteCall;
    }

    public string GetLevelName() { return LevelName; }

    public void AddEnemyToQueue(EnemySpawnData newEnemy)
    {
        enemySpawnQueue.Enqueue(newEnemy);
    }

    public void EnemyDied()
    {
        liveEnemies--;

        if (enemySpawnQueue.Count <= 0 && liveEnemies <= 0)
        {
            // There are no enemies left
            // We beat the stage!

            if (callOnComplete != null)
            {
                callOnComplete();
            }
            else
            {
                Debug.LogError("Nothing to call on level complete!");
            }
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySpawnQueue = new Queue<EnemySpawnData>();

        for (int i = 0; i < waves.Length; i++)
        {
            for (int j = 0; j < waves[i].enemiesToSpawn.Length; j++)
            {
                AddEnemyToQueue(waves[i].enemiesToSpawn[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        spawnTimer -= Time.deltaTime;

        while (spawnTimer <= 0 && enemySpawnQueue.Count > 0)
        {
            EnemySpawnData currentEnemyData = enemySpawnQueue.Dequeue();

            spawnTimer = currentEnemyData.spawnTime;

            // Do we want to spawn at any specific place?
            if (currentEnemyData.spawnPointIndices.Length > 0)
            {
                // Yes. For each place
                for (int i = 0; i < currentEnemyData.spawnPointIndices.Length; i++)
                {
                    // Spawn in it
                    SpawnEnemy(currentEnemyData.enemyGO, spawnPoints[currentEnemyData.spawnPointIndices[i]].position, Quaternion.identity);
                }
            }
            else
            {
                // No. Just spawn everywhere
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    SpawnEnemy(currentEnemyData.enemyGO, spawnPoints[i].position, Quaternion.identity);
                }
            }

            if (enemySpawnQueue.Count == 0)
            {
                Debug.Log("Finished spawning everything...");
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint, rotation);
        HealthSystem healthSystem = enemyGO.GetComponent<HealthSystem>();
        healthSystem.SetOnDeathCall(EnemyDied);

        liveEnemies++;
    }
}
