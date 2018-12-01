using System;

[Serializable]
public class Room
{
    public int x;
    public int y;

    public int[] walls;

    public bool blocked;

    public RoomType type;
}

public enum RoomType
{
    Start = 0,
    Exit = 1,
    Normal = 2,
    Death = 3,
}
