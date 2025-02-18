using System.Collections.Generic;
using UnityEngine;

public class RandomPlacement : MapGenerator
{
    public override List<MapCell> GenerateMapData(int maxLevels)
    {
        base.GenerateMapData(maxLevels);

        MapCell start = new MapCell(0, 0, CellType.Start);
        generatedMap.Add(start);

        while (generatedMap.Count < maxLevels - 1)
        {
            int randomIndex = Random.Range(0, generatedMap.Count);
            MapCell targetLevel = generatedMap[randomIndex];
            Vector2Int randomDirection = GetRandomDirection();
            Vector2Int neighbourPosition = new Vector2Int(targetLevel.x + randomDirection.x, targetLevel.y + randomDirection.y);

            if (!CheckStartOverpopulation(neighbourPosition.x, neighbourPosition.y, maxLevels))
            {
                continue;
            }

            MapCell newLevel = AttemptNeighbourCreation(neighbourPosition.x, neighbourPosition.y, 0);

            if (newLevel != null)
            {
                generatedMap.Add(newLevel);
            }
        }

        AddEndToFarthestWorldLevel();
        RecalculateGridNeighbours();
        
        return generatedMap;
    }
}
