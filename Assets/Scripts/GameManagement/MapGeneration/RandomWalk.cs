using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomWalk : MapGenerator
{
    private bool cyclicalMode = false;
    public override List<MapCell> GenerateMapData(int maxLevels)
    {
        base.GenerateMapData(maxLevels);

        cyclicalMode = maxLevels == -1;

        if (cyclicalMode)
        {
            maxLevels = 3;
        }

        MapCell start = new MapCell(0, 0, CellType.Start);
        generatedMap.Add(start);
        Vector2Int targetPosition = new Vector2Int(0, 0);
        Vector2Int previousTargetPosition = new Vector2Int(0, 0);

        int failSale = 0;
        while (generatedMap.Count < maxLevels && failSale < 1000)
        {
            failSale++;
            Vector2Int newDirection = targetPosition + GetRandomDirection();
            if (CheckStartOverpopulation(newDirection.x, newDirection.y, maxLevels) && !CheckLevelType(newDirection.x, newDirection.y, CellType.Start))
            {
                if (CyclicalCheck(previousTargetPosition, newDirection))
                {
                    continue;
                }
                // We won't be messing with start here
                previousTargetPosition = targetPosition;
                targetPosition = newDirection;

                // Are we somewhere with a filled level?
                if (CheckLevelType(targetPosition.x, targetPosition.y, CellType.Filled))
                {
                    continue; // Ignore
                }
                else
                {
                    // Not a filled level
                    // Create a new level!
                    generatedMap.Add(CreateNewLevel(targetPosition.x, targetPosition.y, maxLevels));
                }
            }
        }

        if (cyclicalMode)
        {
            generatedMap.Last().SetCellType(CellType.Empty);
        }

        RecalculateGridNeighbours();

        return generatedMap;
    }

    private bool CyclicalCheck(Vector2Int previousDirection, Vector2Int newDirection)
    {
        if (!cyclicalMode)
        {
            return false;
        }

        return previousDirection.x == newDirection.x && previousDirection.y == newDirection.y;
    }
}
