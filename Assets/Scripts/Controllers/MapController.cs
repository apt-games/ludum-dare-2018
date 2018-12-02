using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Factories;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public event Action<RoomBehaviour> RoomSelected;

    public RoomBehaviour Current;

    private readonly List<RoomBehaviour> _rooms = new List<RoomBehaviour>();

    // Use this for initialization
    public void InitiateMap()
    {
        Grid grid = GenerateMap(12);

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

        //Cell start = grid.PickRandomCell();

        bool startAtXEdge = Random.Range(0, 1) == 1;
        int startX = startAtXEdge ? Random.Range(0, 1) == 1 ? 0 : size - 1 : Random.Range(1, size - 2);
        int startY = startAtXEdge ? Random.Range(1, size - 2) : Random.Range(0, 1) == 1 ? 0 : size - 1;

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

        int endX = startX == 0 || startX == size - 1 ? (startX == 0 ? size - 1 : 0) : (startX + Math.Floor(grid.width / 3 + grid.width)) % grid.width;
        int endY = startY == 0 || startY == size - 1 ? (startY == 0 ? size - 1 : 0) : (startY + Math.Floor(grid.height / 3 + grid.height)) % grid.height;

        Cell exit = grid.GetCellAtCoord(new Vector2Int(endX, endY));

        exit._type = RoomType.Exit;
        exit._item = RoomItem.None;

        Graph graph = new Graph();

        foreach (Cell cell in grid._cells)
        {
            Node node = new Node(cell);
            graph.AddNode(node);
        }

        foreach (Cell cell in grid._cells)
        {
            Node cellNode = graph.GetNode(graph.CreateId(cell));

            List<Cell> neighbors = cell.GetNeighbors(grid);

            foreach (Cell neighbor in neighbors)
            {
                Node neighborNode = graph.GetNode(graph.CreateId(cell));
                cellNode.AddEdge(neighborNode);
                neighborNode.AddEdge(cellNode);
            }
        }

        Queue<Node> queue = new Queue<Node>();

        Node startNode = graph.SetStart(graph.CreateId(start));
        Node endNode = graph.SetStart(graph.CreateId(exit));

        queue.Enqueue(startNode);

        while (queue.Count > 0) {
            Node curr = queue.Dequeue();

            if (curr == endNode)
            {
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
        }

        List<Node> optimalPath = new List<Node>();

        Node next = endNode._parent;
        Node prev = null;

        while (next != null)
        {
            optimalPath.Add(next);
            next = next._parent;
        }

        for (int i = 0; i < optimalPath.Count; i++)
        {
            Node node = optimalPath[i];
            Cell cell = node._cell;

            if (i < optimalPath.Count - 2 && cell._type != RoomType.Start && cell._type != RoomType.Exit)
            {
                List<RoomType> types = new List<RoomType>() {
                    RoomType.Safe,
                    RoomType.UncertainSafe,
                    RoomType.Death,
                    RoomType.UncertainDeath,
                };

                int weightedTypeIndex = cell.WeightedRandom(new List<int>() { 10, 50, 10, 30 });

                cell._type = types[weightedTypeIndex];
            }
            else
            {
                cell._type = RoomType.Safe;
            }

            if (prev != null)
            {
                cell.RemoveWallsTo(prev._cell);
            }

            prev = node;
        }

        return grid;
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

    //private string json =
        //"{\"Rooms\":[{\"x\":0,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,0],\"type\":0},{\"x\":3,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,0],\"type\":0},{\"x\":4,\"y\":0,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":0,\"start\":false,\"blocked\":false,\"walls\":[1,1,1,1],\"type\":1},{\"x\":0,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":1},{\"x\":1,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":1,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":3,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":4,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":5,\"y\":1,\"start\":false,\"blocked\":true,\"walls\":[1,1,1,1]},{\"x\":0,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,0,1],\"type\":0},{\"x\":1,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":2,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":1},{\"x\":3,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":4,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":1},{\"x\":5,\"y\":2,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":1},{\"x\":0,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":1,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":2,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[1,1,0,0],\"type\":0},{\"x\":3,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":4,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":5,\"y\":3,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":0},{\"x\":0,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,1],\"type\":1},{\"x\":1,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[1,0,0,0],\"type\":0},{\"x\":2,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":3,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":1},{\"x\":4,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,0],\"type\":0},{\"x\":5,\"y\":4,\"start\":false,\"blocked\":false,\"walls\":[0,1,0,1],\"type\":1},{\"x\":0,\"y\":5,\"start\":true,\"blocked\":false,\"walls\":[0,1,1,1],\"type\":1},{\"x\":1,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,1],\"type\":0},{\"x\":2,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0},{\"x\":3,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[1,0,1,1],\"type\":0},{\"x\":4,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,0,1,0],\"type\":0},{\"x\":5,\"y\":5,\"start\":false,\"blocked\":false,\"walls\":[0,1,1,0],\"type\":0}]}";
}

public class MapStructure
{
    public Room[] Rooms;
}
