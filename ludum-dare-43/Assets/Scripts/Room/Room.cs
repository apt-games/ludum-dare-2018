using System;

[Serializable]
public class Room
{
    public int x;
    public int y;

    public int[] walls;

    public bool blocked;

    public bool start;

    public RoomType type;
}

public enum RoomType
{
    Death = 0,
    Normal = 1,
}
