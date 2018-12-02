﻿using System;
using System.Linq;

[Serializable]
public class Room
{
    public int x;
    public int y;

    public int[] walls;

    public bool blocked;

    public RoomType type;
    public RoomItem item;
}

public enum RoomType
{
    Blocked = 0,
    Start = 1,
    Exit = 2,
    Safe = 3,
    UncertainSafe = 4,
    Death = 5,
    UncertainDeath = 6,
}

public enum RoomItem
{
    None = 0,
    Person = 1,
}

public static class EnumExtensions
{
    public static bool IsOneOf(this RoomType enumeration, params RoomType[] enums)
    {
        return enums.Contains(enumeration);
    }
}
