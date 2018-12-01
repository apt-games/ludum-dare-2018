using System;

[Serializable]
public class Room
{
    public int x;
    public int y;

    public int[] walls;

    public bool blocked;

    public bool start;

    public int type;
}
