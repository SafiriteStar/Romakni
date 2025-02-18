using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellularAutomataGeneration : MapGenerator
{
    private Queue<MapCell> levelQueue;

    public override List<MapCell> GenerateMapData(int maxLevels)
    {
        base.GenerateMapData(maxLevels);

        levelQueue = new Queue<MapCell>();

        // Create empty grid
        GenerateVoidGrid(maxLevels);

        // Add the start
        generatedMap.Add(mapGrid.GetLevel(0, 0));
        levelQueue.Enqueue(mapGrid.GetLevel(0, 0));

        while (levelQueue.Count > 0)
        {
            MapCell currentCell = levelQueue.Dequeue();

            int neighbourIndex = -1;
            foreach (MapCell neighbour in currentCell.neighbours)
            {
                neighbourIndex++; // Which neighbour are we looking at?

                // Do we have all the levels we need?
                if (generatedMap.Count >= maxLevels)
                {
                    break; // Yes, get out immediately
                }

                // Check neighbour isn't already a level
                if (neighbour.cellType == CellType.Filled || neighbour.cellType == CellType.Start)
                {
                    continue; // It is, skip
                }

                // Would gaining a neighbour cause overpop?
                if (CheckOverpopulation(neighbour))
                {
                    continue; // It would, skip
                }

                // Would start become overpopulated?
                if (CheckStartOverpopulation(neighbour))
                {
                    continue;
                }

                // Is there no levels in the queue and is this the last neighbour?
                // Random will always return 0 or 1 Random.Range(0, 2)
                if (!(levelQueue.Count == 0 && neighbourIndex == currentCell.neighbours.Count - 1) && Random.Range(0, 2) == 0)
                {
                    continue;
                }

                neighbour.SetCellType(CellType.Filled);
                generatedMap.Add(neighbour);
                levelQueue.Enqueue(neighbour);
            }
        }

        // We should have a list of map levels
        // We need to prune them so that the
        // only neighboursin these cells are
        // actual possible paths
        PruneImpossibleNeighbours();

        // Now we need to add the end to the farthest level from start
        MapCell farthestLevel = dijkstra.CalculateFarthestNode(generatedMap, generatedMap.ElementAt(0));
        farthestLevel.SetCellType(CellType.End);

        return generatedMap;
    }

    // Returns true if its okay to add more around start
    // Returns false if it would overpopulate
    private bool CheckStartOverpopulation(MapCell potentialNeighbour)
    {
        MapCell start = mapGrid.GetLevel(0, 0);
        int startNeighbourCount = 0;

        // Count the number of filled neighbours in the first place
        foreach (MapCell neighbour in start.neighbours)
        {
            if (neighbour.cellType == CellType.Filled)
            {
                startNeighbourCount++;
            }
        }

        return start.neighbours.Contains(potentialNeighbour) && startNeighbourCount >= 2;
    }

    private bool CheckOverpopulation(MapCell targetCell)
    {
        int neighbourCount = 0;

        foreach (MapCell neighbour in targetCell.neighbours)
        {
            if (neighbour.cellType == CellType.Filled || neighbour.cellType == CellType.Start || neighbour.cellType == CellType.End)
            {
                neighbourCount++;
            }
        }

        return neighbourCount >= 2;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
