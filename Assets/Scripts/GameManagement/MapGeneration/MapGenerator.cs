using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// We always assume that start is at 0, 0
// Which means we can have negative coords
// So we start at a negative to determine
// the coordinates of a cell

// The grid also moves outwardly from the
// start cell based on the maximum number
// of allowed cells. So a max of 5 would
// look like this grid:

// 
//                E
//                4
//                3
//                2
//                1
// E -4 -3 -2 -1  S  1  2  3  4  E
//               -1
//               -2
//               -3
//               -4
//                E
// 
// S -> Start
// E -> Possible position for end


public class MapGrid
{
    private MapCell[][] gridCells;
    private int size;
    private int offset;

    public MapGrid(int mapSize)
    {
        size = mapSize + 1 + mapSize;
        offset = mapSize;

        // Initialize void map
        gridCells = new MapCell[size][];

        for (int i = 0; i < gridCells.Length; i++)
        {
            gridCells[i] = new MapCell[size];

            for (int j = 0; j < gridCells[i].Length; j++)
            {
                // Cells will mark themselves with the offset coordinates
                gridCells[i][j] = new MapCell(j - mapSize, i - mapSize, CellType.Void);
            }
        }

        // Set all neighbours
        for (int i = 0; i < gridCells.Length; i++)
        {
            for (int j = 0; j < gridCells[i].Length; j++)
            {
                // Careful with the boundries!
                if (i < size - 1) { gridCells[i][j].AddNeighbour(gridCells[i + 1][j]); } // Top neighbour
                if (i > 0)        { gridCells[i][j].AddNeighbour(gridCells[i - 1][j]); } // Bottom neighbour
                if (j < size - 1) { gridCells[i][j].AddNeighbour(gridCells[i][j + 1]); } // Right neighbour
                if (j > 0)        { gridCells[i][j].AddNeighbour(gridCells[i][j - 1]); } // Left neighbour
            }
        }

        // Set 0, 0 as the start
        SetLevel(0, 0, CellType.Start);
    }

    public MapCell GetLevel(int xIndex, int yIndex)
    {
        return gridCells[xIndex + offset][yIndex + offset];
    }

    public void SetLevel(int xIndex, int yIndex, CellType cellType)
    {
        gridCells[xIndex + offset][yIndex + offset].SetCellType(cellType);
    }
    
    public void PrintAllLevels()
    {
        for (int i = 0; i < gridCells.Length; i++)
        {
            string rowData = "";
            for (int j = 0; j < gridCells[i].Length; j++)
            {
                rowData += "(" + gridCells[i][j].x + ", " + gridCells[i][j].y + ") | ";
            }

            Debug.Log(rowData);
        }
    }
}

public class MapGenerator : MonoBehaviour
{
    protected MapGenerationType generationType;
    protected MapGrid mapGrid;
    protected List<MapCell> generatedMap;
    protected Dijkstra dijkstra;
    protected Vector2Int[] cardinalDirections = new Vector2Int[4]
        {
            new Vector2Int(0, 1),   // Up
            new Vector2Int(0, -1),  // Down
            new Vector2Int(-1, 0),  // Left
            new Vector2Int(1, 0)    // Right
        };

    public virtual List<MapCell> GenerateMapData(int maxLevels)
    {
        dijkstra = new Dijkstra();
        generatedMap = new List<MapCell>();
        return generatedMap;
    }

    protected void GenerateVoidGrid(int expansionSize)
    {
        mapGrid = new MapGrid(expansionSize);
    }

    protected void PruneImpossibleNeighbours()
    {
        foreach (MapCell level in generatedMap)
        {
            List<MapCell> neighbourPruneList = new List<MapCell>();

            foreach (MapCell neighbour in level.neighbours)
            {
                if (!generatedMap.Contains(neighbour))
                {
                    neighbourPruneList.Add(neighbour);
                }
            }

            foreach (MapCell targetNeighbour in neighbourPruneList)
            {
                level.neighbours.Remove(targetNeighbour);
            }
        }
    }

    // Returns true if no overpop occurs
    // Also stops end from spawning next to start
    protected bool CheckStartOverpopulation(int x, int y, int maxLevels)
    {
        return !(x > -2 && x < 2 &&
                  y > -2 && y < 2 &&
                  (generatedMap.ElementAt(0).neighbours.Count >= 2 ||
                  generatedMap.Count >= maxLevels - 1));
    }

