using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cyclical : MapGenerator
{
    RandomWalk randomWalk;

    public override List<MapCell> GenerateMapData(int maxLevels)
    {
        randomWalk = GetComponent<RandomWalk>();

        // Random Walk For 2 steps
        generatedMap = randomWalk.GenerateMapData(-1);

        int randomFirstTwo = Random.Range(0, 2);

        MapCell thirdAdditionRoot = generatedMap.ElementAt(1 + randomFirstTwo);

        Vector2Int thirdAdditionPosition = new Vector2Int(thirdAdditionRoot.x, thirdAdditionRoot.y);

        bool validBranchDirection = false;
        
        while (!validBranchDirection)
        {
            // Get a direction
            Vector2Int randomDirection = GetRandomDirection();
            Vector2Int branchDirection = thirdAdditionPosition + randomDirection;

            // Is there something here?
            if (GetLevelByCoord(branchDirection.x, branchDirection.y) == null)
            {
                // No its valid!
                generatedMap.Add(CreateNewLevel(branchDirection.x, branchDirection.y, generatedMap.Count + 2));
                validBranchDirection = true;
            }
        }

        GenerateNeighbours(1); // Bulk things out
        GenerateNeighbours(2); // Remove corners

        AddEndToFarthestWorldLevel();

        PruneEmptyAndVoid();
        RecalculateGridNeighbours();
        return generatedMap;
    }

    private bool IsLevelInList(List<MapCell> levelList, int x, int y)
    {
        foreach (MapCell level in levelList)
        {
            if (level.x == x && level.y == y)
            {
                return true;
            }
        }

        return false;
    }

    private void GenerateNeighbours(int minConnectionCount)
    {
        List<MapCell> neighboursToAdd = new List<MapCell>();

        foreach (MapCell level in generatedMap)
        {
            // Check each cardinal direction

            for (int i = 0; i < cardinalDirections.Length; i++)
            {
                Vector2Int neighbourPosition = new Vector2Int(level.x + cardinalDirections[i].x, level.y + cardinalDirections[i].y);

                // Is a level already here?
                if (GetLevelByCoord(neighbourPosition.x, neighbourPosition.y) != null)
                {
                    continue;
                }

                // Are we not already adding this neighbour?
                if (IsLevelInList(neighboursToAdd, neighbourPosition.x, neighbourPosition.y))
                {
                    continue;
                }

                MapCell neighbourAttempt = AttemptNeighbourCreation(neighbourPosition.x, neighbourPosition.y, minConnectionCount);

                // Did we already add this neighbour?
                if (neighboursToAdd.Contains(neighbourAttempt))
                {
                    continue;
                }

                if (neighbourAttempt != null)
                {
                    neighboursToAdd.Add(neighbourAttempt);
                }
            }
        }

        foreach (MapCell level in neighboursToAdd)
        {
            generatedMap.Add(level);
        }
    }
}
