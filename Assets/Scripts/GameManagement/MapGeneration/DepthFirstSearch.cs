using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DepthFirstSearch : MapGenerator
{
    public override List<MapCell> GenerateMapData(int maxLevels)
    {
        base.GenerateMapData(maxLevels);

        // We need to keep track of what directions we have visited for each cell
        Dictionary<MapCell, Queue<Vector2Int>> remainingDirections = new Dictionary<MapCell, Queue<Vector2Int>>();
        LinkedList<MapCell> path = new LinkedList<MapCell>();

        // Create the start cell
        MapCell start = new MapCell(0, 0, CellType.Start);
        generatedMap.Add(start);
        remainingDirections[start] = GenerateRandomDirections();

        path.AddLast(start);

        while (path.Count > 0 && generatedMap.Count < maxLevels)
        {
            // Get our current level
            MapCell currentCell = path.Last.Value;

            // Are there neighbours left to explore?
            if (remainingDirections[currentCell].Count > 0)
            {
                // Yes
                // Get the next neighbour
                Vector2Int neighbourCoords = remainingDirections[currentCell].Dequeue();
                Vector2Int relativeCoords = new Vector2Int(neighbourCoords.x + currentCell.x, neighbourCoords.y + currentCell.y);

                // Let us check the next neighbour in the queue then

                MapCell neighbour = GetLevelByCoord(relativeCoords.x, relativeCoords.y);

                // Does that neighbour exist?
                // Would creating a neighbour here cause overpopulation?
                if (neighbour == null && CheckStartOverpopulation(relativeCoords.x, relativeCoords.y, maxLevels))
                {
                    // It does not exist!

                    MapCell newNeighbour = CreateNewLevel(relativeCoords.x, relativeCoords.y, maxLevels);
                    generatedMap.Add(newNeighbour);
                    remainingDirections[newNeighbour] = GenerateRandomDirections();
                    path.AddLast(newNeighbour);
                    RecalculateGridNeighbours();
                }
            }
            else
            {
                // There are no neighbours left
                // We need to go back
                path.RemoveLast();
            }
        }

        return generatedMap;
    }

    private Queue<Vector2Int> GenerateRandomDirections()
    {
        Vector2Int[] randomDirections = new Vector2Int[] { new(0, 1), new(-1, 0), new(1, 0), new(0, -1)};

        // Fisher-Yates algorithm
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        for (int i = 0; i < randomDirections.Length - 1; i++)
        {
            int r = Random.Range(i, randomDirections.Length);
            (randomDirections[r], randomDirections[i]) = (randomDirections[i], randomDirections[r]);
        }

        Queue<Vector2Int> randomDirectionQueue = new Queue<Vector2Int>();
        for (int i = 0; i < randomDirections.Length; i++)
        {
            randomDirectionQueue.Enqueue(randomDirections[i]);
        }

        return randomDirectionQueue;
    }
}
