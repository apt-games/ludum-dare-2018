using System;
using System.Collections.Generic;
using Assets.Scripts.Factories;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public RoomBehaviour Current { get; private set; }

    private readonly List<RoomBehaviour> _rooms = new List<RoomBehaviour>();

    private const int MAP_SIZE = 12;

    // Use this for initialization
    public void InitiateMap()
    {
        Grid grid = GenerateMap(MAP_SIZE);

        // foreach (var room in map.Rooms)
        foreach (Cell cell in grid._cells)
        {
            Room room = new Room
            {
                x = cell._coord.x,
                y = cell._coord.y,
                walls = cell._walls,
                type = (RoomType)cell._type,
                item = (RoomItem)cell._item
            };

            // spawn tiles as children
            var spawned = RoomFactory.Create(room);
            spawned.transform.parent = transform;
            spawned.Selected += OnRoomClicked;

            _rooms.Add(spawned);

            // start room
            if (room.type == RoomType.Start)
                Current = spawned;
        }

        SetCurrentRoom(Current);
    }

    public void SetCurrentRoom(RoomBehaviour room)
    {
        Current = room;
        Current.SetVisited();
        ShowNeighbours(room);
    }

    private void Start()
    {
        // add surface mesh on current object and build from this
        var surface = Current.Floor.gameObject.AddComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }

    private void OnRoomClicked(RoomBehaviour room)
    {
        if (CanMoveToRoom(room.Model))
        {
            RoomSelected?.Invoke(room);
        }
    }

    private void ShowNeighbours(RoomBehaviour room)
    {
        var i = _rooms.IndexOf(room);
        var walls = room.Model.walls;

        for (int d = 0; d < walls.Length; d++)
        {
            if (walls[d] == 0) //dor
            {
                switch (d)
                {
                    //top
                    case 0:
                        _rooms[i - MAP_SIZE].SetDiscovered();
                        break;
                    //right
                    case 1:
                        _rooms[i + 1].SetDiscovered();
                        break;
                    //bottom
                    case 2:
                        _rooms[i + MAP_SIZE].SetDiscovered();
                        break;
                    //left
                    case 3:
                        _rooms[i - 1].SetDiscovered();
                        break;
                    default:
                        Debug.LogError("Wah? this shouldn't happen, should only have 4 walls");
                        break;
                }
            }
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

    public Grid GenerateMap(int size)
    {
        Queue<Cell> stack = new Queue<Cell>();

        Grid grid = new Grid(size);

        //Cell start = grid.PickRandomCell();

        bool startAtXEdge = Random.Range(0, 2) == 1;
        int startX = startAtXEdge ? Random.Range(0, 2) == 1 ? 0 : size - 1 : Random.Range(1, size - 1);
        int startY = startAtXEdge ? Random.Range(1, size - 1) : Random.Range(0, 2) == 1 ? 0 : size - 1;

        Cell start = grid.GetCellAtCoord(new Vector2Int(startX, startY));

        start._type = RoomType.Start;
        start._item = RoomItem.None;
        start._visited = true;

        Cell current = start;

        do
        {
            Cell next = current.GetRandomAvailableNeighbor(grid);

            if (next != null)
            {
                next._visited = true;

                stack.Enqueue(current);

                current.RemoveWallsTo(next);

                current = next;
            }
            else
            {
                Cell prev = stack.Dequeue();

                current = prev;
            }
        } while (stack.Count > 0);

        //foreach (var cell in grid._cells)
        //{
        //    if (cell._type == 0) continue;

        //    Cell neighbor = cell.GetRandomNeighbor(grid);

        //    if (neighbor != null)
        //    {
        //        cell.RemoveWallsTo(neighbor);
        //    }
        //}

        //Cell exit = grid.PickRandomCell();

        int endX = startX == 0 || startX == size - 1 ? (startX == 0 ? size - 1 : 0) : (startX + (int)Math.Round(size / 3.0) + size) % size;
        int endY = startY == 0 || startY == size - 1 ? (startY == 0 ? size - 1 : 0) : (startY + (int)Math.Round(size / 3.0) + size) % size;

        Cell exit = grid.GetCellAtCoord(new Vector2Int(endX, endY));

        exit._type = RoomType.Exit;
        exit._item = RoomItem.None;

        Graph graph = new Graph();

        foreach (Cell cell in grid._cells)
        {
            Node node = new Node(cell);
            graph.AddNode(node);
        }

        //foreach (Node node in graph._nodes)
        //{
        //    List<Cell> neighbors = node._cell.GetNeighbors(grid);

        //    foreach (Cell neighbor in neighbors)
        //    {
        //        string neighborId = Node.GetIdFromCell(neighbor);
        //        Node neighborNode = graph.GetNode(neighborId);
        //        node.AddEdge(neighborNode);
        //        neighborNode.AddEdge(node);
        //    }
        //}

        foreach (Cell cell in grid._cells)
        {
            string cellId = Node.GetIdFromCell(cell);
            Node cellNode = graph.GetNode(cellId);

            List<Cell> neighbors = cell.GetNeighbors(grid);

            foreach (Cell neighbor in neighbors)
            {
                string neighborId = Node.GetIdFromCell(neighbor);
                Node neighborNode = graph.GetNode(neighborId);
                cellNode.AddEdge(neighborNode);
                neighborNode.AddEdge(cellNode);
            }
        }

        Debug.Log("NODES IN GRAPH: " + graph._graph.Count);

        Queue<Node> queue = new Queue<Node>();

        string startId = Node.GetIdFromCell(start);
        Node startNode = graph.SetStart(startId);

        string endId = Node.GetIdFromCell(exit);
        Node endNode = graph.SetStart(endId);

        queue.Enqueue(startNode);
        startNode._searched = true;

        int searchCount = 0;

        while (queue.Count > 0) {
            Node curr = queue.Dequeue();

            if (curr == endNode)
            {
                Debug.Log("FOUND EXIT");
                break;
            }

            List<Node> edges = curr._edges;

            foreach (Node edge in edges)
            {
                if (!edge._searched)
                {
                    edge._searched = true;
                    edge._parent = curr;
                    queue.Enqueue(edge);
                }
            }

            searchCount++;
        }

        Debug.Log("SEARCHED " + searchCount + " nodes.");

        List<Node> optimalPath = new List<Node>();

        Node n = endNode._parent;
        Node p = null;

        while (n != null)
        {
            optimalPath.Add(n);
            n = n._parent;
        }

        Debug.Log("There's " + optimalPath.Count + " steps between start and exit");

        for (int i = 0; i < optimalPath.Count; i++)
        {
            Node node = optimalPath[i];
            Cell cell = node._cell;

            if (cell._type != RoomType.Start && cell._type != RoomType.Exit)
            {
                List<RoomType> types = new List<RoomType>() {
                    RoomType.Safe,
                    RoomType.UncertainSafe,
                    RoomType.Death,
                    RoomType.UncertainDeath,
                };

                int weightedTypeIndex = cell.WeightedRandom(new List<int>() { 10, 50, 10, 30 });

                // Make two first tiles in optimal path safe.
                cell._type = i >= optimalPath.Count - 3 ? RoomType.Safe : types[weightedTypeIndex];
            }

            if (p != null)
            {
                cell.RemoveWallsTo(p._cell);
            }

            p = node;
        }

        return grid;
    }
}

public class MapStructure
{
    public Room[] Rooms;
}
