using System.Collections.Generic;

public class Group
{
    public List<Coord> beneath, all;
    public bool stuck = false;
}

public struct Coord
{
    public int x, y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}