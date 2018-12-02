using System;

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
