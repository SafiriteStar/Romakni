using System.Collections.Generic;
using UnityEngine;

// Good ol Dijkstra
public class Dijkstra
{
    public MapCell CalculateFarthestNode(List<MapCell> graph, MapCell source)
    {
        Dictionary<MapCell, int> dist = new Dictionary<MapCell, int>();
        Dictionary<MapCell, MapCell> prev = new Dictionary<MapCell, MapCell>();
        List<MapCell> unvisited = new List<MapCell>();

        foreach (MapCell cell in graph)
        {
            dist[cell] = int.MaxValue;
            prev[cell] = null;

            unvisited.Add(cell);
        }

        dist[source] = 0;
        while (unvisited.Count > 0)
        {
            MapCell currentVertex = FindMinDistanceVertex(unvisited, dist);
            unvisited.Remove(currentVertex);

            foreach (MapCell neighbour in currentVertex.neighbours)
            {
                if (!unvisited.Contains(neighbour)) { continue; }

                int totalNeighbourCost = dist[currentVertex] + 1; // Always costs one to go from u to neighbour

                if (totalNeighbourCost < dist[neighbour])
                {
                    dist[neighbour] = totalNeighbourCost;
                    prev[neighbour] = currentVertex;
                }
            }
        }

        // Distances have been calculated
        // Now we just find the highest one
        return FindMaxDistanceVertex(graph, dist);
    }

    private MapCell FindMinDistanceVertex(List<MapCell> cellList, Dictionary<MapCell, int> dist)
    {
        MapCell closest = null;
        int lowestDistance = int.MaxValue;

        foreach (MapCell cell in cellList)
        {
            if (dist[cell] < lowestDistance)
            {
                closest = cell;
            }
        }

        return closest;
    }

    private MapCell FindMaxDistanceVertex(List<MapCell> cellList, Dictionary<MapCell, int> dist)
    {
        MapCell closest = null;
        int highestDistance = int.MinValue;

        foreach (MapCell cell in cellList)
        {
            if (dist[cell] > highestDistance)
            {
                closest = cell;
            }
        }

        return closest;
    }
}
