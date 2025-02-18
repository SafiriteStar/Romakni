using System;
using System.Collections.Generic;
using UnityEngine;

public class MapCell
{
    public int x { get; private set; }
    public int y { get; private set; }
    public CellType cellType { get; private set; }
    public void SetCellType(CellType cellType) { this.cellType = cellType; }

    public List<MapCell> neighbours { get; private set; }
    public void AddNeighbour(MapCell cell) { neighbours.Add(cell); }

    public MapCell(int x, int y, CellType cellType)
    {
        this.x = x;
        this.y = y;
        this.cellType = cellType;
        this.neighbours = new List<MapCell>();
    }

    public void Print()
    {
        string info = "Cell: " + x.ToString() + ", " + y.ToString() + " Type: " + cellType.ToString() + "\nn: ";

        foreach (MapCell neighbour in neighbours)
        {
            info += "(" + neighbour.x.ToString() + ", " + neighbour.y.ToString() + ") ";
        }

        Debug.Log(info);
    }
}
