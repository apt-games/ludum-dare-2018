using System;
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

        //Cell start = grid.PickRandomCell();

        bool startAtXEdge = Random.Range(0, 2) == 1;
        int startX = startAtXEdge ? Random.Range(0, 2) == 1 ? 0 : size - 1 : Random.Range(1, size - 1);
        int startY = startAtXEdge ? Random.Range(1, size - 1) : Random.Range(0, 2) == 1 ? 0 : size - 1;

        Cell start = grid.GetCellAtCoord(new Vector2Int(startX, startY));

        start._type = RoomType.Start;
        start._item = RoomItem.None;

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

        while (queue.Count > 0)
        {
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

        List<Node> optimalPath = new List<Node>
        {
            endNode
        };

        Node nextNode = endNode._parent;
        Node prevNode = null;

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

                int weightedTypeIndex = cell.WeightedRandom(new List<int>() { 10, 55, 10, 25 });

                // Make two first tiles in optimal path safe.
                cell._type = i >= optimalPath.Count - 3 ? RoomType.Safe : types[weightedTypeIndex];
            }

            if (prevNode != null)
            {
                cell.RemoveWallsTo(prevNode._cell);
            }

            prevNode = node;
        }

        Queue<Cell> stack = new Queue<Cell>();

        Cell current = start;
        current._visited = true;

        do
        {
            Cell next = current.GetRandomNeighborNotVisited(grid);

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
                Cell neighbor = cell.GetRandomNeighbor(grid);

                if (neighbor._type != RoomType.Blocked)
                {
                    cell.RemoveWallsTo(neighbor);
                }
            }
        }

        return grid;
    }
}