    protected MapCell CreateNewLevel(int x, int y, int maxLevels)
    {
        if (generatedMap.Count < maxLevels - 1)
        {
            return new MapCell(x, y, CellType.Filled);
        }
        else
        {
            return new MapCell(x, y, CellType.End);
        }
    }

    protected MapCell GetLevelByCoord(int x, int y)
    {
        for (int i = 0; i < generatedMap.Count; i++)
        {
            if (generatedMap[i].x == x && generatedMap[i].y == y)
            {
                return generatedMap[i];
            }
        }

        return null;
    }


    // Returns true if level is the said level
    protected bool CheckLevelType(int x, int y, CellType levelType)
    {
        foreach (MapCell Level in generatedMap)
        {
            if (Level.x == x && Level.y == y && Level.cellType == levelType)
            {
                return true;
            }
        }

        return false;
    }
    protected int CalculateWorldDistance(MapCell source, MapCell target)
    {
        return CalculateWorldDistance(source.x, source.y, target.x, target.y);
    }

    protected int CalculateWorldDistance(int sourceX, int sourceY, int targetX, int targetY)
    {
        return Mathf.CeilToInt(Mathf.Sqrt(Mathf.Pow((float)(sourceX - targetX),2) + Mathf.Pow((float)(sourceY - targetY), 2)));
    }

    protected void RecalculateGridNeighbours()
    {
        foreach (MapCell level in generatedMap)
        {
            level.neighbours.Clear();
            foreach (MapCell potentialNeighbour in generatedMap)
            {
                int totalDistance = CalculateWorldDistance(level, potentialNeighbour);

                if (totalDistance < 2 && totalDistance != 0)
                {
                    // We have a neighbour
                    level.neighbours.Add(potentialNeighbour);
                }
            }
        }
    }
    
    protected Vector2Int GetRandomDirection()
    {
        int cardinalIndex = Random.Range(0, 3);

        switch (cardinalIndex)
        {
            case 0: return new Vector2Int(0, 1);
            case 1: return new Vector2Int(0, -1);
            case 2: return new Vector2Int(1, 0);
            case 3: return new Vector2Int(-1, 0);
            default: return new Vector2Int(0, 1);
        }
    }

    protected void PruneEmptyAndVoid()
    {
        List<MapCell> levelsToRemove = new List<MapCell>();

        foreach (MapCell level in generatedMap)
        {
            if (level.cellType == CellType.Empty || level.cellType == CellType.Void)
            {
                levelsToRemove.Add(level);
            }
        }

        foreach (MapCell level in levelsToRemove)
        {
            generatedMap.Remove(level);
        }
    }

    protected MapCell FarthestLevelFromStart()
    {
        MapCell farthestLevel = generatedMap.ElementAt(0);
        float farthestDistance = 0;

        foreach (MapCell level in generatedMap)
        {
            float currentDistance = CalculateWorldDistance(generatedMap.ElementAt(0), level);

            if (currentDistance > farthestDistance)
            {
                farthestDistance = currentDistance;
                farthestLevel = level;
            }
        }

        return farthestLevel;
    }

    protected bool CheckMinimumConnections(int x, int y, int minConnectionCount)
    {
        int neighbourCount = 0;
        foreach (MapCell level in generatedMap)
        {
            // Does this level neighbour our target position?
            if (CalculateWorldDistance(x, y, level.x, level.y) == 1)
            {
                neighbourCount++; // It does
            }
        }
        // Are the number of neighbours less than the minimmum connections?
        return neighbourCount >= minConnectionCount;
    }

    protected MapCell AttemptNeighbourCreation(int x, int y, int minConnectionCount)
    {
        // Make sure level doesn't exist
        // Make sure we have enough connections to make it
        if (GetLevelByCoord(x, y) == null && CheckMinimumConnections(x, y, minConnectionCount))
        {
            return CreateNewLevel(x, y, generatedMap.Count + 2);
        }

        return null;
    }

    protected void AddEndToFarthestWorldLevel()
    {
        // Get the farthest cell
        MapCell farthestFromStart = FarthestLevelFromStart();
        for (int i = 0; i < cardinalDirections.Length; i++)
        {
            MapCell potentialEnd = AttemptNeighbourCreation(farthestFromStart.x + cardinalDirections[i].x, farthestFromStart.y + cardinalDirections[i].y, 0);

            if (potentialEnd == null)
            {
                continue;
            }

            potentialEnd.SetCellType(CellType.End);
            generatedMap.Add(potentialEnd);

            break;
        }
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
