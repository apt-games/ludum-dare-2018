using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Graph
{
    public List<Node> _nodes = new List<Node>();
    public Dictionary<string, Node> _graph = new Dictionary<string, Node>();

    public Node _start = null;
    public Node _end = null;

    public void AddNode(Node node)
    {
        _nodes.Add(node);
        _graph.Add(node._id, node);
    }

    public Node GetNode(string id)
    {
        return _graph[id];
    }

    public Node SetStart(string id)
    {
        _start = _graph[id];
        return _start;
    }

    public Node SetEnd(string id)
    {
        _end = _graph[id];
        return _end;
    }

    public void Reset()
    {
        foreach (Node node in _nodes)
        {
            node.Reset();
        }
    }
}

public class Node
{
    public Cell _cell;
    public List<Node> _edges = new List<Node>();

    public bool _searched = false;
    public Node _parent = null;

    public string _id;

    public Node(Cell cell)
    {
        _cell = cell;
        _id = GetIdFromCell(cell);
    }

    static public string GetIdFromCell(Cell cell)
    {
        return "x:" + cell._coord.x + ",y:" + cell._coord.y;
    }

    public void AddEdge(Node neighbor)
    {
        this._edges.Add(neighbor);
    }

    public void Reset()
    {
        this._searched = false;
        this._parent = null;
    }
}
