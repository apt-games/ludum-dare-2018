using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public RoomBehaviour RoomPrefab;

    public RoomBehaviour Current;

    private readonly List<RoomBehaviour> _rooms = new List<RoomBehaviour>();

    // Use this for initialization
    public void InitiateMap()
    {
        // var map = JsonUtility.FromJson<MapStructure>(json);

        Grid grid = GenerateMap(12);

        // foreach (var room in map.Rooms)
        foreach (Cell cell in grid._cells)
        {
            Room room = new Room();
            room.x = cell._coord.x;
            room.y = cell._coord.y;
            room.walls = cell._walls;
            room.blocked = cell._blocked;
            room.type = (RoomType) cell._type;

            // spawn tiles as children
            var spawned = CreateRoom(room);
            spawned.Selected += OnRoomClicked;

            _rooms.Add(spawned);

            // start room
            if (room.type == 0)
                Current = spawned;
        }
    }

    private void Start()
    {
        // add surface mesh on current object and build from this
        var surface = Current.Floor.gameObject.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    public Grid GenerateMap(int size)
    {
        Queue<Cell> stack = new Queue<Cell>();

        Grid grid = new Grid(size);

        Cell current = grid.PickRandomCell();
        // let current = grid.cells[0];
        current._type = 0;
        current._visited = true;

        int i = 0;

        do
        {
            Cell next = current.GetRandomAvailableNeighbor(grid);

            if (next != null)
            {
                next._visited = true;

                stack.Enqueue(current);

                current.RemoveWallsTo(next);

                if (i > 3)
                {
                    List<Cell> neighbors = current.GetNeighbors(grid);
                    List<Cell> dangerousNeighbors = neighbors.FindAll(neighbor => neighbor._type == 3);

                    if (dangerousNeighbors.Count < 1)
                    {
                        Cell neighbor = current.GetRandomNeighbor(grid);

                        if (neighbor._type != 0)
                        {
                            neighbor._type = 3;
                        }
                    }
                }

                current = next;

                i++;
            }
            else
            {
                Cell prev = stack.Dequeue();

                current = prev;
            }
        } while (stack.Count != 0);

        foreach (var cell in grid._cells)
        {
            if (cell._blocked) continue;

            Cell neighbor = cell.GetRandomNeighbor(grid);

            if (neighbor != null)
            {
                cell.RemoveWallsTo(neighbor);
            }
        }

        Cell endCell = grid.PickRandomCell();
        endCell._type = 1;

        return grid;
    }

    private RoomBehaviour CreateRoom(Room room)
    {
        var go = Instantiate(RoomPrefab, new Vector3(room.x, -room.y, 0), Quaternion.identity, transform);
        go.name = $"Room ({room.x},{room.y})";
        go.SetModel(room);

        return go;
    }

    private void OnRoomClicked(RoomBehaviour room)
    {
        if (CanMoveToRoom(room.Model))
        {
            RoomSelected?.Invoke(room);
        }
    }

    private bool CanMoveToRoom(Room room)
    {
        // check if is neighbour and open room
        var diff = new Vector2Int(room.x - Current.Model.x, room.y - Current.Model.y);

        // move up
        if (diff.x == 0 && diff.y == -1 && Current.Model.walls[0] == 0)
            return true;

        // move right
        if (diff.x == 1 && diff.y == 0 && Current.Model.walls[1] == 0)
            return true;

        // move down
        if (diff.x == 0 && diff.y == 1 && Current.Model.walls[2] == 0)
            return true;

        // move left
        if (diff.x == -1 && diff.y == 0 && Current.Model.walls[3] == 0)
            return true;

        return false;
    }

    private string json =
        "{\"Rooms\":[{\"x\":0,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,0],\"type\":0},{\"x\":3,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,0],\"type\":0},{\"x\":4,\"y\":0,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,1],\"type\":1},{\"x\":0,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":1},{\"x\":1,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":3,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":4,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":0,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":0},{\"x\":1,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":2,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":1},{\"x\":3,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":4,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":5,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":0,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":1,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":0},{\"x\":3,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":4,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":5,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":0,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":3,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":4,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":5,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":0,\"y\":5,\"start\":true,\"blocked\":false,\"walls\":[0,1,1,1],\"type\":1},{\"x\":1,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":0},{\"x\":2,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0},{\"x\":3,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,1],\"type\":0},{\"x\":4,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":5,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0}]}";
}

public class MapStructure
{
    public Room[] Rooms;
}

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
        List<Cell> filteredCells = _cells.FindAll(cell => !cell._blocked);

        int i = Random.Range(0, filteredCells.Count);

        return filteredCells[i];
    }

    public Cell GetRandomCellByType(int type)
    {
        List<Cell> filteredCells = _cells.FindAll(cell => cell._type == type);

        int i = Random.Range(0, filteredCells.Count);

        return filteredCells[i];
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

    public bool _blocked;
    public int _type;

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

        _type = 2;

        _blocked = Mathf.PerlinNoise((float)(x * 0.1), (float)(y * 0.1)) > 0.45;

        _visited = false;
    }

    public void setType(int type)
    {
        _type = type;
    }

    public List<Cell> GetNeighbors(Grid grid)
    {
        List<Cell> neighborCells = new List<Cell>();

        foreach (Vector2Int coord in _neighborCoords)
        {
            Cell cell = grid._cells[coord.y * grid._size.x + coord.x];

            if (!cell._blocked)
            {
                neighborCells.Add(cell);
            }
        }

        return neighborCells;
    }

    public Cell GetRandomAvailableNeighbor(Grid grid)
    {
        List<Cell> neighbors = GetNeighbors(grid).FindAll(cell => !cell._visited);

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
