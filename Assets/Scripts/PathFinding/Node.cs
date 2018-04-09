using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable {
    //public static int ID_Counter = 1;
    //public int ID { get; set; }

    private Node parent;
    //public int ParentID { get; set; }

    public int X { get; set; }
    public int Y { get; set; }

    public float Cost { get; set; }
    public float Heuristic { get; set; }

    public int PosInPath { get; set; }

    public TileType Type { get; set; }

    public bool IsSearched { get; set; }


    public Node()
    {
        IsSearched = false;
        Type = TileType.Wall;
    }

    public Node(int _x, int _y)
    {
        X = _x;
        Y = _y;
        IsSearched = false;
    }

    public Node(int _x, int _y, TileType _type)
    {
        X = _x;
        Y = _y;
        Type = _type;
        IsSearched = false;
    }


    public int CompareTo(object other)
    {
        Node o = (Node)other;

        float cost = Heuristic + Cost;
        float otherCost = o.Heuristic + o.Cost;

        if (cost < otherCost)
        {
            return -1;
        }
        else if (cost > otherCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public int SetParent(Node p)
    {
        parent = p;
        PosInPath = parent.PosInPath + 1;

        return PosInPath;
    }

    public Node GetParent()
    {
        return parent;
    }
}
