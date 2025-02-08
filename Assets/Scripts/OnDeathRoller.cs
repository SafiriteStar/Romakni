using UnityEngine;

[System.Serializable]
public class DeathRollData
{
    public GameObject ObjectToSpawn;
    public float weight;
}

public class OnDeathRoller : MonoBehaviour
{
    [SerializeField] private DeathRollData[] deathRollObjects;

    private void SpawnRandomObject()
    {
        float totalWeights = 0;

        for (int i = 0; i < deathRollObjects.Length; i++)
        {
            totalWeights += deathRollObjects[i].weight;
        }

        float targetObjectIndex = Random.Range(0, totalWeights);

        float currentWeightThreshold = 0;

        for (int i = 0; i < deathRollObjects.Length; i++)
        {
            currentWeightThreshold += deathRollObjects[i].weight;

            if (currentWeightThreshold > targetObjectIndex)
            {
                if (deathRollObjects[i].ObjectToSpawn != null)
                {
                    Instantiate(deathRollObjects[i].ObjectToSpawn, transform.position, Quaternion.identity);
                }

                break;
            }
        }

        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnRandomObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
