using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid
{
    public List<Cell> _cells = new List<Cell>();
    public Vector2Int _size = new Vector2Int(0, 0);

    public Grid(int size)
    {
        _size.x = size;
        _size.y = size;

        for (int y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++)
            {
                _cells.Add(new Cell(x, y, _size));
            }
        }
    }

    public Cell PickRandomCell()
    {
        List<Cell> filteredCells = _cells.FindAll(cell => cell._type != 0);

        int i = Random.Range(0, filteredCells.Count);

        return filteredCells[i];
    }

    public Cell GetRandomCellByType(RoomType type)
    {
        List<Cell> filteredCells = _cells.FindAll(cell => cell._type == type);

        int i = Random.Range(0, filteredCells.Count);

        return filteredCells[i];
    }

    public Cell GetCellAtCoord(Vector2Int coord)
    {
        return _cells[coord.y * _size.x + coord.x];
    }

    public void Reset()
    {
        foreach (Cell cell in _cells)
        {
            cell.Reset();
        }
    }
}

public class Cell
{
    public Vector2Int _coord = new Vector2Int(0, 0);

    public bool _start;
    public bool _exit;

    public RoomType _type;
    public RoomItem _item;

    public bool _visited;

    public int[] _walls = new[] { 1, 1, 1, 1 };

    public List<Vector2Int> _neighborCoords = new List<Vector2Int>();

    public Cell(int x, int y, Vector2Int size)
    {
        _coord.x = x;
        _coord.y = y;

        if (_coord.x - 1 >= 0)
            _neighborCoords.Add(new Vector2Int(_coord.x - 1, _coord.y));

        if (_coord.x + 1 < size.x)
            _neighborCoords.Add(new Vector2Int(_coord.x + 1, _coord.y));

        if (_coord.y - 1 >= 0)
            _neighborCoords.Add(new Vector2Int(_coord.x, _coord.y - 1));

        if (_coord.y + 1 < size.y)
            _neighborCoords.Add(new Vector2Int(_coord.x, _coord.y + 1));

        _start = false;
        _exit = false;

        bool blocked = Mathf.PerlinNoise((float)(x * 0.5), (float)(y * 0.5)) > 0.2;

        List<RoomType> types = new List<RoomType>() {
            RoomType.Safe,
            RoomType.UncertainSafe,
            RoomType.Death,
            RoomType.UncertainDeath,
        };
        int weightedTypeIndex = WeightedRandom(new List<int>() { 10, 50, 10, 30 });

        _type = blocked ? 0 : types[weightedTypeIndex];

        List<RoomItem> items = new List<RoomItem>() { RoomItem.None, RoomItem.Person };
        int weightedItemIndex = WeightedRandom(new List<int>() { 85, 15 });

        _item = blocked ? 0 : items[weightedItemIndex];

        _visited = false;
    }

    public int WeightedRandom(List<int> weights)
    {
        int total = weights.Sum();
        int random = Random.Range(0, total);

        for (int i = 0; i < weights.Count; i++)
        {
            if (random < weights[i])
            {
                return i;
            }

            random -= weights[i];
        }

        return 0;
    }

    public void SetType(RoomType type)
    {
        _type = type;
    }

    public List<Cell> GetNeighbors(Grid grid)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2Int coord in _neighborCoords)
        {
            Cell cell = grid._cells[coord.y * grid._size.x + coord.x];

            neighborCells.Add(cell);
        }

        return neighborCells;
    }

    public List<Cell> GetNeighborsNotBlocked(Grid grid)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2Int coord in _neighborCoords)
        {
            Cell cell = grid._cells[coord.y * grid._size.x + coord.x];

            if (cell._type != 0)
            {
                neighborCells.Add(cell);
            }
        }

        return neighborCells;
    }

    public Cell GetRandomAvailableNeighbor(Grid grid)
    {
        List<Cell> neighbors = GetNeighbors(grid).FindAll(cell => !cell._visited && cell._type != RoomType.Blocked);

        if (neighbors.Count > 0)
        {
            int i = Random.Range(0, neighbors.Count);

            return neighbors[i];
        }

        return null;
    }

    public Cell GetRandomNeighborNotVisited(Grid grid)
    {
        List<Cell> neighbors = GetNeighbors(grid).FindAll(cell => !cell._visited);

        if (neighbors.Count > 0)
        {
            int i = Random.Range(0, neighbors.Count);

            return neighbors[i];
        }

        return null;
    }

    public Cell GetRandomNeighborNotBlocked(Grid grid)
    {
        List<Cell> neighbors = GetNeighbors(grid).FindAll(cell => cell._type != RoomType.Blocked);

        if (neighbors.Count > 0)
        {
            int i = Random.Range(0, neighbors.Count);

            return neighbors[i];
        }

        return null;
    }

    public Cell GetRandomNeighbor(Grid grid)
    {
        List<Cell> neighbors = GetNeighbors(grid);

        int i = Random.Range(0, neighbors.Count);

        return neighbors[i];
    }

    public void RemoveWallsTo(Cell cell)
    {
        int x = _coord.x - cell._coord.x;

        if (x == 1)
        {
            _walls[3] = 0;
            cell._walls[1] = 0;
        }
        else if (x == -1)
        {
            _walls[1] = 0;
            cell._walls[3] = 0;
        }

        int y = _coord.y - cell._coord.y;

        if (y == 1)
        {
            _walls[0] = 0;
            cell._walls[2] = 0;
        }
        else if (y == -1)
        {
            _walls[2] = 0;
            cell._walls[0] = 0;
        }
    }

    public void Reset()
    {
        _visited = false;
    }
}
