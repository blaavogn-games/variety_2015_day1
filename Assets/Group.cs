using System.Collections.Generic;

public class Group
{
    public List<Coord> beneath = new List<Coord>(), top = new List<Coord>(), all = new List<Coord>();
}

public class Coord
{
    public int x, y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}