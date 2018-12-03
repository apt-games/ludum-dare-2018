using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MapGenerator
{
    public static List<Room> CreateGeneratedMap(int size)
    {
        var rooms = new List<Room>();
        var grid = GenerateMap(size);

        foreach (var cell in grid._cells)
        {
            rooms.Add(new Room
            {
                x = cell._coord.x,
                y = cell._coord.y,
                walls = cell._walls,
                type = (RoomType)cell._type,
                item = (RoomItem)cell._item
            });
        }

        return rooms;
    }

    public static List<Room> TutorialMap()
    {
        var rooms = new List<Room>();
        return rooms;
    }

    private static Grid GenerateMap(int size)
    {
        Grid grid = new Grid(size);

        // Cell start = grid.PickRandomCell();

        int[] edgeCoords = { 1, size - 2 };

        int startX = edgeCoords[Random.Range(0, 2)];
        int startY = edgeCoords[Random.Range(0, 2)];

        Vector2Int startCoord = new Vector2Int(startX, startY);
        Cell start = grid.GetCellAtCoord(startCoord);

        start._type = RoomType.Start;
        start._item = RoomItem.None;

        Cell exit = grid.PickRandomCell();
        int endTries = 0;

        while (Vector2Int.Distance(startCoord, exit._coord) < size - 1.5)
        {
            exit = grid.PickRandomCell();
            endTries++;
        }

        Debug.Log("Tried to find an exit " + endTries + " times, the distance is " + Vector2Int.Distance(startCoord, exit._coord).ToString());

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

        while (queue.Count > 0)
        {
            Node curr = queue.Dequeue();

            if (curr == endNode)
            {
                Debug.Log("FOUND EXIT");
                break;
            }

            List<Node> edges = curr._edges;

            edges = edges.OrderBy(x => Random.value).ToList();

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

        List<Node> optimalPath = new List<Node>
        {
            endNode
        };

        Node nextNode = endNode._parent;

        while (nextNode != null)
        {
            optimalPath.Add(nextNode);
            nextNode = nextNode._parent;
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
                cell._type = i >= optimalPath.Count - 3 ? RoomType.UncertainSafe : types[weightedTypeIndex];
            }
        }


        foreach (var cell in grid._cells)
        {
            if (cell._type == RoomType.Blocked)
            {
                List<Cell> neighbors = cell.GetNeighbors(grid);

                foreach (Cell neighbor in neighbors)
                {
                    cell.AddWallsTo(neighbor);
                }

                // Debug.Log("CELL: " + cell._coord.x + "," + cell._coord.y + ", type: " + cell._type);
            }
            else
            {
                List<Cell> neighbors = cell.GetNeighbors(grid);

                foreach (Cell neighbor in neighbors)
                {
                    if (neighbor._type != RoomType.Blocked)
                    {
                        cell.RemoveWallsTo(neighbor);
                    }
                }
            }
        }

        List<Cell> startNeighbors = start.GetNeighbors(grid);

        startNeighbors = startNeighbors.OrderBy(x => Random.value).ToList();

        start.RemoveWallsTo(startNeighbors[0]);
        startNeighbors[0]._type = RoomType.UncertainSafe;
        startNeighbors[0]._item = RoomItem.Person;

        start.RemoveWallsTo(startNeighbors[1]);
        startNeighbors[1]._type = RoomType.UncertainSafe;
        startNeighbors[1]._item = RoomItem.Person;

        if (startNeighbors[2]._type != RoomType.Blocked)
        {
            startNeighbors[2]._item = RoomItem.None;
        }

        if (startNeighbors[3]._type != RoomType.Blocked)
        {
            startNeighbors[3]._item = RoomItem.None;
        }

        return grid;
    }
}
